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
    public class FiliaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FiliaisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Filiais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filial>>> GetFiliais()
        {
            return Ok(await _context.Filiais.ToListAsync());
        }

        // GET: api/Filiais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Filial>> GetFilial(int id)
        {
            var filial = await _context.Filiais.FindAsync(id);

            if (filial == null)
            {
                return NotFound();
            }

            return filial;
        }

        // GET: api/Filiais/patio/5
        [HttpGet("patio/{patioId}")]
        public async Task<ActionResult<Filial>> GetFilialByPatio(int patioId)
        {
            var filial = await _context.Filiais
                .FirstOrDefaultAsync(f => f.ID_PATIO == patioId);

            if (filial == null)
            {
                return NotFound();
            }

            return filial;
        }

        // POST: api/Filiais
        [HttpPost]
        public async Task<ActionResult<Filial>> PostFilial(Filial filial)
        {
            _context.Filiais.Add(filial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFilial", new { id = filial.ID_FILIAL }, filial);
        }

        // PUT: api/Filiais/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilial(int id, Filial filial)
        {
            if (id != filial.ID_FILIAL)
            {
                return BadRequest();
            }

            _context.Entry(filial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilialExists(id))
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

        // DELETE: api/Filiais/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilial(int id)
        {
            var filial = await _context.Filiais.FindAsync(id);
            if (filial == null)
            {
                return NotFound();
            }

            _context.Filiais.Remove(filial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FilialExists(int id)
        {
            return _context.Filiais.Any(e => e.ID_FILIAL == id);
        }
    }
}