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

namespace API_.Net.Controllers
{
    /// <summary>
    /// API para gerenciamento de motos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui motos")]
    public class MotosController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        /// <summary>
        /// Obtém todas as motos cadastradas
        /// </summary>
        /// <returns>Lista de motos</returns>
        /// <response code="200">Retorna a lista de motos</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todas as motos",
            Description = "Obtém uma lista de todas as motos cadastradas no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MotosListResponseExample))]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
        {
            return Ok(await _context.Motos.ToListAsync());
        }

        /// <summary>
        /// Obtém uma moto específica pelo ID
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <returns>Dados da moto solicitada</returns>
        /// <response code="200">Retorna a moto</response>
        /// <response code="404">Se a moto não for encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém uma moto pelo ID",
            Description = "Busca e retorna informações detalhadas de uma moto específica")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MotoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Moto>> GetMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);

            if (moto == null)
            {
                return NotFound();
            }

            return moto;
        }

        /// <summary>
        /// Busca motos associadas a um cliente específico
        /// </summary>
        /// <param name="clienteId">ID do cliente</param>
        /// <returns>Lista de motos do cliente</returns>
        /// <response code="200">Retorna a lista de motos do cliente</response>
        [HttpGet("cliente/{clienteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Busca motos por cliente",
            Description = "Obtém todas as motos associadas a um cliente específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MotosListResponseExample))]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotosByCliente(int clienteId)
        {
            return await _context.Motos
                .Where(m => m.ID_CLIENTE == clienteId)
                .ToListAsync();
        }

        /// <summary>
        /// Busca uma moto pela placa
        /// </summary>
        /// <param name="placa">Placa da moto (formato XXX0000)</param>
        /// <returns>Moto correspondente à placa</returns>
        /// <response code="200">Retorna a moto com a placa especificada</response>
        /// <response code="404">Se nenhuma moto com a placa informada for encontrada</response>
        [HttpGet("placa/{placa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Busca moto pela placa",
            Description = "Localiza uma moto usando sua placa como critério de busca")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MotoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Moto>> GetMotoByPlaca(string placa)
        {
            var moto = await _context.Motos
                .FirstOrDefaultAsync(m => m.PLACA == placa);

            if (moto == null)
            {
                return NotFound();
            }

            return moto;
        }

        /// <summary>
        /// Cadastra uma nova moto
        /// </summary>
        /// <param name="moto">Dados da moto a ser cadastrada</param>
        /// <returns>Moto cadastrada com seu ID</returns>
        /// <response code="201">Retorna a moto recém criada</response>
        /// <response code="400">Se os dados da moto são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/Motos
        ///     {
        ///        "placa": "ABC1234",
        ///        "id_cliente": 1,
        ///        "id_modelo_moto": 2
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra uma nova moto",
            Description = "Cria um novo registro de moto no sistema")]
        [SwaggerRequestExample(typeof(Moto), typeof(MotoRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(MotoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<Moto>> PostMoto(Moto moto)
        {
            moto.DATA_REGISTRO = DateTime.Now;
            
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMoto), new { id = moto.ID_MOTO }, moto);
        }

        /// <summary>
        /// Atualiza os dados de uma moto existente
        /// </summary>
        /// <param name="id">ID da moto a ser atualizada</param>
        /// <param name="moto">Novos dados da moto</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se a moto foi atualizada com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se a moto não for encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza uma moto",
            Description = "Atualiza informações de uma moto existente no sistema")]
        [SwaggerRequestExample(typeof(Moto), typeof(MotoRequestExample))]
        public async Task<IActionResult> PutMoto(int id, Moto moto)
        {
            if (id != moto.ID_MOTO)
            {
                return BadRequest();
            }

            _context.Entry(moto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotoExists(id))
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
        /// Remove uma moto do sistema
        /// </summary>
        /// <param name="id">ID da moto a ser removida</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se a moto foi removida com sucesso</response>
        /// <response code="404">Se a moto não for encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui uma moto",
            Description = "Remove permanentemente uma moto do sistema")]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
            {
                return NotFound();
            }

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MotoExists(int id)
        {
            return _context.Motos.Any(e => e.ID_MOTO == id);
        }
    }
}