using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_.Net.Models;
using API_.Net.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using API_.Net.Examples;
using Microsoft.AspNetCore.Http;

namespace API.Net.Controllers
{
    /// <summary>
    /// API para gerenciamento de tipos de movimentação
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui tipos de movimentação")]
    public class TiposMovimentacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposMovimentacaoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os tipos de movimentação cadastrados
        /// </summary>
        /// <returns>Lista de tipos de movimentação</returns>
        /// <response code="200">Retorna a lista de tipos de movimentação</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os tipos de movimentação",
            Description = "Obtém uma lista de todos os tipos de movimentação cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TiposMovimentacaoListResponseExample))]
        public async Task<ActionResult<IEnumerable<TipoMovimentacao>>> GetTiposMovimentacao()
        {
            return Ok(await _context.TiposMovimentacao.ToListAsync());
        }

        /// <summary>
        /// Obtém um tipo de movimentação específico pelo ID
        /// </summary>
        /// <param name="id">ID do tipo de movimentação</param>
        /// <returns>Dados do tipo de movimentação solicitado</returns>
        /// <response code="200">Retorna o tipo de movimentação</response>
        /// <response code="404">Se o tipo de movimentação não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um tipo de movimentação pelo ID",
            Description = "Busca e retorna informações detalhadas de um tipo de movimentação específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TipoMovimentacaoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<TipoMovimentacao>> GetTipoMovimentacao(int id)
        {
            var tipoMovimentacao = await _context.TiposMovimentacao.FindAsync(id);

            if (tipoMovimentacao == null)
            {
                return NotFound();
            }

            return tipoMovimentacao;
        }

        /// <summary>
        /// Cadastra um novo tipo de movimentação
        /// </summary>
        /// <param name="tipoMovimentacao">Dados do tipo de movimentação a ser cadastrado</param>
        /// <returns>Tipo de movimentação cadastrado com seu ID</returns>
        /// <response code="201">Retorna o tipo de movimentação recém criado</response>
        /// <response code="400">Se os dados do tipo de movimentação são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/TiposMovimentacao
        ///     {
        ///        "descricao": "Entrada"
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo tipo de movimentação",
            Description = "Cria um novo registro de tipo de movimentação no sistema")]
        [SwaggerRequestExample(typeof(TipoMovimentacao), typeof(TipoMovimentacaoRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(TipoMovimentacaoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<TipoMovimentacao>> PostTipoMovimentacao(TipoMovimentacao tipoMovimentacao)
        {
            _context.TiposMovimentacao.Add(tipoMovimentacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoMovimentacao", new { id = tipoMovimentacao.ID_TIPO_MOVIMENTACAO }, tipoMovimentacao);
        }

        /// <summary>
        /// Atualiza os dados de um tipo de movimentação existente
        /// </summary>
        /// <param name="id">ID do tipo de movimentação a ser atualizado</param>
        /// <param name="tipoMovimentacao">Novos dados do tipo de movimentação</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o tipo de movimentação foi atualizado com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se o tipo de movimentação não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um tipo de movimentação",
            Description = "Atualiza informações de um tipo de movimentação existente no sistema")]
        [SwaggerRequestExample(typeof(TipoMovimentacao), typeof(TipoMovimentacaoRequestExample))]
        public async Task<IActionResult> PutTipoMovimentacao(int id, TipoMovimentacao tipoMovimentacao)
        {
            if (id != tipoMovimentacao.ID_TIPO_MOVIMENTACAO)
            {
                return BadRequest();
            }

            _context.Entry(tipoMovimentacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoMovimentacaoExists(id))
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
        /// Remove um tipo de movimentação do sistema
        /// </summary>
        /// <param name="id">ID do tipo de movimentação a ser removido</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o tipo de movimentação foi removido com sucesso</response>
        /// <response code="404">Se o tipo de movimentação não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um tipo de movimentação",
            Description = "Remove permanentemente um tipo de movimentação do sistema")]
        public async Task<IActionResult> DeleteTipoMovimentacao(int id)
        {
            var tipoMovimentacao = await _context.TiposMovimentacao.FindAsync(id);
            if (tipoMovimentacao == null)
            {
                return NotFound();
            }

            _context.TiposMovimentacao.Remove(tipoMovimentacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoMovimentacaoExists(int id)
        {
            return _context.TiposMovimentacao.Any(e => e.ID_TIPO_MOVIMENTACAO == id);
        }
    }
}