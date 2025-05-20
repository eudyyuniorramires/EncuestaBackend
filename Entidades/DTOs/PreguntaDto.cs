

namespace Entidades.DTOs
{
    public class PreguntaDto
    {
        public string Texto { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public List<string> Opciones { get; set; }

        public List<PreguntaDto> Preguntas { get; set; }



    }
}
