using System.ComponentModel;

namespace Chart_of_account.Models
{
    public class Combine_Model
    {
        public string AccountTitle { get; set; }
        [DisplayName("AccountClassTitle")]
        public int AccountClassId { get; set; }
        public string AccountClassTitle { get; set; }
    }
}