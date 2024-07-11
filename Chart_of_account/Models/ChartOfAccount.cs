using System.ComponentModel;

namespace Chart_of_account.Models
{
    public class ChartOfAccount
    {
        public int AccountId { get; set; }
        public string AccountTitle { get; set; }
        [DisplayName("AccountClassTitle")]
        public int AccountClassId { get; set; }

        // Navigation property to represent the associated AccountClass
        public AccountClass AccountClass { get; set; }
    }
}