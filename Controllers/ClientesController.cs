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
using AutoMapper;
using API_.Net.DTOs;                  // ClienteDTO
using API_.Net.DTOs.Requests;
using Asp.Versioning; // CreateClienteDTO / UpdateClienteDTO

namespace API.Net.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")] 
    [ApiVersion("1.0")] 
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ClientesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todos os clientes",
                          Description = "Obtém uma lista de todos os clientes cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ClientesListResponseExample))]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            var entities = await _context.Clientes.AsNoTracking().ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ClienteDto>>(entities));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém um cliente pelo ID",
                          Description = "Busca e retorna informações detalhadas de um cliente específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ClienteResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            var entity = await _context.Clientes.AsNoTracking()
                                .FirstOrDefaultAsync(c => c.ID_CLIENTE == id);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<ClienteDto>(entity));
        }

        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Busca cliente pelo CPF",
                          Description = "Localiza um cliente usando seu CPF como critério de busca")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ClienteResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<ClienteDto>> GetClienteByCpf(string cpf)
        {
            var entity = await _context.Clientes.AsNoTracking()
                                .FirstOrDefaultAsync(c => c.CPF == cpf);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<ClienteDto>(entity));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra um novo cliente",
                          Description = "Cria um novo registro de cliente no sistema")]
        [SwaggerRequestExample(typeof(CreateClienteDto), typeof(ClienteRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ClienteResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<ClienteDto>> PostCliente([FromBody] CreateClienteDto dto)
        {
            var entity = _mapper.Map<Cliente>(dto);
            entity.DATA_CADASTRO = DateTime.Now;

            _context.Clientes.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ClienteDto>(entity);
            return CreatedAtAction(nameof(GetCliente), new { id = entity.ID_CLIENTE }, result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza um cliente",
                          Description = "Atualiza informações de um cliente existente no sistema")]
        [SwaggerRequestExample(typeof(UpdateClienteDto), typeof(ClienteRequestExample))]
        public async Task<ActionResult<ClienteDto>> PutCliente(int id, [FromBody] UpdateClienteDto dto)
        {
            var entity = await _context.Clientes.FirstOrDefaultAsync(c => c.ID_CLIENTE == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity); // aplica apenas campos enviados (mapeamento ignora null)
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ClienteDto>(entity));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui um cliente",
                          Description = "Remove permanentemente um cliente do sistema")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var entity = await _context.Clientes.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Clientes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id) => _context.Clientes.Any(e => e.ID_CLIENTE == id);
    }
}
