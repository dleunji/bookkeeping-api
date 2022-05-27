using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bookkeeping.Models;

namespace bookkeeping
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BookKeepingDbContext _context;

        public UsersController(BookKeepingDbContext context)
        {
            _context = context;
        }

        // 유저 리스트 불러오기
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User
                .AsNoTracking()
                .ToListAsync();
        }

        // 선택한 유저의 정보와 최대 10개의 최신 저널, 그리고 이에 대한 각 요소 모두 가져오기
        // [HttpGet("{id}")]
        // public async Task<ActionResult<User>> GetUser(int id)
        // {
        //     var user = await _context.User
        //         .AsNoTracking()
        //         .Include(u => u.Journals.OrderByDescending(j => j.JournalId).Take(10))
        //             .ThenInclude(j => j.Elements)
        //         .FirstOrDefaultAsync(u => u.UserId == id);
        //
        //     if (user == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return user;
        // }

        // 유저 생성
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // 특정 유저 삭제
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            user.DeletedYn = true;
            
            try
            {
                _context.User.Update(user);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                Console.WriteLine("업데이트 중 예외가 발생했습니다.");
            }

            return NoContent();

        }
    }
}
