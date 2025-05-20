namespace Entidades.DTOs
{
    public class EncuestaCrearDto
    {

        public int IdCreador { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }

        public bool EsPrivada { get; set; }

        public DateTime FechaExpiracion { get; set; }
        public List<PreguntaDto> Preguntas { get; set; }



    }
}
