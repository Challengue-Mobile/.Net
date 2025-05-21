using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_.Net.Models;
using API_.Net.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using API_.Net.Examples;
using Microsoft.AspNetCore.Http;

namespace API.Net.Controllers
{
    /// <summary>
    /// API para gerenciamento de localizações
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui localizações de motos")]
    public class LocalizacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocalizacoesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as localizações registradas
        /// </summary>
        /// <returns>Lista de localizações</returns>
        /// <response code="200">Retorna a lista de localizações</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todas as localizações",
            Description = "Obtém uma lista de todas as localizações de motos registradas no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LocalizacoesListResponseExample))]
        public async Task<ActionResult<IEnumerable<Localizacao>>> GetLocalizacoes()
        {
            return Ok(await _context.Localizacoes.ToListAsync());
        }

        /// <summary>
        /// Obtém uma localização específica pelo ID
        /// </summary>
        /// <param name="id">ID da localização</param>
        /// <returns>Dados da localização solicitada</returns>
        /// <response code="200">Retorna a localização</response>
        /// <response code="404">Se a localização não for encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém uma localização pelo ID",
            Description = "Busca e retorna informações detalhadas de uma localização específica")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LocalizacaoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Localizacao>> GetLocalizacao(int id)
        {
            var localizacao = await _context.Localizacoes.FindAsync(id);

            if (localizacao == null)
            {
                return NotFound();
            }

            return localizacao;
        }

        /// <summary>
        /// Busca localizações de uma moto específica
        /// </summary>
        /// <param name="motoId">ID da moto</param>
        /// <returns>Lista de localizações da moto, ordenadas pela data mais recente</returns>
        /// <response code="200">Retorna a lista de localizações da moto</response>
        [HttpGet("moto/{motoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Busca localizações por moto",
            Description = "Obtém o histórico de localizações de uma moto específica, ordenadas da mais recente para a mais antiga")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LocalizacoesListResponseExample))]
        public async Task<ActionResult<IEnumerable<Localizacao>>> GetLocalizacoesByMoto(int motoId)
        {
            return await _context.Localizacoes
                .Where(l => l.ID_MOTO == motoId)
                .OrderByDescending(l => l.DATA_HORA)
                .ToListAsync();
        }

        /// <summary>
        /// Registra uma nova localização
        /// </summary>
        /// <param name="localizacao">Dados da localização a ser registrada</param>
        /// <returns>Localização registrada com seu ID</returns>
        /// <response code="201">Retorna a localização recém registrada</response>
        /// <response code="400">Se os dados da localização são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/Localizacoes
        ///     {
        ///        "posicao_x": -23.550520,
        ///        "posicao_y": -46.633308,
        ///        "id_moto": 1,
        ///        "id_patio": null
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Registra uma nova localização",
            Description = "Cria um novo registro de localização de moto no sistema")]
        [SwaggerRequestExample(typeof(Localizacao), typeof(LocalizacaoRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(LocalizacaoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<Localizacao>> PostLocalizacao(Localizacao localizacao)
        {
            localizacao.DATA_HORA = DateTime.Now;
            
            _context.Localizacoes.Add(localizacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocalizacao", new { id = localizacao.ID_LOCALIZACAO }, localizacao);
        }

        /// <summary>
        /// Atualiza os dados de uma localização existente
        /// </summary>
        /// <param name="id">ID da localização a ser atualizada</param>
        /// <param name="localizacao">Novos dados da localização</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se a localização foi atualizada com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se a localização não for encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza uma localização",
            Description = "Atualiza informações de uma localização existente no sistema")]
        [SwaggerRequestExample(typeof(Localizacao), typeof(LocalizacaoRequestExample))]
        public async Task<IActionResult> PutLocalizacao(int id, Localizacao localizacao)
        {
            if (id != localizacao.ID_LOCALIZACAO)
            {
                return BadRequest();
            }

            _context.Entry(localizacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalizacaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Remove uma localização do sistema
        /// </summary>
        /// <param name="id">ID da localização a ser removida</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se a localização foi removida com sucesso</response>
        /// <response code="404">Se a localização não for encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui uma localização",
            Description = "Remove permanentemente uma localização do sistema")]
        public async Task<IActionResult> DeleteLocalizacao(int id)
        {
            var localizacao = await _context.Localizacoes.FindAsync(id);
            if (localizacao == null)
            {
                return NotFound();
            }

            _context.Localizacoes.Remove(localizacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocalizacaoExists(int id)
        {
            return _context.Localizacoes.Any(e => e.ID_LOCALIZACAO == id);
        }
    }
}