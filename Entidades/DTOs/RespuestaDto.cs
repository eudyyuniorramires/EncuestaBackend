namespace Entidades.DTOs 
{


    public class RespuestaDto
   {
    public int PreguntaId { get; set; }

    public int? NumeroRespuesta { get; set; } // Puede ser nulo si solo es texto
    public string TextoRespuesta { get; set; }
   }



}
