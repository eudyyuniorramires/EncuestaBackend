using Data.Repositories.Interfaces;
using EncuestaBackend.Services.Interfaces;
using Entidades.DTOs;
using Entidades.Models;
using Logica.Services.Interfaces;

namespace Logica.Services.Implementaciones
{
    public class RespuestaServicio : IRespuestaServicio
    {
        private readonly IRespuestaRepositorio _respuestaRepositorio;

        public RespuestaServicio(IRespuestaRepositorio respuestaRepositorio)
        {
            _respuestaRepositorio = respuestaRepositorio;
        }

        public async Task<bool> GuardarRespuestasUsuarioAsync(EncuestaResueltaDto dto)
        {
            // Si el usuario es diferente de null, validar si ya respondió.
            if (dto.UsuarioId.HasValue)
            {
                bool yaRespondio = await _respuestaRepositorio.UsuarioYaRespondioEncuestaAsync(dto.UsuarioId.Value, dto.EncuestaId);
                if (yaRespondio)
                    return false;
            }

            // Guardar respuestas siempre, usuario anónimo o con ID.
            var respuestas = dto.Respuestas.Select(r => new Respuesta
            {
                PreguntaId = r.PreguntaId,
                UsuarioId = dto.UsuarioId,  // Puede ser null.
                TextoRespuesta = r.TextoRespuesta,
                NumeroRespuesta = r.NumeroRespuesta
            }).ToList();

            return await _respuestaRepositorio.GuardarRespuestasUsuarioAsync(respuestas);
        }


    }
}
