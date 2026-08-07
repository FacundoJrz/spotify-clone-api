using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.API.Data;
using SpotifyClone.API.Models;
using SpotifyClone.API.DTOs;
using SpotifyClone.API.Services;


namespace SpotifyClone.API.Controllers;

    [ApiController]
    [Route("api/[controller]")] //setea la ruta para que este controller sea de usuario "api/usuario"
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
            
        {
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);
            if(usuario == null) return NotFound("El usuario solicitado no existe");
            return Ok(usuario);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] UsuarioRequestDto usuarioRequest)
        {
            try
            {
                var resultado = await _usuarioService.CrearAsync(usuarioRequest);
                return CreatedAtAction(nameof(GetUsuario), new {id = resultado.Id}, resultado);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario (int id, [FromBody] UsuarioRequestDto usuarioRequest)
        {
            var exito = await _usuarioService.ActualizarAsync(id, usuarioRequest);
            if (!exito) return NotFound("No se encontró el usuario solicitado");
            return NoContent();
            
        }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUsuario(int id)
        {
            var exito = await _usuarioService.EliminarAsync(id);
            if(!exito) return NotFound("No se encontró el usuario solicitado");
            return NoContent();
            
        }
}