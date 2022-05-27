using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookkeeping.Models
{
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("user_first_name")]
        public string UserFirstName { get; set; }

        [Column("user_last_name")]
        public string UserLastName { get; set; }

        [Column("pocket_balance")]
        public int PocketBalance { get; set; }

        [Column("acc_balance")]
        public int AccBalance { get; set; }

        [Column("unpaid_bill")]
        public int UnpaidBill { get; set; }

        [Column("deleted_yn")]
        public bool DeletedYn { get; set; } = false;


        public virtual List<Journal> Journals { get; set; }
    }
}
