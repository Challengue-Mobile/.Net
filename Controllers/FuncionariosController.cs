using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API_.Net.Data;
using API_.Net.Models;
using AutoMapper;
using API_.Net.DTOs;               // FuncionarioDTO
using API_.Net.DTOs.Requests;      // CreateFuncionarioDTO / UpdateFuncionarioDTO

namespace API.Net.Controllers
{
    /// <summary>API para gerenciamento de funcionários</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui funcionários")]
    public class FuncionariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FuncionariosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>Lista todos os funcionários</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todos os funcionários", Description = "Obtém todos os funcionários cadastrados")]
        public async Task<ActionResult<IEnumerable<FuncionarioDto>>> GetFuncionarios()
        {
            var entities = await _context.Funcionarios
                .AsNoTracking()
                .Include(f => f.Departamento)
                .Include(f => f.Usuario)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<FuncionarioDto>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém um funcionário pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém um funcionário por ID", Description = "Retorna informações detalhadas de um funcionário")]
        public async Task<ActionResult<FuncionarioDto>> GetFuncionario(int id)
        {
            var entity = await _context.Funcionarios
                .AsNoTracking()
                .Include(f => f.Departamento)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(f => f.ID_FUNCIONARIO == id);

            if (entity is null) return NotFound();

            return Ok(_mapper.Map<FuncionarioDto>(entity));
        }

        /// <summary>Cadastra um novo funcionário</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra um novo funcionário", Description = "Cria um novo registro de funcionário")]
        public async Task<ActionResult<FuncionarioDto>> PostFuncionario([FromBody] CreateFuncionarioDto dto)
        {
            var entity = _mapper.Map<Funcionario>(dto);

            _context.Funcionarios.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<FuncionarioDto>(entity);
            return CreatedAtAction(nameof(GetFuncionario), new { id = entity.ID_FUNCIONARIO }, result);
        }

        /// <summary>Atualiza um funcionário existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza um funcionário", Description = "Atualiza informações de um funcionário existente")]
        public async Task<ActionResult<FuncionarioDto>> PutFuncionario(int id, [FromBody] UpdateFuncionarioDto dto)
        {
            var entity = await _context.Funcionarios.FirstOrDefaultAsync(f => f.ID_FUNCIONARIO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity); // aplica somente campos enviados (se usar IgnoreNulls no Profile)
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<FuncionarioDto>(entity));
        }

        /// <summary>Exclui um funcionário</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui um funcionário", Description = "Remove um funcionário do sistema")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            var entity = await _context.Funcionarios.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Funcionarios.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
