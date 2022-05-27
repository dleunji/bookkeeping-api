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
    public class OrdinaryCardsController : ControllerBase
    {
        private readonly BookKeepingDbContext _context;

        public OrdinaryCardsController(BookKeepingDbContext context)
        {
            _context = context;
        }

        // GET: api/OrdinaryCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdinaryCard>>> GetOrdinaryCard()
        {
            return await _context.OrdinaryCard.ToListAsync();
        }
        
        // 카드 정보 가져오기
        [HttpGet("{cardAddress}/{validMonth}/{validYear}")]
        public async Task<ActionResult<OrdinaryCard>> GetOrdinaryCard(string cardAddress, int validMonth, int validYear)
        {
            var ordinaryCard = await _context.OrdinaryCard
                .Where(card => card.CardAddress == cardAddress && card.ValidMonth == validMonth && card.ValidYear == validYear)
                .FirstOrDefaultAsync();

            if (ordinaryCard == null)
            {
                return NotFound();
            }

            return ordinaryCard;
        }
        
        // 카드 등록하기
        [HttpPost("register")]
        public async Task<ActionResult> RegisterCard(RegisterRequest request)
        {
            var password = request.Password;
            var address = request.Address;
            var ordinaryCard = await _context.OrdinaryCard
                .Where(card => card.CardAddress == address)
                .FirstOrDefaultAsync();

            if (ordinaryCard == null)
            {
                return NotFound();
            }

            ordinaryCard.CardPassword = password;
            ordinaryCard.IsRegistered = true;
            
            try
            { 
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(ordinaryCard);
        }

        // PUT: api/OrdinaryCards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdinaryCard(int id, OrdinaryCard ordinaryCard)
        {
            if (id != ordinaryCard.CardId)
            {
                return BadRequest();
            }

            _context.Entry(ordinaryCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdinaryCardExists(id))
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

        // POST: api/OrdinaryCards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrdinaryCard>> PostOrdinaryCard(OrdinaryCard ordinaryCard)
        {
            _context.OrdinaryCard.Add(ordinaryCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrdinaryCard", new { id = ordinaryCard.CardId }, ordinaryCard);
        }

        // DELETE: api/OrdinaryCards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdinaryCard(int id)
        {
            var ordinaryCard = await _context.OrdinaryCard.FindAsync(id);
            if (ordinaryCard == null)
            {
                return NotFound();
            }

            _context.OrdinaryCard.Remove(ordinaryCard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdinaryCardExists(int id)
        {
            return _context.OrdinaryCard.Any(e => e.CardId == id);
        }
    }
    public class RegisterRequest
    {
        public string Address { get; set; }
        public string Password { get; set; }
    }
}
