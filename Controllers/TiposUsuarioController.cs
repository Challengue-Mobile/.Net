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
    public class TiposUsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposUsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TiposUsuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoUsuario>>> GetTiposUsuario()
        {
            return Ok(await _context.TiposUsuario.ToListAsync());
        }

        // GET: api/TiposUsuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoUsuario>> GetTipoUsuario(int id)
        {
            var tipoUsuario = await _context.TiposUsuario.FindAsync(id);

            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return tipoUsuario;
        }

        // POST: api/TiposUsuario
        [HttpPost]
        public async Task<ActionResult<TipoUsuario>> PostTipoUsuario(TipoUsuario tipoUsuario)
        {
            _context.TiposUsuario.Add(tipoUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoUsuario", new { id = tipoUsuario.ID_TIPO_USUARIO }, tipoUsuario);
        }

        // PUT: api/TiposUsuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoUsuario(int id, TipoUsuario tipoUsuario)
        {
            if (id != tipoUsuario.ID_TIPO_USUARIO)
            {
                return BadRequest();
            }

            _context.Entry(tipoUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoUsuarioExists(id))
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

        // DELETE: api/TiposUsuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoUsuario(int id)
        {
            var tipoUsuario = await _context.TiposUsuario.FindAsync(id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }

            _context.TiposUsuario.Remove(tipoUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoUsuarioExists(int id)
        {
            return _context.TiposUsuario.Any(e => e.ID_TIPO_USUARIO == id);
        }
    }
}