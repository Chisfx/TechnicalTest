using Microsoft.AspNetCore.Identity;
namespace TechnicalTest.Domain.Entities
{
    public class User : IdentityUser
    {
        public string OrganizationId { get; set; }
    }
}
