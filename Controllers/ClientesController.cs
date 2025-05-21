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
    /// API para gerenciamento de clientes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os clientes cadastrados
        /// </summary>
        /// <returns>Lista de clientes</returns>
        /// <response code="200">Retorna a lista de clientes</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os clientes",
            Description = "Obtém uma lista de todos os clientes cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ClientesListResponseExample))]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return Ok(await _context.Clientes.ToListAsync());
        }

        /// <summary>
        /// Obtém um cliente específico pelo ID
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Dados do cliente solicitado</returns>
        /// <response code="200">Retorna o cliente</response>
        /// <response code="404">Se o cliente não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um cliente pelo ID",
            Description = "Busca e retorna informações detalhadas de um cliente específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ClienteResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        /// <summary>
        /// Busca um cliente pelo CPF
        /// </summary>
        /// <param name="cpf">CPF do cliente (formato XXX.XXX.XXX-XX)</param>
        /// <returns>Cliente correspondente ao CPF</returns>
        /// <response code="200">Retorna o cliente com o CPF especificado</response>
        /// <response code="404">Se nenhum cliente com o CPF informado for encontrado</response>
        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Busca cliente pelo CPF",
            Description = "Localiza um cliente usando seu CPF como critério de busca")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ClienteResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Cliente>> GetClienteByCpf(string cpf)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.CPF == cpf);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        /// <summary>
        /// Cadastra um novo cliente
        /// </summary>
        /// <param name="cliente">Dados do cliente a ser cadastrado</param>
        /// <returns>Cliente cadastrado com seu ID</returns>
        /// <response code="201">Retorna o cliente recém criado</response>
        /// <response code="400">Se os dados do cliente são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/Clientes
        ///     {
        ///        "nome": "João da Silva",
        ///        "cpf": "123.456.789-00",
        ///        "email": "joao@exemplo.com",
        ///        "telefone": "(11) 99999-8888"
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo cliente",
            Description = "Cria um novo registro de cliente no sistema")]
        [SwaggerRequestExample(typeof(Cliente), typeof(ClienteRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ClienteResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            cliente.DATA_CADASTRO = DateTime.Now;
            
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.ID_CLIENTE }, cliente);
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente
        /// </summary>
        /// <param name="id">ID do cliente a ser atualizado</param>
        /// <param name="cliente">Novos dados do cliente</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o cliente foi atualizado com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se o cliente não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um cliente",
            Description = "Atualiza informações de um cliente existente no sistema")]
        [SwaggerRequestExample(typeof(Cliente), typeof(ClienteRequestExample))]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.ID_CLIENTE)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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
        /// Remove um cliente do sistema
        /// </summary>
        /// <param name="id">ID do cliente a ser removido</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o cliente foi removido com sucesso</response>
        /// <response code="404">Se o cliente não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um cliente",
            Description = "Remove permanentemente um cliente do sistema")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ID_CLIENTE == id);
        }
    }
}