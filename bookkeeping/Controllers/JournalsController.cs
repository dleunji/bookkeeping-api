using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bookkeeping.Models;
using System.Globalization;

namespace bookkeeping
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController : ControllerBase
    {
        private readonly BookKeepingDbContext _context;

        public JournalsController(BookKeepingDbContext context)
        {
            _context = context;
        }

        // 특정 유저의 최신 저널 내역
        [HttpGet("latest/{userId}/{page}")]
        public async Task<ActionResult<PagedJournal>> GetLatestJournalsByUserId(int userId, int page)
        {
            var pageSize = 10;
            try
            {
                var journals = await _context.Journal
                .AsNoTracking()
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.JournalId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(j => j.Elements)
                .ToListAsync();

                if (journals == null)
                {
                    return NotFound();
                }
                var totalRowsCount = await _context.Journal
                .AsNoTracking()
                .Where(j => j.UserId == userId)
                .CountAsync();

                return new PagedJournal(totalRowsCount, (int)Math.Ceiling(totalRowsCount / (double)pageSize), journals);
            }
            catch(ArgumentException)
            {
                return BadRequest("파라미터를 확인하세요");
            }
        }

        // 5개씩 일자별 저널 불러오기
        // lastJournalId는 Optional
        [HttpGet("{userId}/{date}/{page}")]
        public async Task<ActionResult<PagedJournal>> GetDailyJournalsByUserId(int userId, string date, int page)
        {
            var pageSize = 5;
            try
            {
                // Parsing
                DateTime dateTime = DateTime.Parse(date);
                var journals = await _context.Journal
                    .AsNoTracking()
                    .Where(j => j.UserId == userId && j.TransactedAt.Date == dateTime.Date)
                    .Include(j => j.Elements)
                    .OrderByDescending(j => j.JournalId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (journals == null)
                {
                    return NotFound();
                }

                var totalRowsCount = await _context.Journal
                    .AsNoTracking()
                    .Where(j => j.UserId == userId && j.TransactedAt.Date == dateTime.Date)
                    .CountAsync();

                return new PagedJournal(totalRowsCount, (int) Math.Ceiling(totalRowsCount / (double)pageSize), journals);
            }
            catch (FormatException)
            {
                return BadRequest("날짜 포맷을 확인하세요");
            }
        }

        // 저널 생성
        [HttpPost]
        public async Task<ActionResult<Journal>> PostJournal(Journal journal)
        {
            // 저널 요소에 따른 유저의 잔액 변경
            var user = await _context.User.FindAsync(journal.UserId);
            if(user == null)
            {
                return NotFound();
            }
            journal.PrevPocketBalance = user.PocketBalance;
            journal.PrevAccBalance = user.AccBalance;
            journal.PrevUnpaidBill = user.UnpaidBill;
            foreach(var element in journal.Elements)
            {
                switch (element.CategoryId)
                {
                    // case 1001:
                    //     // 현금
                    //     user.PocketBalance += element.Sign > 0 ? element.Amount : -element.Amount;
                    //     break;
                    // case 1002:
                    //     // 예금
                    //     user.AccBalance += element.Sign > 0 ? element.Amount : -element.Amount;
                    //     break;
                    case 1003:
                        // 넥토머니
                        user.PocketBalance += element.Sign > 0 ? element.Amount : -element.Amount;
                        break;
                    // case 1004:
                    //     // 상품권
                    //     user.PocketBalance += element.Sign > 0 ? element.Amount : -element.Amount;
                    //     break;
                    // case 1005:
                    //     // 연결계좌
                    //     user.PocketBalance += element.Sign > 0 ? element.Amount : -element.Amount;
                    //     break;
                    // case 2001:
                    //     // 카드 대금
                    //     user.UnpaidBill += element.Sign > 0 ? element.Amount : -element.Amount;
                    //     break;
                    // case 2002:
                    //     // 휴대폰 대금
                    //     user.UnpaidBill += element.Sign > 0 ? element.Amount : -element.Amount;
                    //     break;
                    // case 2003:
                    //     // 후불결제 대금
                    //     user.UnpaidBill += element.Sign > 0 ? element.Amount : -element.Amount;
                    //     break;

                    default:
                        break;
                }
            }
            journal.PocketBalance = user.PocketBalance;
            journal.AccBalance = user.AccBalance;
            journal.UnpaidBill = user.UnpaidBill;
            _context.Journal.Add(journal);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(journal);
        }

        // 저널 삭제
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJournal(int id)
        {
            var journal = await _context.Journal
                .Where(j => j.JournalId == id)
                .Include(j => j.Elements)
                .FirstOrDefaultAsync();
            if (journal == null)
            {
                return NotFound();
            }

            _context.Journal.Update(journal);
            var user = await _context.User.FindAsync(journal.UserId);

            if(user == null)
            {
                return NotFound();
            }

            journal.DeletedYn = true;
            journal.PrevPocketBalance = user.PocketBalance;
            journal.PrevAccBalance = user.AccBalance;
            journal.UnpaidBill = user.UnpaidBill;
            foreach (var element in journal.Elements)
            {
                // 현금
                switch (element.CategoryId)
                {
                    case 1001:
                        // 현금
                        user.PocketBalance += element.Sign < 0 ? element.Amount : -element.Amount;
                        break;
                    case 1002:
                        // 예금
                        user.AccBalance += element.Sign < 0 ? element.Amount : -element.Amount;
                        break;
                    case 2001:
                        // 카드 대금
                        user.UnpaidBill += element.Sign < 0 ? element.Amount : -element.Amount;
                        break;
                    case 1003:
                        // 넥토머니
                        user.PocketBalance += element.Sign < 0 ? element.Amount : -element.Amount;
                        break;
                }
            }
            journal.PocketBalance = user.PocketBalance;
            journal.AccBalance = user.AccBalance;
            journal.UnpaidBill = user.UnpaidBill;

            try
            { 
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(journal);
        }
    }
}
