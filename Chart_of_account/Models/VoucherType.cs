using System.ComponentModel.DataAnnotations;

namespace Chart_of_account.Models
{
    public class VoucherType
    {
        [Key]
        public int VoucherTypeId { get; set; }
        [Required]
        public string VoucherTypeTitle { get; set; }
    }
}
