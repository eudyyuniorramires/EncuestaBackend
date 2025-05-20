
namespace Entidades.DTOs
{
    public class EncuestaResueltaDto
    {
        public int? UsuarioId { get; set; }

        public int EncuestaId { get; set; }
        public List<RespuestaDto> Respuestas { get; set; }
    }
}
