namespace SpotifyClone.API.Services;
using SpotifyClone.API.DTOs;

public interface ISpotifyAuthService
    {
        string ObtenerUrlAutorización();
        Task<SpotifyTokenResponseDto?> IntercambiarCodigoPorTokensAsync(string code);
        Task<SpotifyTokenResponseDto?> RefrescarTokenAsync(string refreshToken);
}

