using Entidades.DTOs;

namespace Logica.Services.Interfaces
{
    public interface IEncuestaServicio
    {
        Task<bool> CrearEncuestaAsync(EncuestaCrearDto dto, int IdUsuario);

        Task<bool> EliminarEncuestaAsync(int encuestaId);
    }
}
