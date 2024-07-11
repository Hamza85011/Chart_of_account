using System.ComponentModel;

namespace Chart_of_account.Models
{
    public class AccountClass
    {
        [DisplayName ("AccountClassTitle")]
        public int AccountClassId { get; set; }
        public string AccountClassTitle { get; set; }

        // Navigation property to represent the one-to-many relationship
        public ICollection<ChartOfAccount> ChartOfAccounts { get; set; }
    }
}