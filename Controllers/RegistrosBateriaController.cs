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
    public class RegistrosBateriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegistrosBateriaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RegistrosBateria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroBateria>>> GetRegistrosBateria()
        {
            return Ok(await _context.RegistrosBateria.ToListAsync());
        }

        // GET: api/RegistrosBateria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroBateria>> GetRegistroBateria(int id)
        {
            var registroBateria = await _context.RegistrosBateria.FindAsync(id);

            if (registroBateria == null)
            {
                return NotFound();
            }

            return registroBateria;
        }

        // GET: api/RegistrosBateria/beacon/5
        [HttpGet("beacon/{beaconId}")]
        public async Task<ActionResult<IEnumerable<RegistroBateria>>> GetRegistrosByBeacon(int beaconId)
        {
            return await _context.RegistrosBateria
                .Where(rb => rb.ID_BEACON == beaconId)
                .OrderByDescending(rb => rb.DATA_HORA)
                .ToListAsync();
        }

        // POST: api/RegistrosBateria
        [HttpPost]
        public async Task<ActionResult<RegistroBateria>> PostRegistroBateria(RegistroBateria registroBateria)
        {
            registroBateria.DATA_HORA = DateTime.Now;
            
            _context.RegistrosBateria.Add(registroBateria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroBateria", new { id = registroBateria.ID_REGISTRO }, registroBateria);
        }

        // PUT: api/RegistrosBateria/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroBateria(int id, RegistroBateria registroBateria)
        {
            if (id != registroBateria.ID_REGISTRO)
            {
                return BadRequest();
            }

            _context.Entry(registroBateria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroBateriaExists(id))
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

        // DELETE: api/RegistrosBateria/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroBateria(int id)
        {
            var registroBateria = await _context.RegistrosBateria.FindAsync(id);
            if (registroBateria == null)
            {
                return NotFound();
            }

            _context.RegistrosBateria.Remove(registroBateria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroBateriaExists(int id)
        {
            return _context.RegistrosBateria.Any(e => e.ID_REGISTRO == id);
        }
    }
}