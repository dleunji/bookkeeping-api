using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookkeeping.Models
{
    public class Journal
    {
        [Column("journal_id")]
        public int JournalId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("transacted_at")]
        public DateTime TransactedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Today;

        [Column("summary")]
        public string Summary { get; set; }

        [Column("prev_acc_balance")]
        public int PrevAccBalance { get; set; }

        [Column("acc_balance")]
        public int AccBalance { get; set; }

        [Column("prev_unpaid_bill")]
        public int PrevUnpaidBill { get; set; }

        [Column("unpaid_bill")]
        public int UnpaidBill { get; set; }

        [Column("prev_pocket_balance")]
        public int PrevPocketBalance { get; set; }

        [Column("pocket_balance")]
        public int PocketBalance { get; set; }

        [Column("total_amount")]
        public int TotalAmount { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("note")]
        public string Note { get; set; }

        [Column("deleted_yn")] public bool DeletedYn { get; set; } = false;

        public IEnumerable<Element> Elements { get; set; }
    }
}
