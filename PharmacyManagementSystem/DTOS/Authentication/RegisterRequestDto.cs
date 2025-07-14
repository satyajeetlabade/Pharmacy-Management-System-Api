using PharmacyManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace PharmacyManagementSystem.DTOS.Authentication
{
    public class RegisterRequestDto
    {
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Phone]
        public string Contact { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}
