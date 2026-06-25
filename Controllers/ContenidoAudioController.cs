using Microsoft.AspNetCore.Mvc;
using SpotifyClone.API.Models;
using SpotifyClone.API.Services;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpotifyClone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContenidoAudioController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;

        public ContenidoAudioController(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }
        /// <summary>
        /// Endpoint para buscar canciones y podcasts en Spotify.
        /// Ruta: GET api/contenidoaudio/buscar?query=...&...
        /// </summary>

        [HttpGet ("buscar")]
        public async Task<IActionResult> BuscarContenido([FromQuery] string query)
        {
            if(string.IsNullOrWhiteSpace(query)){
                return BadRequest("El campo de busqueda debe ser completado");
            }
            try
            {
                var resultado = await _spotifyService.BuscarContenidoAsync(query);
                return Ok(resultado);
            }
            catch(Exception ex)
            {
                return StatusCode(500,$"Error al conectar con Spotify: {ex.Message}" );
            } 
        }

    }

}