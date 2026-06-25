using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.API.Data;
using SpotifyClone.API.Models;
using SpotifyClone.API.DTOs;


namespace SpotifyClone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //setea la ruta para que este controller sea de usuario "api/usuario"
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }
        [HttpGet("ById/{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
            
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            return Ok(usuario);
        }
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioRequestDto usuarioRequest)
        {   if (string.IsNullOrWhiteSpace(usuarioRequest.Nombre) || string.IsNullOrWhiteSpace(usuarioRequest.Email))
            {
                throw new Exception("Los campos son obligatorios");
            }
            var usuario = new Usuario ()
            {
            GoogleId = "HolaPapá22",
            Nombre = usuarioRequest.Nombre,
            Email = usuarioRequest.Email,
            };
            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new{id=usuario.Id}, usuario);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> UpdateUsuario (UsuarioRequestDto usuarioRequest, int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                throw new Exception("No se encontró el Usuario");
            }

            if (!string.IsNullOrWhiteSpace(usuarioRequest.Nombre)) usuario.Nombre = usuarioRequest.Nombre;
            if (!string.IsNullOrWhiteSpace(usuarioRequest.Email))  usuario.Email = usuarioRequest.Email;

            
            await _context.SaveChangesAsync();
            return NoContent();
        }
    [HttpDelete]
    public async Task<ActionResult> DeleteUsuario([FromQuery] int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                throw new Exception("El usuario ingresado no está registrado");
            }

            _context.Usuarios.Remove(usuario);

            await _context.SaveChangesAsync();
            return NoContent();//Setear mensaje de eliminación correcta en el Front
        }
}
}