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
    public class BeaconsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BeaconsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Beacons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beacon>>> GetBeacons()
        {
            return Ok(await _context.Beacons.ToListAsync());
        }

        // GET: api/Beacons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Beacon>> GetBeacon(int id)
        {
            var beacon = await _context.Beacons.FindAsync(id);

            if (beacon == null)
            {
                return NotFound();
            }

            return beacon;
        }

        // GET: api/Beacons/moto/5
        [HttpGet("moto/{motoId}")]
        public async Task<ActionResult<IEnumerable<Beacon>>> GetBeaconsByMoto(int motoId)
        {
            return await _context.Beacons
                .Where(b => b.ID_MOTO == motoId)
                .ToListAsync();
        }

        // GET: api/Beacons/uuid/{uuid}
        [HttpGet("uuid/{uuid}")]
        public async Task<ActionResult<Beacon>> GetBeaconByUuid(string uuid)
        {
            var beacon = await _context.Beacons
                .FirstOrDefaultAsync(b => b.UUID == uuid);

            if (beacon == null)
            {
                return NotFound();
            }

            return beacon;
        }

        // POST: api/Beacons
        [HttpPost]
        public async Task<ActionResult<Beacon>> PostBeacon(Beacon beacon)
        {
            _context.Beacons.Add(beacon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBeacon", new { id = beacon.ID_BEACON }, beacon);
        }

        // PUT: api/Beacons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeacon(int id, Beacon beacon)
        {
            if (id != beacon.ID_BEACON)
            {
                return BadRequest();
            }

            _context.Entry(beacon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeaconExists(id))
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

        // DELETE: api/Beacons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeacon(int id)
        {
            var beacon = await _context.Beacons.FindAsync(id);
            if (beacon == null)
            {
                return NotFound();
            }

            _context.Beacons.Remove(beacon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BeaconExists(int id)
        {
            return _context.Beacons.Any(e => e.ID_BEACON == id);
        }
    }
}