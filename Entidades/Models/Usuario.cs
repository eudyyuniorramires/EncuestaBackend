
using Entidades.Models.Enumeraciones;

namespace Entidades.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Correo { get; set; } = "";

        public string PasswordHash { get; set; } = "";

        public Rol Rol { get; set; }

        public ICollection<Respuesta>? Respuesta { get; set; }


    }
}
