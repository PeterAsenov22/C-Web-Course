namespace RunesWebApp.Data
{
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class RunesContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<TrackAlbum> TracksAlbums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=DESKTOP-9TBAQKA\\SQLEXPRESS;Database=Runes;Integrated Security=true;")
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<TrackAlbum>()
                .HasKey(ta => new {ta.AlbumId, ta.TrackId});
        }
    }
}
