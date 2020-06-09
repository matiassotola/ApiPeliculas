using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Cada vez que creamos una entidad nueva del modelo lo tenemos que declarar su
        // correspondiente DbSet, en caso de olvidarlo, cuando se cree la migración
        // la misma va a tener los métodos "Up" y "Down" vacios.

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
