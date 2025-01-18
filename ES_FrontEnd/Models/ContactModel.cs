using System.ComponentModel.DataAnnotations;

namespace ES_FrontEnd.Models
{
    public class ContactModel
    {
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
