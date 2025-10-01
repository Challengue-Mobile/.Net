#!/usr/bin/env bash
set -euo pipefail

# =========================
# build-fast.sh
# - Habilita BuildKit (se possível)
# - Gera/atualiza .dockerignore para reduzir contexto
# - Mostra tamanho do contexto e diretórios pesados
# - Faz limpeza opcional (git clean / docker prune)
# - Build com buildx (ou fallback pro build legado)
# - Push opcional para ACR
# =========================

# -------- Parâmetros --------
IMG_DEFAULT_LOCAL="mottoth-api:dev"
IMG_DEFAULT_ACR="${ACR:-}/mottoth-api:v2-inmemory"  # se ACR=acrmottoth....azurecr.io estiver no env
IMG="${IMG:-${IMG_DEFAULT_ACR:-$IMG_DEFAULT_LOCAL}}"

PUSH="${PUSH:-false}"              # use PUSH=true para dar push
CLEAN="${CLEAN:-false}"            # use CLEAN=true para rodar git clean/docker prune
NO_CACHE="${NO_CACHE:-false}"      # use NO_CACHE=true para build sem cache
DOCKERFILE="${DOCKERFILE:-Dockerfile}"

# -------- Funções utilitárias --------
info(){ echo -e "\033[1;36m[INFO]\033[0m $*"; }
warn(){ echo -e "\033[1;33m[WARN]\033[0m $*"; }
err() { echo -e "\033[1;31m[ERRO]\033[0m $*" >&2; }

have(){ command -v "$1" >/dev/null 2>&1; }

line(){ printf '%*s\n' "${COLUMNS:-80}" '' | tr ' ' -; }

# -------- 1) Habilitar BuildKit --------
enable_buildkit(){
  if [[ "${DOCKER_BUILDKIT:-}" != "1" ]]; then
    export DOCKER_BUILDKIT=1
    info "BuildKit habilitado (DOCKER_BUILDKIT=1)."
  else
    info "BuildKit já está habilitado."
  fi
}

# -------- 2) Gerar/atualizar .dockerignore --------
ensure_dockerignore(){
  local path=".dockerignore"
  if [[ ! -f "$path" ]]; then
    info "Criando .dockerignore..."
    cat > "$path" <<'EOF'
bin/
obj/
out/
.git/
.vs/
**/*.user
**/*.suo
**/*.db
**/*.mdf
**/*.ldf
node_modules/
Dockerfile
docker-compose*.yml
*.zip
*.tar
*.tgz
*.log
EOF
  else
    info ".dockerignore já existe. Garantindo entradas essenciais…"
    # acrescenta linhas essenciais se faltarem
    for p in bin/ obj/ out/ .git/ node_modules/; do
      grep -qxF "$p" "$path" || echo "$p" >> "$path"
    done
  fi
}

# -------- 3) Mostrar tamanho do contexto --------
show_context_size(){
  line
  info "Tamanho do contexto (raiz):"
  du -sh . || true
  line
  info "Top de diretórios (nível 1):"
  du -h --max-depth=1 | sort -h || true
  line
  info "bin/ obj/ out/ node_modules/ (se existirem):"
  du -sh bin obj out node_modules 2>/dev/null || true
  line
}

# -------- 4) Limpeza opcional --------
optional_cleanup(){
  if [[ "$CLEAN" == "true" ]]; then
    warn "LIMPANDO arquivos não versionados e cache do Docker (opcional habilitado)…"
    if have git; then
      git clean -xfd || true
    fi
    docker system prune -af || true
  else
    info "Limpeza opcional desativada (CLEAN=false)."
  fi
}

# -------- 5) Build (buildx se disponível) --------
do_build(){
  local no_cache_flag=()
  if [[ "$NO_CACHE" == "true" ]]; then
    no_cache_flag+=(--no-cache)
  fi

  line
  info "Iniciando build da imagem: $IMG"
  info "Dockerfile: $DOCKERFILE"
  info "NO_CACHE=$NO_CACHE"
  line

  if have docker buildx; then
    info "Usando docker buildx (com --progress=plain e cache de NuGet via Dockerfile se definido)…"
    docker buildx build \
      -t "$IMG" \
      -f "$DOCKERFILE" \
      . \
      --progress=plain \
      "${no_cache_flag[@]}"
  else
    warn "docker buildx não encontrado. Usando docker build (sem --progress)…"
    # se alguém deixou DOCKER_BUILDKIT=0, removemos pra reduzir confusão
    unset DOCKER_BUILDKIT || true
    docker build \
      -t "$IMG" \
      -f "$DOCKERFILE" \
      . \
      "${no_cache_flag[@]}"
  fi

  info "Build finalizado: $IMG"
}

# -------- 6) Push opcional --------
maybe_push(){
  if [[ "$PUSH" == "true" ]]; then
    if [[ "$IMG" != *".azurecr.io/"* ]]; then
      warn "Imagem não parece apontar para um ACR (azurecr.io). Pulei o push."
      return 0
    fi
    local acr_host
    acr_host="$(echo "$IMG" | awk -F/ '{print $1}')"

    info "Logando no ACR: $acr_host"
    az acr login -n "${acr_host%%.*}"  # pega o nome antes do primeiro ponto
    info "Fazendo push: $IMG"
    docker push "$IMG"
  else
    info "Push desativado (PUSH=false)."
  fi
}

# -------- 7) Diagnóstico rápido de rede (NuGet) --------
net_diag(){
  line
  info "Diagnóstico rápido de rede (NuGet)…"
  if have nslookup; then
    nslookup api.nuget.org || true
  else
    warn "nslookup não encontrado."
  fi
  if have ping; then
    ping -c 2 api.nuget.org || true
  else
    warn "ping não encontrado."
  fi
  line
}

# -------- Execução --------
main(){
  info "Imagem alvo: $IMG"
  enable_buildkit
  ensure_dockerignore
  show_context_size
  optional_cleanup
  net_diag
  do_build
  maybe_push

  line
  info "PRONTO! Para rodar local:"
  cat <<EOF

docker run --rm -p 8080:8080 \\
  -e ASPNETCORE_ENVIRONMENT=Development \\
  -e ASPNETCORE_URLS="http://+:8080" \\
  -e USE_INMEMORY=true \\
  "$IMG"

# Testes:
curl -i http://localhost:8080/
curl -i http://localhost:8080/api/ping
curl -i http://localhost:8080/swagger/v1/swagger.json
EOF
  line
}

main "$@"
