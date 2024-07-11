using System.ComponentModel;

namespace Chart_of_account.Models
{
    public class CombineVoucherModel
    {
        public string AccountTitle { get; set; }
        public int AccountClassId { get; set; }
        public int VoucherId { get; set; }
        public int VoucherTypeId { get; set; }
        public string VoucherTypeTitle { get; set; }
        public decimal TotalAmount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int From { get; set; } 
        public int To { get; set; }
    }
}
