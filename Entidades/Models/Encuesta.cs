namespace Entidades.Models
{
    public class Encuesta
    {
        public int Id { get; set; }


        public string Titulo { get; set; } = "";

        public string Descripcion { get; set; } = "";


        public bool EsPrivada { get; set; }

        public DateTime FechaExpiracion { get; set; }

        public int CreadorId { get; set; }

        public Usuario? Creador { get; set; }

        public ICollection<Pregunta>? Preguntas { get; set; }


    }
}
