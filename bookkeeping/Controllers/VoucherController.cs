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
    public class VoucherController : ControllerBase
    {
        private readonly BookKeepingDbContext _context;

        public VoucherController(BookKeepingDbContext context)
        {
            _context = context;
        }

        // 특정 유저의 최신 저널 내역
        [HttpGet("{voucherCode}")]
        public async Task<ActionResult<Voucher>> GetVoucher(string voucherCode)
        {
            var pageSize = 10;
            try
            {
                var voucher = await _context.Voucher
                .AsNoTracking()
                .Where(v => v.VoucherCode == voucherCode)
                .ToListAsync();

                if (voucher == null)
                {
                    return NotFound();
                }
                if (voucher[0].Used == true)
                {
                    return BadRequest("이미 사용한 번호입니다");
                }


                return Ok(voucher[0]);
            }
            catch (ArgumentException)
            {
                return BadRequest("파라미터를 확인하세요");
            }
        }
    }
}
