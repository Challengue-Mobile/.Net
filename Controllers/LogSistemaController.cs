using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottothTracking.Data;
using MottothTracking.Models;

namespace MottothTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogSistemaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LogSistemaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LogSistema
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LogSistema>>> GetLogsSistema()
        {
            return await _context.LogsSistema
                .Include(l => l.Usuario)
                .OrderByDescending(l => l.DataHora)
                .ToListAsync();
        }

        // GET: api/LogSistema/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LogSistema>> GetLogSistema(int id)
        {
            var logSistema = await _context.LogsSistema
                .Include(l => l.Usuario)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (logSistema == null)
            {
                return NotFound();
            }

            return logSistema;
        }

        // GET: api/LogSistema/ByUsuario/{usuarioId}
        [HttpGet("ByUsuario/{usuarioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LogSistema>>> GetLogsSistemaByUsuario(int usuarioId)
        {
            return await _context.LogsSistema
                .Where(l => l.UsuarioId == usuarioId)
                .OrderByDescending(l => l.DataHora)
                .ToListAsync();
        }

        // POST: api/LogSistema
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LogSistema>> PostLogSistema(LogSistema logSistema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (logSistema.DataHora == default)
            {
                logSistema.DataHora = DateTime.Now;
            }

            _context.LogsSistema.Add(logSistema);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLogSistema), new { id = logSistema.Id }, logSistema);
        }

        // DELETE: api/LogSistema/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLogSistema(int id)
        {
            var logSistema = await _context.LogsSistema.FindAsync(id);
            if (logSistema == null)
            {
                return NotFound();
            }

            _context.LogsSistema.Remove(logSistema);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
