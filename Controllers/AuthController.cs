using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace API_.Net.Controllers
{
    /// <summary>
    /// Controller responsável pela autenticação e geração de tokens JWT
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Realiza login e gera um token JWT para autenticação
        /// </summary>
        /// <param name="request">Credenciais do usuário (username e password)</param>
        /// <returns>Token JWT e informações do usuário</returns>
        /// <response code="200">Login realizado com sucesso - retorna o token JWT</response>
        /// <response code="400">Dados de entrada inválidos</response>
        /// <response code="401">Credenciais inválidas</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/v1/auth/login
        ///     {
        ///       "username": "admin",
        ///       "password": "admin123"
        ///     }
        ///     
        /// Usuários de teste disponíveis:
        /// - admin / admin123 (Administrador)
        /// - user / user123 (Usuário comum)
        /// - gestor / gestor123 (Gestor de frota)
        /// 
        /// **IMPORTANTE:** Em produção, valide as credenciais com banco de dados e use hash de senha!
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new 
                { 
                    message = "Dados inválidos",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            // ATENÇÃO: Em produção, validar com banco de dados usando hash de senha (bcrypt, argon2)
            // Essa é apenas uma implementação de demonstração
            var (isValid, role) = ValidateCredentials(request.Username, request.Password);
            
            if (!isValid)
            {
                _logger.LogWarning("Tentativa de login falhou para o usuário: {Username}", request.Username);
                return Unauthorized(new { message = "Usuário ou senha inválidos" });
            }

            var token = GenerateJwtToken(request.Username, role);
            var expirationMinutes = Convert.ToInt32(_configuration["JwtSettings:ExpirationMinutes"] ?? "60");

            _logger.LogInformation("Login realizado com sucesso para o usuário: {Username}", request.Username);

            return Ok(new LoginResponse
            {
                Token = token,
                Username = request.Username,
                Role = role,
                ExpiresIn = expirationMinutes * 60, // em segundos
                TokenType = "Bearer",
                Message = "Login realizado com sucesso"
            });
        }

        /// <summary>
        /// Valida as credenciais do usuário (método de exemplo)
        /// </summary>
        /// <remarks>
        /// ⚠️ EM PRODUÇÃO:
        /// - Buscar usuário no banco de dados
        /// - Validar senha usando BCrypt.Verify ou similar
        /// - Implementar rate limiting para prevenir brute force
        /// - Adicionar logs de auditoria
        /// - Implementar bloqueio após múltiplas tentativas falhas
        /// </remarks>
        private (bool isValid, string role) ValidateCredentials(string username, string password)
        {
            // Usuários de demonstração
            var users = new Dictionary<string, (string password, string role)>
            {
                { "admin", ("admin123", "Administrator") },
                { "user", ("user123", "User") },
                { "gestor", ("gestor123", "Manager") }
            };

            if (users.TryGetValue(username.ToLower(), out var userData))
            {
                if (userData.password == password)
                {
                    return (true, userData.role);
                }
            }

            return (false, string.Empty);
        }

        /// <summary>
        /// Gera um token JWT válido
        /// </summary>
        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(
                jwtSettings["SecretKey"] ?? "ChaveSecretaParaJWT_MinimoDe32Caracteres!@#$%"
            );

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? "MotoTrackingAPI",
                audience: jwtSettings["Audience"] ?? "MotoTrackingClient",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        /// <summary>
        /// Valida o token JWT atual (endpoint de teste)
        /// </summary>
        /// <returns>Informações do token decodificado</returns>
        [HttpGet("validate")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ValidateToken()
        {
            var username = User.Identity?.Name;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            return Ok(new
            {
                message = "Token válido",
                username,
                role,
                tokenId = jti,
                isAuthenticated = User.Identity?.IsAuthenticated ?? false
            });
        }
    }

    /// <summary>
    /// Modelo de requisição para login
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Nome de usuário
        /// </summary>
        /// <example>admin</example>
        [Required(ErrorMessage = "Username é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username deve ter entre 3 e 50 caracteres")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário
        /// </summary>
        /// <example>admin123</example>
        [Required(ErrorMessage = "Password é obrigatório")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password deve ter no mínimo 6 caracteres")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modelo de resposta do login
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Token JWT gerado
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Nome do usuário autenticado
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Papel/perfil do usuário
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Tempo de expiração do token em segundos
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Tipo do token (sempre "Bearer")
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Mensagem de sucesso
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}