using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookkeeping.Models
{
    public class Element
    {
        [Column("element_id")]
        public int ElementId { get; set; }

        [Column("journal_id")]
        public int JournalId { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("deleted_yn")] public bool DeletedYn { get; set; } = false;

        [Column("sign")]
        public int Sign { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Today;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

    }
}
