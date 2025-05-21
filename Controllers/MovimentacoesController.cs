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
    /// API para gerenciamento de movimentações de motos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui movimentações de motos")]
    public class MovimentacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovimentacoesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as movimentações registradas
        /// </summary>
        /// <returns>Lista de movimentações</returns>
        /// <response code="200">Retorna a lista de movimentações</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todas as movimentações",
            Description = "Obtém uma lista de todas as movimentações de motos registradas no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MovimentacoesListResponseExample))]
        public async Task<ActionResult<IEnumerable<Movimentacao>>> GetMovimentacoes()
        {
            return Ok(await _context.Movimentacoes.ToListAsync());
        }

        /// <summary>
        /// Obtém uma movimentação específica pelo ID
        /// </summary>
        /// <param name="id">ID da movimentação</param>
        /// <returns>Dados da movimentação solicitada</returns>
        /// <response code="200">Retorna a movimentação</response>
        /// <response code="404">Se a movimentação não for encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém uma movimentação pelo ID",
            Description = "Busca e retorna informações detalhadas de uma movimentação específica")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MovimentacaoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Movimentacao>> GetMovimentacao(int id)
        {
            var movimentacao = await _context.Movimentacoes.FindAsync(id);

            if (movimentacao == null)
            {
                return NotFound();
            }

            return movimentacao;
        }

        /// <summary>
        /// Busca movimentações de uma moto específica
        /// </summary>
        /// <param name="motoId">ID da moto</param>
        /// <returns>Lista de movimentações da moto, ordenadas pela data mais recente</returns>
        /// <response code="200">Retorna a lista de movimentações da moto</response>
        [HttpGet("moto/{motoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Busca movimentações por moto",
            Description = "Obtém o histórico de movimentações de uma moto específica, ordenadas da mais recente para a mais antiga")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MovimentacoesListResponseExample))]
        public async Task<ActionResult<IEnumerable<Movimentacao>>> GetMovimentacoesByMoto(int motoId)
        {
            return await _context.Movimentacoes
                .Where(m => m.ID_MOTO == motoId)
                .OrderByDescending(m => m.DATA_MOVIMENTACAO)
                .ToListAsync();
        }

        /// <summary>
        /// Registra uma nova movimentação
        /// </summary>
        /// <param name="movimentacao">Dados da movimentação a ser registrada</param>
        /// <returns>Movimentação registrada com seu ID</returns>
        /// <response code="201">Retorna a movimentação recém registrada</response>
        /// <response code="400">Se os dados da movimentação são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/Movimentacoes
        ///     {
        ///        "observacao": "Saída para entrega",
        ///        "id_usuario": 1,
        ///        "id_moto": 1,
        ///        "id_tipo_movimentacao": 2
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Registra uma nova movimentação",
            Description = "Cria um novo registro de movimentação de moto no sistema")]
        [SwaggerRequestExample(typeof(Movimentacao), typeof(MovimentacaoRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(MovimentacaoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<Movimentacao>> PostMovimentacao(Movimentacao movimentacao)
        {
            movimentacao.DATA_MOVIMENTACAO = DateTime.Now;
            
            _context.Movimentacoes.Add(movimentacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovimentacao", new { id = movimentacao.ID_MOVIMENTACAO }, movimentacao);
        }

        /// <summary>
        /// Atualiza os dados de uma movimentação existente
        /// </summary>
        /// <param name="id">ID da movimentação a ser atualizada</param>
        /// <param name="movimentacao">Novos dados da movimentação</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se a movimentação foi atualizada com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se a movimentação não for encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza uma movimentação",
            Description = "Atualiza informações de uma movimentação existente no sistema")]
        [SwaggerRequestExample(typeof(Movimentacao), typeof(MovimentacaoRequestExample))]
        public async Task<IActionResult> PutMovimentacao(int id, Movimentacao movimentacao)
        {
            if (id != movimentacao.ID_MOVIMENTACAO)
            {
                return BadRequest();
            }

            _context.Entry(movimentacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimentacaoExists(id))
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
        /// Remove uma movimentação do sistema
        /// </summary>
        /// <param name="id">ID da movimentação a ser removida</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se a movimentação foi removida com sucesso</response>
        /// <response code="404">Se a movimentação não for encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui uma movimentação",
            Description = "Remove permanentemente uma movimentação do sistema")]
        public async Task<IActionResult> DeleteMovimentacao(int id)
        {
            var movimentacao = await _context.Movimentacoes.FindAsync(id);
            if (movimentacao == null)
            {
                return NotFound();
            }

            _context.Movimentacoes.Remove(movimentacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovimentacaoExists(int id)
        {
            return _context.Movimentacoes.Any(e => e.ID_MOVIMENTACAO == id);
        }
    }
}