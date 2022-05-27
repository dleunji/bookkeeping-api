using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookkeeping.Models
{
    public class Carrier
    {
        [Key]
        [Column("phone_no")]
        public string PhoneNo { get; set; }
        
        [Column("carrier_name")]
        public string CarrierName { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("phone_password")]
        public string PhonePassword { get; set; }

        [Column("social_num")] public string SocialNum { get; set; }

    }
}