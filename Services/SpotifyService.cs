using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace SpotifyClone.API.Services;

public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public SpotifyService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        _clientId = configuration["Spotify:ClientID"]
        ?? throw new ArgumentNullException("Falta configurar ClientID");

        _clientSecret = configuration["Spotify:ClientSecret"]
        ?? throw new ArgumentNullException("Falta configurar el ClientSecret");
    }

        /// <summary>
        /// Método privado que se encarga del PASO 1: Autenticarse y conseguir el Token.
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
        /// Método público del contrato para el PASO 2: Buscar música.
        /// </summary>
        
        public async Task<IEnumerable<object>> BuscarContenidoAsync(string query)
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

        return new List<object>{jsonResult};

    }
    }