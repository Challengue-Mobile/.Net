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
    public class ModelosBeaconController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModelosBeaconController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ModelosBeacon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeloBeacon>>> GetModelosBeacon()
        {
            return Ok(await _context.ModelosBeacon.ToListAsync());
        }

        // GET: api/ModelosBeacon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModeloBeacon>> GetModeloBeacon(int id)
        {
            var modeloBeacon = await _context.ModelosBeacon.FindAsync(id);

            if (modeloBeacon == null)
            {
                return NotFound();
            }

            return modeloBeacon;
        }

        // POST: api/ModelosBeacon
        [HttpPost]
        public async Task<ActionResult<ModeloBeacon>> PostModeloBeacon(ModeloBeacon modeloBeacon)
        {
            _context.ModelosBeacon.Add(modeloBeacon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModeloBeacon", new { id = modeloBeacon.ID_MODELO_BEACON }, modeloBeacon);
        }

        // PUT: api/ModelosBeacon/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModeloBeacon(int id, ModeloBeacon modeloBeacon)
        {
            if (id != modeloBeacon.ID_MODELO_BEACON)
            {
                return BadRequest();
            }

            _context.Entry(modeloBeacon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeloBeaconExists(id))
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

        // DELETE: api/ModelosBeacon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModeloBeacon(int id)
        {
            var modeloBeacon = await _context.ModelosBeacon.FindAsync(id);
            if (modeloBeacon == null)
            {
                return NotFound();
            }

            _context.ModelosBeacon.Remove(modeloBeacon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModeloBeaconExists(int id)
        {
            return _context.ModelosBeacon.Any(e => e.ID_MODELO_BEACON == id);
        }
    }
}