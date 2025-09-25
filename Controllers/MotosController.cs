using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottothTracking.Data;
using MottothTracking.Models;

namespace MottothTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Motos
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
        {
            return await _context.Motos.Include(m => m.Beacon).ToListAsync();
        }

        // GET: api/Motos/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Moto>> GetMoto(int id)
        {
            var moto = await _context.Motos
                .Include(m => m.Beacon)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (moto == null)
            {
                return NotFound();
            }

            return moto;
        }

        // GET: api/Motos/ByPlaca/{placa}
        [HttpGet("ByPlaca/{placa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Moto>> GetMotoByPlaca(string placa)
        {
            var moto = await _context.Motos
                .Include(m => m.Beacon)
                .FirstOrDefaultAsync(m => m.Placa == placa);

            if (moto == null)
            {
                return NotFound();
            }

            return moto;
        }

        // GET: api/Motos/ByStatus/{status}
        [HttpGet("ByStatus/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotosByStatus(string status)
        {
            return await _context.Motos
                .Include(m => m.Beacon)
                .Where(m => m.Status == status)
                .ToListAsync();
        }

        // POST: api/Motos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Moto>> PostMoto(Moto moto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (moto.DataRegistro == default)
            {
                moto.DataRegistro = DateTime.Now;
            }

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto);
        }

        // PUT: api/Motos/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMoto(int id, Moto moto)
        {
            if (id != moto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

        // DELETE: api/Motos/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            return _context.Motos.Any(e => e.Id == id);
        }
    }
}
