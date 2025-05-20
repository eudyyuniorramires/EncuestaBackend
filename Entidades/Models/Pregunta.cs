using Entidades.Models;
using Entidades.Models.Enumeraciones;

namespace Entidades.Models
{
    public class Pregunta
    {
        public int Id { get; set; }

        public string Texto { get; set; } = "";

        public TipoPregunta Tipo { get; set; }

        public int EncuestaId { get; set; } 

        public Encuesta? Encuesta { get; set; }

        public ICollection<Opcion>  Opciones { get; set; } 


        public List<Respuesta> Respuestas { get; set; } = new ();
    }
}
