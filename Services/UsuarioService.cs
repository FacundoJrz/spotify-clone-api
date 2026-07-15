using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SpotifyClone.API.Data;
using SpotifyClone.API.DTOs;
using SpotifyClone.API.Models;

namespace SpotifyClone.API.Services;

public class UsuarioService : IUsuarioService
{
    private readonly ApplicationDbContext _context;

    public UsuarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync()
    {
        return await _context.Usuarios
        .Select(u => new UsuarioDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Email = u.Email,
            GoogleId = u.GoogleId
        }).ToListAsync();
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if(usuario == null) return null;

        return new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            GoogleId = usuario.GoogleId
        };
    }

    public async Task<UsuarioDto>CrearAsync(UsuarioRequestDto dto)
    {
        if(string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new ArgumentException("Todos los campos son requeridos");
        }

        var NuevoUsuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            GoogleId = dto.GoogleId
        };
        _context.Usuarios.Add(NuevoUsuario);
        await _context.SaveChangesAsync();

        return new UsuarioDto
        {
            Id = NuevoUsuario.Id,
            Nombre = NuevoUsuario.Nombre,
            Email = NuevoUsuario.Email,
            GoogleId = NuevoUsuario.GoogleId,
        };
    }

    public async Task<bool>ActualizarAsync(int id, UsuarioRequestDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if(usuario == null) return false;
        //verifica que en caso de ser nulos el valor del atributo se mantiene
        if(string.IsNullOrWhiteSpace(dto.Nombre)) usuario.Nombre = dto.Nombre;
        if(string.IsNullOrWhiteSpace(dto.Email)) usuario.Email = dto.Email; 

        var filasAfectadas = await _context.SaveChangesAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if(usuario==null) return false;

        _context.Usuarios.Remove(usuario);
        var filasAfectadas = await _context.SaveChangesAsync();
        return filasAfectadas > 0;

    }

}