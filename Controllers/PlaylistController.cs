using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.API.DTOs;
using SpotifyClone.API.Services;

namespace SpotifyClone.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class PLaylistController : ControllerBase
{
    
    private readonly IPlaylistService _playlistService;

    public PLaylistController (IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    [HttpPost("usuario/{UsuarioId}")]

    public async Task<IActionResult> CrearPlaylist(int usuarioId, [FromBody] CrearPlaylistDto dto)
    {
        var resultado = await _playlistService.CrearAsync(usuarioId, dto);
        return CreatedAtAction(nameof(ObtenerPlaylist), new {id = resultado.Id}, resultado);
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> ObtenerPlaylist(int id)
    {
        var playlist = await _playlistService.ObtenerPorIdAsync(id);
        if(playlist == null) return NotFound("La playlist solicitada no existe");
        return Ok(playlist);
    }

    [HttpPost("{playlistId}/contenido")]

    public async Task<IActionResult> AgregarContenido(int playlistId, [FromBody]AgregarContenidoPlaylistDto dto)
    {
        var exito = await _playlistService.AgregarContenidoAsync(playlistId, dto);
        if (!exito)
        {
            return BadRequest("No se pudo agregar el contenido a la playlist");
        } return Ok(new {mensaje = "El contenido se agregó con exito"});
    }
}