using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.API.Models;

namespace SpotifyClone.API.Data;

public class ApplicationDbContext : DbContext
{
    // Constructor (recibe la config y se la pasa a la clase padre de EF)
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    //TABLAS
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Playlist> Playlists { get; set; } = null!;
    public DbSet<PlaylistContenido> PlaylistContenidos { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PlaylistContenido>()
        .HasOne(pc => pc.Playlist)
        .WithMany(p => p.Contenidos)
        .HasForeignKey(pc => pc.PlaylistId)
        .OnDelete(DeleteBehavior.Cascade);

    }
    

}