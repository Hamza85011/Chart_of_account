using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chart_of_account.Models
{
    public class VoucherModel
    {
        public int VoucherId { get; set; }
        [ForeignKey("VoucherType")]
        public int VoucherTypeId { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual VoucherType VoucherType { get; set; }
    }
}
