using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottothTracking.Data;
using MottothTracking.Models;

namespace MottothTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeaconsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BeaconsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Beacons
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Beacon>>> GetBeacons()
        {
            return await _context.Beacons.ToListAsync();
        }

        // GET: api/Beacons/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Beacon>> GetBeacon(int id)
        {
            var beacon = await _context.Beacons.FindAsync(id);

            if (beacon == null)
            {
                return NotFound();
            }

            return beacon;
        }

        // GET: api/Beacons/ByUuid/{uuid}
        [HttpGet("ByUuid/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Beacon>> GetBeaconByUuid(string uuid)
        {
            var beacon = await _context.Beacons.FirstOrDefaultAsync(b => b.Uuid == uuid);

            if (beacon == null)
            {
                return NotFound();
            }

            return beacon;
        }

        // GET: api/Beacons/ByStatus/{status}
        [HttpGet("ByStatus/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Beacon>>> GetBeaconsByStatus(string status)
        {
            return await _context.Beacons.Where(b => b.Status == status).ToListAsync();
        }

        // POST: api/Beacons
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Beacon>> PostBeacon(Beacon beacon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Beacons.Add(beacon);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBeacon), new { id = beacon.Id }, beacon);
        }

        // PUT: api/Beacons/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBeacon(int id, Beacon beacon)
        {
            if (id != beacon.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            return _context.Beacons.Any(e => e.Id == id);
        }
    }
}
