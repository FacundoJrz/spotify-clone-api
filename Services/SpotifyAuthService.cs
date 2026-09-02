using SpotifyClone.API.DTOs;
using System.Net.Http.Headers;
using System.Text;

namespace SpotifyClone.API.Services
{
    public class SpotifyAuthService : ISpotifyAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public SpotifyAuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public string ObtenerUrlAutorización()
        {
            var clientId = _configuration["Authentication:Spotify:ClientId"]
                ?? throw new InvalidOperationException("Falta configurar 'Spotify:ClientId' en appsettings.json");
            var redirectUri = _configuration["Authentication:Spotify:RedirectUri"]
                ?? throw new InvalidOperationException("Falta configurar 'Spotify:RedirectUri en appsettings.json");

            var scopes = "streaming user-read-email user-read-private user-read-playback-state user-modify-playback-state";
            return $"https://accounts.spotify.com/authorize?" +
                    $"response_type=code" +
                    $"&client_id={Uri.EscapeDataString(clientId)}" +
                    $"&scope={Uri.EscapeDataString(scopes)}" +
                    $"&redirect_uri={Uri.EscapeDataString(redirectUri)}";                   
        }

        public async Task<SpotifyTokenResponseDto?> IntercambiarCodigoPorTokensAsync(string code)
        {
            var redirectUri = _configuration["Authentication:Spotify:RedirectUri"];

            var dictParams = new Dictionary<string, string>
            {
                {"grant_type", "authorization_code"},
                {"code", code},
                {"redirect_uri", redirectUri! }
            };

            return await EnviarSolicitudTokenAsync(dictParams);
        }

        public async Task<SpotifyTokenResponseDto?> RefrescarTokenAsync(string refreshToken)
        {
            var dictParams = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken }
        };

            return await EnviarSolicitudTokenAsync(dictParams);
        }
        private async Task<SpotifyTokenResponseDto?> EnviarSolicitudTokenAsync(Dictionary<string, string> payload)
        {
            var clientId = _configuration["Authentication:Spotify:ClientId"];
            var clientSecret = _configuration["Authentication:Spotify:ClientSecret"];

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token") 
            { 
                Content = new FormUrlEncodedContent(payload)
            };

            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<SpotifyTokenResponseDto>();
        }
    }
}
