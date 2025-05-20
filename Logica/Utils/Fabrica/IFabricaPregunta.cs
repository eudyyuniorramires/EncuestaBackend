using Entidades.DTOs;
using Entidades.Models;

namespace EncuestaBackend.Utils.Fabrica
{
    public interface IFabricaPregunta
    {
        Pregunta CrearPregunta(PreguntaDto dto);
    }
}
