using SpotifyClone.API.DTOs;

namespace SpotifyClone.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginWithGoogleAsync(GoogleLoginDto dto);

}