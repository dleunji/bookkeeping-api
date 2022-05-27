using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookkeeping.Models
{
    public class RegisteredAccount
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("account_address")]
        public string AccountAddress { get; set; }
        
        [Column("registered_account_password")]
        public string RegisteredAccountPassword { get; set; }

        [Column("bank")]
        public string Bank { get; set; }
    }
}