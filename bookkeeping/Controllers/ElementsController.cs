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
    public class ElementsController : ControllerBase
    {
        private readonly BookKeepingDbContext _context;

        public ElementsController(BookKeepingDbContext context)
        {
            _context = context;
        }

        // 특정 유저의 계정별 원장 작성용
        [HttpGet("{userId}/{categoryId}/{year}/{month}/{page}")]
        public async Task<ActionResult<PagedElement>> GetElementsByCategory(int userId, int categoryId, int year, int month, int page)
        {
            var pageSize = 10;
            try
            {
                var totalRowsCount = await _context.Element
                .AsNoTracking()
                .Join(
                    _context.Journal,
                    element => element.JournalId,
                    journal => journal.JournalId,
                    (element, journal) => new
                    {
                        element.ElementId,
                        journal.JournalId,
                        journal.UserId,
                        element.CategoryId,
                        journal.TransactedAt,
                    }
                )
                .Where(e => e.UserId == userId && e.CategoryId == categoryId && e.TransactedAt.Year == year &&
                            e.TransactedAt.Month == month)
                .CountAsync();
                var elements = await _context.Element
                    .AsNoTracking()
                    .Join(
                        _context.Journal,
                        element => element.JournalId,
                        journal => journal.JournalId,
                        (element, journal) => new PagedElement.JoinedElement
                        {
                            ElementId = element.ElementId,
                            JournalId = journal.JournalId,
                            UserId = journal.UserId,
                            CategoryId = element.CategoryId,
                            Amount = element.Amount,
                            Sign = element.Sign,
                            TransactedAt = journal.TransactedAt,
                            Summary = journal.Summary
                        }
                    )
                    .Where(e => e.UserId == userId && e.CategoryId == categoryId && e.TransactedAt.Year == year &&
                                e.TransactedAt.Month == month)
                    .OrderBy(e => e.TransactedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return new PagedElement(totalRowsCount, (int)Math.Ceiling(totalRowsCount / (double)pageSize), elements);
            }
            catch (ArgumentException)
            {
                return BadRequest("파라미터를 확인하세요");

            }

        }

        // 수익 비용의 통계를 위해 카테고리별 금액 합계
        [HttpGet("{userId}/{year}/{month}")]
        public IQueryable GetStatistics(int userId, int year, int month)
        {
            var elements = _context.Element
                .AsNoTracking()
                .Join(
                    _context.Journal,
                    element => element.JournalId,
                    journal => journal.JournalId,
                    (element, journal) => new PagedElement.JoinedElement
                    {
                        ElementId = element.ElementId,
                        JournalId = journal.JournalId,
                        UserId = journal.UserId,
                        CategoryId = element.CategoryId,
                        Amount = element.Amount,
                        Sign = element.Sign,
                        TransactedAt = journal.TransactedAt,
                        Summary = journal.Summary
                    }
                )
                .Where(e => e.UserId == userId
                            && e.TransactedAt.Year == year
                            && e.TransactedAt.Month == month
                            && e.CategoryId > 3000)
                .GroupBy(e => e.CategoryId)
                .Select(g => new
                {
                    g.Key,
                    Amount = g.Sum(e => e.Amount)
                });
            return elements;
        }
    }
}
