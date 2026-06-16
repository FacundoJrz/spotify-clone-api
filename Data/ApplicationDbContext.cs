using Microsoft.EntityFrameworkCore;
using SpotifyClone.API.Models;

namespace SpotifyClone.API.Data;

public class ApplicationDbContext : DbContext
{
    // Declaramos constructor (recibe la config y se la pasa a la clase padre de EF)
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    //TABLAS
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Playlist> Playlists { get; set; } = null!;

    public DbSet<ContenidoAudio> ContenidosAudio { get; set; } = null!;
    public DbSet<Cancion> Canciones { get; set; } = null!;
    public DbSet<Podcast> Podcasts { get; set; } = null!;
    

}