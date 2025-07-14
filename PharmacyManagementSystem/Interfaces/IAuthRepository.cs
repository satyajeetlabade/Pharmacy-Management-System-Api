using PharmacyManagementSystem.DTOS.Authentication;

namespace PharmacyManagementSystem.Interfaces
{
    public interface IAuthRepository
    {
        Task RegisterAsync(RegisterRequestDto dto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    }
}
