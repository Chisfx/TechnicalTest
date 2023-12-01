using System.ComponentModel.DataAnnotations;
namespace TechnicalTest.Application.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = default!;

        [Required]
        public string OrganizationId { get; set; }
    }
}
