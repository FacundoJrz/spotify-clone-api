using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SpotifyClone.API.Services;


namespace SpotifyClone.API.Controllers
{
    [Authorize]
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

          /// <summary>
        /// Endpoint para obtener detalles de cancion
        /// Ruta: GET api/contenidoaudio/cancion/{id}
        /// </summary>
        
        [HttpGet("cancion/{id}")]

        public async Task<IActionResult> ObtenerCancionPorIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("El ID de la cancíón es obligatorio");
            }
            try
            {
                var resultado = await _spotifyService.ObtenerCancionPorIdAsync(id);
                if(resultado == null)
                {
                    return NotFound($"No se encontro la canción con el ID: {id}");
                } 
                return Ok(resultado);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error al conectar con el servidor: {ex.Message}");
            }
        }

        ///<summary>
        /// Endpoint para obtener el detalle de un podcast
        /// Ruta: GET api/contenidoaudio/podcast/{id}
        /// </summary>
        
        [HttpGet("podcast/{id}")]

        public async Task<IActionResult> ObtenerPodcastPorIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("El ID del Podcast es obligatorio");
            } 
            try{
            var resultado = await _spotifyService.ObtenerPodcastPorIdAsync(id); 
            if(resultado == null)
                {
                    return NotFound($"No se encontró el podcast con el ID: {id}");
                }
            return Ok(resultado);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"No se pudo conectar con el servidor: {ex.Message}");
            }
            
        }
    }

}