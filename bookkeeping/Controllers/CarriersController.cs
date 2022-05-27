using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bookkeeping;
using bookkeeping.Models;

namespace bookkeeping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarriersController : ControllerBase
    {
        private readonly BookKeepingDbContext _context;

        public CarriersController(BookKeepingDbContext context)
        {
            _context = context;
        }

        // GET: api/Carriers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carrier>>> GetCarrier()
        {
            return await _context.Carrier.ToListAsync();
        }

        // GET: api/Carriers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Carrier>> GetCarrier(string id)
        {
            var carrier = await _context.Carrier.FindAsync(id);

            if (carrier == null)
            {
                return NotFound();
            }

            return carrier;
        }
        
        // 소유주 확인
        [HttpGet("{carrierName}/{phoneNum}/{socialNum}")]
        public async Task<ActionResult<Carrier>> GetCarrier(string carrierName, string phoneNum, string socialNum)
        {
            var carrier = await _context.Carrier
                .Where(c => c.CarrierName == carrierName && c.PhoneNo == phoneNum && c.SocialNum == socialNum)
                .FirstOrDefaultAsync();

            if (carrier == null)
            {
                return NotFound();
            }

            return carrier;
        }

        // PUT: api/Carriers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarrier(string id, Carrier carrier)
        {
            if (id != carrier.PhoneNo)
            {
                return BadRequest();
            }

            _context.Entry(carrier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarrierExists(id))
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

        // POST: api/Carriers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Carrier>> PostCarrier(Carrier carrier)
        {
            _context.Carrier.Add(carrier);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CarrierExists(carrier.PhoneNo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCarrier", new { id = carrier.PhoneNo }, carrier);
        }

        // DELETE: api/Carriers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrier(string id)
        {
            var carrier = await _context.Carrier.FindAsync(id);
            if (carrier == null)
            {
                return NotFound();
            }

            _context.Carrier.Remove(carrier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarrierExists(string id)
        {
            return _context.Carrier.Any(e => e.PhoneNo == id);
        }
    }
}
