using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SpotifyClone.API.Data;
using SpotifyClone.API.DTOs;
using SpotifyClone.API.Models;

namespace SpotifyClone.API.Services;

public class PlaylistService : IPlaylistService
{
    private readonly ApplicationDbContext _context;

    public PlaylistService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlaylistDto>> ObtenerPlaylistsUsuarioAsync(int usuarioId)
    {
        var playlists = await _context.Playlists
        .Where(p => p.UsuarioId == usuarioId)
        .Include(p => p.Contenidos)
        .ToListAsync();

        if (playlists == null || playlists.Count == 0) return new List<PlaylistDto>();

        var dtos = new List<PlaylistDto>();

        foreach (var playlist in playlists)
        {
            var dto = new PlaylistDto
            {
                Id = playlist.Id,
                NombrePlaylist = playlist.Nombre,
                Descripcion = playlist.Descripcion,
                FechaCreacion = playlist.FechaCreacion,
                Elementos = playlist.Contenidos.Select( 
                    c => new ContenidoAudioDto
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        Creador = c.Creador,
                        ImagenUrl = c.ImagenUrl,
                        DuracionMs = c.DuracionMs,
                        Tipo = c.Tipo
                    }
                    ).ToList()
            };
            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<PlaylistDto?> ObtenerPorIdAsync(int id)
    {
        var playlist = await _context.Playlists
        .Include(p =>p.Contenidos)
        .FirstOrDefaultAsync(p=> p.Id == id);

        if (playlist == null) return null;

        var dto = new PlaylistDto
        {
            Id = playlist.Id,
            NombrePlaylist = playlist.Nombre,
            Descripcion = playlist.Descripcion,
            FechaCreacion = playlist.FechaCreacion,

            Elementos = playlist.Contenidos.Select(c => new ContenidoAudioDto {
                Id = c.Id,
                SpotifyId = c.SpotifyId,
                Nombre = c.Nombre,
                Creador = c.Creador,
                ImagenUrl = c.ImagenUrl,
                DuracionMs = c.DuracionMs,
                Tipo = c.Tipo,
                PreviewUrl = c.PreviewUrl
            }).ToList()
        };
        return dto;
    }

    public async Task<PlaylistDto> CrearAsync(int usuarioId, CrearPlaylistDto dto)
    {
        var nuevaPlaylist = new Playlist // Entidad que será una tabla
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            UsuarioId = usuarioId,
            FechaCreacion = DateTime.UtcNow
        };
        
        _context.Playlists.Add(nuevaPlaylist);
        await _context.SaveChangesAsync();

        return new PlaylistDto
        {
            Id = nuevaPlaylist.Id,
            NombrePlaylist = nuevaPlaylist.Nombre,
            Descripcion = nuevaPlaylist.Descripcion,
            FechaCreacion = nuevaPlaylist.FechaCreacion
        };


    } 

    public async Task<bool> AgregarContenidoAsync(int playlistId, AgregarContenidoPlaylistDto dto)
    {
        var nuevoContenido = new PlaylistContenido
        {
            PlaylistId = playlistId,
            Id = dto.Id,
            SpotifyId = dto.SpotifyId,
            Tipo = dto.Tipo,
            Nombre = dto.Nombre,
            Creador = dto.Creador,
            ImagenUrl = dto.ImagenUrl,
            DuracionMs = dto.DuracionMs,
            PreviewUrl = dto.PreviewUrl
        };

        _context.PlaylistContenidos.Add(nuevoContenido);
        var filasAfectadas = await _context.SaveChangesAsync();

        return filasAfectadas > 0; 

        
    }

    public async Task<bool> EliminarContenidoAsync(int playlistId, int Id)
    {
        var contenido = await _context.PlaylistContenidos
            .FirstOrDefaultAsync(c => c.Id == Id && c.PlaylistId == playlistId);

        if (contenido == null) return false;

        _context.PlaylistContenidos.Remove(contenido);

        var filasAfectadas = await _context.SaveChangesAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarPlaylistAsync(int playlistId) {
        var playlist = await _context.Playlists.FindAsync(playlistId);
        if (playlist == null) return false;

        _context.Playlists.Remove(playlist);

        var filasAfectadas = await _context.SaveChangesAsync();

        return filasAfectadas > 0;
            
    }
}
