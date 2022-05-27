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
    public class RegisteredAccountsController : ControllerBase
    {
        private readonly BookKeepingDbContext _context;

        public RegisteredAccountsController(BookKeepingDbContext context)
        {
            _context = context;
        }

        // GET: api/RegisteredAcounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegisteredAccount>>> GetRegisteredAccount()
        {
            return await _context.RegisteredAccount.ToListAsync();
        }

        /// <summary>
        /// 유저의 계좌정보 가져오기. 없으면 404 Not Found 리턴
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RegisteredAccount>> GetRegisteredAccount(int id)
        {
            var registeredAccount = await _context.RegisteredAccount.FindAsync(id);

            if (registeredAccount == null)
            {
                return NotFound();
            }

            return registeredAccount;
        }

        // PUT: api/RegisteredAcounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegisteredAccount(int id, RegisteredAccount registeredAccount)
        {
            if (id != registeredAccount.UserId)
            {
                return BadRequest();
            }

            _context.Entry(registeredAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegisteredAccountExists(id))
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

        // POST: api/RegisteredAcounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RegisteredAccount>> PostRegisteredAccount(RegisteredAccount registeredAccount)
        {
            _context.RegisteredAccount.Add(registeredAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegisteredAccount", new { id = registeredAccount.UserId }, registeredAccount);
        }

        // DELETE: api/RegisteredAcounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredAccount(int id)
        {
            var registeredAccount = await _context.RegisteredAccount.FindAsync(id);
            if (registeredAccount == null)
            {
                return NotFound();
            }

            _context.RegisteredAccount.Remove(registeredAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegisteredAccountExists(int id)
        {
            return _context.RegisteredAccount.Any(e => e.UserId == id);
        }
    }
}
