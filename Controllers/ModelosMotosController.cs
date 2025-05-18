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
    public class ModelosMotosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModelosMotosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ModelosMotos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeloMoto>>> GetModelosMotos()
        {
            return Ok(await _context.ModelosMotos.ToListAsync());
        }

        // GET: api/ModelosMotos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModeloMoto>> GetModeloMoto(int id)
        {
            var modeloMoto = await _context.ModelosMotos.FindAsync(id);

            if (modeloMoto == null)
            {
                return NotFound();
            }

            return modeloMoto;
        }

        // POST: api/ModelosMotos
        [HttpPost]
        public async Task<ActionResult<ModeloMoto>> PostModeloMoto(ModeloMoto modeloMoto)
        {
            _context.ModelosMotos.Add(modeloMoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModeloMoto", new { id = modeloMoto.ID_MODELO_MOTO }, modeloMoto);
        }

        // PUT: api/ModelosMotos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModeloMoto(int id, ModeloMoto modeloMoto)
        {
            if (id != modeloMoto.ID_MODELO_MOTO)
            {
                return BadRequest();
            }

            _context.Entry(modeloMoto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeloMotoExists(id))
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

        // DELETE: api/ModelosMotos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModeloMoto(int id)
        {
            var modeloMoto = await _context.ModelosMotos.FindAsync(id);
            if (modeloMoto == null)
            {
                return NotFound();
            }

            _context.ModelosMotos.Remove(modeloMoto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModeloMotoExists(int id)
        {
            return _context.ModelosMotos.Any(e => e.ID_MODELO_MOTO == id);
        }
    }
}