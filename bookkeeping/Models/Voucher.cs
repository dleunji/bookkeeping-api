using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace bookkeeping.Models
{
    public class Voucher
    {
        [Column("voucher_code")]
        public string VoucherCode { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("used")]
        public bool Used { get; set; }
    }
}
