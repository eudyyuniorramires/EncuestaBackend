using Entidades.Models;

namespace Data.Repositories.Interfaces
{
    public interface IEncuestaRepositorio
    {
        Task<bool> GuardarEncuestaAsync(Encuesta encuesta);
    }
}
