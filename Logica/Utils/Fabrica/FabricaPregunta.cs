using Entidades.DTOs;
using Entidades.Models;
using Entidades.Models.Enumeraciones;

namespace EncuestaBackend.Utils.Fabrica
{
    public class FabricaPregunta : IFabricaPregunta
    {
        public Pregunta CrearPregunta(PreguntaDto dto)
        {
            // Aquí estamos asegurándonos de que dto.Tipo sea un string
            var tipo = Enum.Parse<TipoPregunta>(dto.Tipo, true);  // El segundo parámetro 'true' ignora mayúsculas/minúsculas

            var pregunta = new Pregunta
            {
                Texto = dto.Texto,
                Tipo = tipo,
                Opciones = tipo == TipoPregunta.OpcionMultiple
                    ? dto.Opciones?.Select(o => new Opcion { Texto = o }).ToList()
                    : null
            };

            return pregunta;
        }
    }


}
