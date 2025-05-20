namespace Entidades.Models
{
    public class Opcion
    {
        public int Id { get; set; }

        public string Texto { get; set; } = "";

        public int PreguntaId { get; set; }


        public Pregunta? Pregunta { get; set; }
    }
}
