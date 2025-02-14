using System.ComponentModel.DataAnnotations;

namespace ES_FrontEnd.Areas.Admin.Models
{
    public class UserModel
    {
        public int? UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Password Length Must Contain 6 to 12")]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
