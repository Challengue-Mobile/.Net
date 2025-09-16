using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

using API_.Net.Data;
using API_.Net.Models;
using AutoMapper;
using API_.Net.DTOs;               
using API_.Net.DTOs.Requests;
using API_.Net.DTOs.Common;  // ← ADICIONAR ESTA LINHA

namespace API_.Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui motos")]
    public class MotosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MotosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        // ================================
        // SUBSTITUIR ESTE MÉTODO:
        // ================================
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<MotoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Lista todas as motos com paginação",
                          Description = "Obtém uma lista paginada de todas as motos cadastradas no sistema")]
        public async Task<ActionResult<PagedResult<MotoDto>>> GetMotos(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            // Validação de parâmetros
            if (page < 1) 
                return BadRequest(new { message = "Página deve ser maior que 0", field = "page" });
            
            if (pageSize < 1 || pageSize > 100) 
                return BadRequest(new { message = "PageSize deve estar entre 1 e 100", field = "pageSize" });

            // Contar total de registros
            var totalCount = await _context.Motos.CountAsync();
            
            // Buscar dados paginados
            var entities = await _context.Motos
                .AsNoTracking()
                .OrderBy(m => m.ID_MOTO)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<MotoDto>>(entities);
            
            // Criar resultado paginado
            var result = new PagedResult<MotoDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalCount
            };
            
            // Adicionar links HATEOAS para navegação
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/motos";
            
            result.Links.Add(new Link 
            { 
                Href = $"{baseUrl}?page={page}&pageSize={pageSize}", 
                Rel = "self", 
                Method = "GET" 
            });
            
            if (result.HasPreviousPage)
            {
                result.Links.Add(new Link 
                { 
                    Href = $"{baseUrl}?page={page - 1}&pageSize={pageSize}", 
                    Rel = "prev", 
                    Method = "GET" 
                });
            }
            
            if (result.HasNextPage)
            {
                result.Links.Add(new Link 
                { 
                    Href = $"{baseUrl}?page={page + 1}&pageSize={pageSize}", 
                    Rel = "next", 
                    Method = "GET" 
                });
            }

            return Ok(result);
        }

        // ================================
        // SUBSTITUIR ESTE MÉTODO:
        // ================================
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MotoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém uma moto pelo ID",
                          Description = "Busca e retorna informações detalhadas de uma moto específica com links HATEOAS")]
        public async Task<ActionResult<MotoDto>> GetMoto(int id)
        {
            var entity = await _context.Motos
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.ID_MOTO == id);

            if (entity is null) 
                return NotFound(new { 
                    message = $"Moto com ID {id} não encontrada",
                    id = id,
                    timestamp = DateTime.UtcNow
                });
            
            var dto = _mapper.Map<MotoDto>(entity);
            
            // Adicionar links HATEOAS
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/motos";
            
            dto.Links.Add(new Link { Href = $"{baseUrl}/{id}", Rel = "self", Method = "GET" });
            dto.Links.Add(new Link { Href = $"{baseUrl}/{id}", Rel = "edit", Method = "PUT" });
            dto.Links.Add(new Link { Href = $"{baseUrl}/{id}", Rel = "delete", Method = "DELETE" });
            dto.Links.Add(new Link { Href = baseUrl, Rel = "all", Method = "GET" });
            
            return Ok(dto);
        }

        // ================================
        // SUBSTITUIR ESTE MÉTODO:
        // ================================
        [HttpGet("cliente/{clienteId:int}")]
        [ProducesResponseType(typeof(PagedResult<MotoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca motos por cliente com paginação")]
        public async Task<ActionResult<PagedResult<MotoDto>>> GetMotosByCliente(
            int clienteId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) return BadRequest("Página deve ser maior que 0");
            if (pageSize < 1 || pageSize > 100) return BadRequest("PageSize deve estar entre 1 e 100");
            
            var totalCount = await _context.Motos.Where(m => m.ID_CLIENTE == clienteId).CountAsync();
            
            var entities = await _context.Motos
                .AsNoTracking()
                .Where(m => m.ID_CLIENTE == clienteId)
                .OrderBy(m => m.ID_MOTO)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<MotoDto>>(entities);
            
            return Ok(new PagedResult<MotoDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalCount
            });
        }

        // ================================
        // SUBSTITUIR ESTE MÉTODO:
        // ================================
        [HttpGet("placa/{placa}")]
        [ProducesResponseType(typeof(MotoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Busca moto pela placa",
                          Description = "Localiza uma moto usando sua placa como critério de busca")]
        public async Task<ActionResult<MotoDto>> GetMotoByPlaca(string placa)
        {
            var entity = await _context.Motos
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.PLACA == placa);

            if (entity is null) 
                return NotFound(new { 
                    message = $"Moto com placa '{placa}' não encontrada",
                    placa = placa,
                    timestamp = DateTime.UtcNow
                });
            
            var dto = _mapper.Map<MotoDto>(entity);
            
            // Adicionar links HATEOAS
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/motos";
            dto.Links.Add(new Link { Href = $"{baseUrl}/{entity.ID_MOTO}", Rel = "self", Method = "GET" });
            dto.Links.Add(new Link { Href = $"{baseUrl}/{entity.ID_MOTO}", Rel = "edit", Method = "PUT" });
            dto.Links.Add(new Link { Href = $"{baseUrl}/{entity.ID_MOTO}", Rel = "delete", Method = "DELETE" });
            dto.Links.Add(new Link { Href = baseUrl, Rel = "all", Method = "GET" });
            
            return Ok(dto);
        }

        // Os métodos POST, PUT e DELETE ficam iguais por enquanto
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra uma nova moto",
                          Description = "Cria um novo registro de moto no sistema")]
        public async Task<ActionResult<MotoDto>> PostMoto([FromBody] CreateMotoDto dto)
        {
            var entity = _mapper.Map<Moto>(dto);
            entity.DATA_REGISTRO = DateTime.Now;

            _context.Motos.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<MotoDto>(entity);
            return CreatedAtAction(nameof(GetMoto), new { id = entity.ID_MOTO }, result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza uma moto",
                          Description = "Atualiza informações de uma moto existente no sistema")]
        public async Task<ActionResult<MotoDto>> PutMoto(int id, [FromBody] UpdateMotoDto dto)
        {
            var entity = await _context.Motos.FirstOrDefaultAsync(m => m.ID_MOTO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<MotoDto>(entity));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui uma moto",
                          Description = "Remove permanentemente uma moto do sistema")]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var entity = await _context.Motos.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Motos.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MotoExists(int id) =>
            _context.Motos.Any(e => e.ID_MOTO == id);
    }
}