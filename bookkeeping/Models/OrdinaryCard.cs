using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookkeeping.Models
{
    public class OrdinaryCard
    {
        [Key]
        [Column("card_id")]
        public int CardId { get; set; }
        
        [Column("card_address")]
        public string CardAddress { get; set; }
        
        [Column("card_password")]
        public string CardPassword { get; set; }
        
        [Column("bank")]
        public string Bank { get; set; }

        [Column("cvc")]
        public string CVC { get; set; }
        
        [Column("valid_year")]
        public int ValidYear { get; set; }
        
        [Column("valid_month")]
        public int ValidMonth { get; set; }
        
        [Column("is_check")]
        public bool IsCheck { get; set; }
        
        [Column("is_registered")]
        public bool IsRegistered { get; set; }
        
        [Column("social_num")]
        public string SocialNum { get; set; }
        
        [Column("phone_num")]
        public string PhoneNum { get; set; }
    }
}