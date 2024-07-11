using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chart_of_account.Models
{
    public class User_Login
    {
        [Key]
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullNamee { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
        public string Photo { get; set; }
       
    }
}