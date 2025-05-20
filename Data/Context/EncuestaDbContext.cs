using Entidades.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class EncuestaDbContext : DbContext
    {
        public EncuestaDbContext(DbContextOptions<EncuestaDbContext> option)
            : base(option)
        {

        }


        public DbSet<Encuesta> Encuestas { get; set; }

        public DbSet<Pregunta> Preguntas { get; set; }

        public DbSet<Opcion> Opciones { get; set; }

        public DbSet<Respuesta> Respuestas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }






    }
}

