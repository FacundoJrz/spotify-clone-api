using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using SpotifyClone.API.DTOs;


namespace SpotifyClone.API.Services;

public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId; 
        private readonly string _clientSecret;

        public SpotifyService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        _clientId = configuration["Authentication:Spotify:ClientID"]
        ?? throw new ArgumentNullException("Falta configurar ClientID");

        _clientSecret = configuration["Authentication:Spotify:ClientSecret"]
        ?? throw new ArgumentNullException("Falta configurar el ClientSecret");
    }

        /// <summary>
        /// Método privado que se encarga del PASO 1: Autenticarse y conseguir el Token
        /// </summary>
        
        private async Task<string> ObtenerTokenAsync(){
        _httpClient.DefaultRequestHeaders.Authorization = null;
        var credenciales = $"{_clientId}:{_clientSecret}";
        var credencialesBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(credenciales));    

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credencialesBase64);

        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var jsonResult = await response.Content.ReadFromJsonAsync<JsonElement>();
            
            // Extraemos el string de la propiedad "access_token"
            return jsonResult.GetProperty("access_token").GetString() 
                ?? throw new Exception("No se pudo obtener el access_token del JSON de Spotify.");
        }

        /// <summary>
        /// Método público del contrato para el PASO 2: Buscar archivos
        /// </summary>
        
        public async Task<IEnumerable<ContenidoAudioDto>> BuscarContenidoAsync(string query)
    {

        var token = await ObtenerTokenAsync();

        var url = $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(query)}&type=track,show&limit=10";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error en la búsqueda de Spotify. Status: {response.StatusCode}");
        }

        var jsonResult = await response.Content.ReadFromJsonAsync<JsonElement>();

        var resultadosUnificados = new List<ContenidoAudioDto>();

        if(jsonResult.TryGetProperty("tracks", out var tracksProp))
        {
            var cancionesMapeadas = tracksProp.GetProperty("items").EnumerateArray().Select(track =>
            {   
                string spotifyId = track.TryGetProperty("id", out var idProp) ? idProp.GetString()?? string.Empty : string.Empty;
                string nombre = track.TryGetProperty("name", out var nameProp) ? nameProp.GetString()?? "Sin titulo": "sin titulo";
                
                string creador = "Artista desconocido";
                if(track.TryGetProperty("artists", out var artistProp) && artistProp.GetArrayLength() > 0)
                {
                    var primerArtista = artistProp[0];
                    creador = primerArtista.TryGetProperty("name", out var artistNameProp) ? artistNameProp.GetString() ?? "Artista desconocido" : "Artista desconocido"; 
                }

                string imagenUrl = string.Empty; 
                if (track.TryGetProperty("album", out var albumProp) && 
                albumProp.TryGetProperty("images", out var imagesProp) 
                && imagesProp.GetArrayLength() > 0)
                { 
                    var primeraImagen = imagesProp[0];
                    imagenUrl = primeraImagen.TryGetProperty("url", out var urlProp) ? urlProp.GetString() ?? string.Empty : string.Empty;
                }

                int duracionMs = track.TryGetProperty("duration_ms", out var durationProp) ? durationProp.GetInt32() : 0;
                string? previewUrl = track.TryGetProperty("preview_url", out var previewProp) ? previewProp.GetString() : null;
                string? uri = track.TryGetProperty("uri", out var uriProp) ? uriProp.GetString() : null;
                return new ContenidoAudioDto
                { 
                    SpotifyId = spotifyId,
                    Nombre = nombre,
                    Creador = creador,
                    ImagenUrl = imagenUrl,
                    DuracionMs = duracionMs,
                    Tipo = "Cancion",
                    PreviewUrl = previewUrl,
                    Uri = uri

                };
            });
            resultadosUnificados.AddRange(cancionesMapeadas);
        } 
        if (jsonResult.TryGetProperty("shows", out var showsProp))
        {
            var podcastsMapeados = showsProp.GetProperty("items").EnumerateArray().Select(show => 
            {
                string spotifyId = show.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? string.Empty : string.Empty;
                string nombre = show.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? "Sin título" : "Sin título";
                string creador = show.TryGetProperty("publisher", out var pubProp) ? pubProp.GetString() ?? "Creador desconocido" : "Creador desconocido";

                
                string imagenUrl = string.Empty;
                if (show.TryGetProperty("images", out var imagesProp) && imagesProp.GetArrayLength() > 0)
                {
                    var primeraImagen = imagesProp[0];
                    imagenUrl = primeraImagen.TryGetProperty("url", out var urlProp) ? urlProp.GetString() ?? string.Empty : string.Empty;
                }

                return new ContenidoAudioDto
                {
                    SpotifyId = spotifyId,
                    Nombre = nombre,
                    Creador = creador,
                    ImagenUrl = imagenUrl,
                    DuracionMs = 0,
                    Tipo = "Podcast",
                    PreviewUrl = null
                };
            });
            resultadosUnificados.AddRange(podcastsMapeados);
        }
        return resultadosUnificados;
    }
     /// <summary>
    /// Método público del contrato para el PASO 3: Buscar canciones
    /// </summary>
    public async Task<CancionDetalleDto?> ObtenerCancionPorIdAsync(string id)
    {
        var token = await ObtenerTokenAsync();
        var url = $"https://api.spotify.com/v1/tracks/{id}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error al obtener la canción: {response.StatusCode}");
        }

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        //mapeo de propiedades para el DTO

        string nombre = json.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? "Sin titulo" : "Sin titulo";
        int duracionMs = json.TryGetProperty("duration_ms", out var duracionProp) ? duracionProp.GetInt32() : 0;
        string? previewUrl = json.TryGetProperty("preview_url", out var previewProp) ? previewProp.GetString() : null;

        //Recorrer el array de album para obtener nombre, imagen y duración

        string albumNombre = "Album desconocido";
        string fechaLanzamiento = "Fecha desconocida";
        string imagenUrl = string.Empty; 

        if (json.TryGetProperty("album", out var albumProp))
        { 
            albumNombre = albumProp.TryGetProperty("name", out var albumNameProp) ? albumNameProp.GetString() ?? "Album desconocido" : "Album desconocido";
            fechaLanzamiento = albumProp.TryGetProperty("release_date", out var lanzamientoProp) ? lanzamientoProp.GetString() ?? "Fecha desconocida" : "Fecha desconocida";
        }
        if (albumProp.TryGetProperty("images", out var imagesProp) && imagesProp.GetArrayLength() > 0)
        {
            imagenUrl = imagesProp[0].TryGetProperty("url", out var urlProp) ? urlProp.GetString() ?? string.Empty : string.Empty;  
        }

        //Recorrer el array de artist para obtener el nombre del artista principal

        string creador = "Artista desconocido";

        if (json.TryGetProperty("artists", out var artistProp) && artistProp.GetArrayLength() > 0)
        {
            creador = artistProp[0].TryGetProperty("name", out var artistNameProp) ? artistNameProp.GetString() ?? "Artista desconocido" : "Artista desconocido";
        }

        return new CancionDetalleDto
        {
            Id = id,
            Nombre = nombre,
            Album = albumNombre,
            Creador = creador,
            ImagenUrl = imagenUrl,
            Duracion = duracionMs,
            FechaLanzamiento = fechaLanzamiento,
            PreviewUrl = previewUrl
        };   

    }
    public async Task<PodcastDetalleDto> ObtenerPodcastPorIdAsync(string id)
    {
        var token = await ObtenerTokenAsync();
        var url = $"https://api.spotify.com/v1/shows/{id}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if(response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error al buscar Podcast: {response.StatusCode}");
        }

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        //Mapeo de propiedades para el Podcast
        string nombre = json.TryGetProperty("name", out var nombreProp) ? nombreProp.GetString ()?? "Sin titulo" : "Sin titulo";
        string descripcion = json.TryGetProperty("description", out var descripcionProp) ? descripcionProp.GetString() ?? "Sin descripcion" : "Sin descripcion";
        string creador = json.TryGetProperty("publisher", out var creadorProp) ? creadorProp.GetString() ?? "Artista desconocido" : "Artista desconocido";
        int totalEpisodios = json.TryGetProperty("total_episodes", out var totalEpisodiosProp) ? totalEpisodiosProp.GetInt32() : 0;

        //Recorrer el array de imagenes para obtener imagen
        string imagenUrl = string.Empty;
        if(json.TryGetProperty("images", out var imagesProp) && imagesProp.GetArrayLength() > 0)
        {
            imagenUrl = imagesProp[0].TryGetProperty("url", out var urlProp) ? urlProp.GetString() ?? string.Empty : string.Empty;
        }

        return new PodcastDetalleDto
        {
            Id = id,
            Nombre = nombre,
            Descripcion = descripcion,
            Creador = creador,
            ImagenUrl = imagenUrl,
            TotalEpisodios = totalEpisodios
        };
    }
    }