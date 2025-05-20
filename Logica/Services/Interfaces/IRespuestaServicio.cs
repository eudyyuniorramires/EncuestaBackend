using Entidades.DTOs;

namespace Logica.Services.Interfaces
{
    public interface IRespuestaServicio
    {
        Task<bool> GuardarRespuestasUsuarioAsync(EncuestaResueltaDto dto);





    }
}
