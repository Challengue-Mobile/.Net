using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_.Net.Models;
using API_.Net.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatiosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatiosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Patios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatios()
        {
            return Ok(await _context.Patios.ToListAsync());
        }

        // GET: api/Patios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patio>> GetPatio(int id)
        {
            var patio = await _context.Patios.FindAsync(id);

            if (patio == null)
            {
                return NotFound();
            }

            return patio;
        }

        // GET: api/Patios/logradouro/5
        [HttpGet("logradouro/{logradouroId}")]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatiosByLogradouro(int logradouroId)
        {
            return await _context.Patios
                .Where(p => p.ID_LOGRADOURO == logradouroId)
                .ToListAsync();
        }

        // POST: api/Patios
        [HttpPost]
        public async Task<ActionResult<Patio>> PostPatio(Patio patio)
        {
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatio", new { id = patio.ID_PATIO }, patio);
        }

        // PUT: api/Patios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatio(int id, Patio patio)
        {
            if (id != patio.ID_PATIO)
            {
                return BadRequest();
            }

            _context.Entry(patio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatioExists(id))
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

        // DELETE: api/Patios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatio(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
            {
                return NotFound();
            }

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatioExists(int id)
        {
            return _context.Patios.Any(e => e.ID_PATIO == id);
        }
    }
}