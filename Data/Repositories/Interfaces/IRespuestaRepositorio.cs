
using Entidades.Models;

namespace Data.Repositories.Interfaces
{
    public interface IRespuestaRepositorio
    {

        Task<bool> GuardarRespuestasUsuarioAsync(List<Respuesta> respuesta);

        Task<bool> EncuestaRespondida(int encuestaId);

        Task<List<Encuesta>> ObtenerEncuestasConRespuestasAsync();

        Task<bool> UsuarioYaRespondioEncuestaAsync(int usuarioId, int encuestaId);




    }
}
