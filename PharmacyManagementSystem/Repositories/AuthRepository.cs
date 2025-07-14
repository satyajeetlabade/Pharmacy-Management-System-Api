using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.DTOS.Authentication;
using PharmacyManagementSystem.Interfaces;
using PharmacyManagementSystem.Models;
using System.Threading.Tasks;
using BCrypt.Net;

namespace PharmacyManagementSystem.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthRepository(ApplicationDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task RegisterAsync(RegisterRequestDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
                if (exists)
                    throw new Exception("Email already exists.");

                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8)
                    throw new Exception("Password must be at least 8 characters long.");

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = hashedPassword,
                    Contact = dto.Contact,
                    Role = dto.Role
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var subject = "Welcome to Pharmacy Management System";
                var body = $"<h3>Hi {user.Name},</h3><p>Thank you for registering!</p>";
                await _emailService.SendEmailAsync(user.Email, subject, body);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("An error occurred during registration: " + ex.Message);
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new Exception("Invalid credentials.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                NotBefore = DateTime.UtcNow,
                Audience = "PharmacyClients",
                Issuer = "PharmacyAPI",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new LoginResponseDto
            {
                Token = jwt,
                UserId = user.Id,
                Name = user.Name,
                Role = user.Role.ToString(),
                Email = user.Email,
    
            };
        }
    }
}
