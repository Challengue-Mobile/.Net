using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_.Net.Models;
using API_.Net.Data;

namespace API_.Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotosController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/Motos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
        {
            return Ok(await _context.Motos.ToListAsync());
        }

        // GET: api/Motos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);

            if (moto == null)
            {
                return NotFound();
            }

            return moto;
        }

        // GET: api/Motos/cliente/5
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotosByCliente(int clienteId)
        {
            return await _context.Motos
                .Where(m => m.ID_CLIENTE == clienteId)
                .ToListAsync();
        }

        // GET: api/Motos/placa/ABC1234
        [HttpGet("placa/{placa}")]
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

        // POST: api/Motos
        [HttpPost]
        public async Task<ActionResult<Moto>> PostMoto(Moto moto)
        {
            moto.DATA_REGISTRO = DateTime.Now;
            
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMoto), new { id = moto.ID_MOTO }, moto);
        }

        // PUT: api/Motos/5
        [HttpPut("{id}")]
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

        // DELETE: api/Motos/5
        [HttpDelete("{id}")]
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