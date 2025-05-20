
using AutoMapper;
using Data.Context;
using Data.Repositories.Interfaces;
using EncuestaBackend.Utils.Fabrica;
using Entidades.DTOs;
using Entidades.Models;
using Logica.Services.Interfaces;


namespace Logica.Services.Implementaciones
{
    public class EncuestaServicio : IEncuestaServicio
    {
        private readonly IEncuestaRepositorio _encuestaRepositorio;
        private readonly IFabricaPregunta _fabricaPregunta;
        private readonly IMapper _mapper;
        private readonly EncuestaDbContext _context;  // <-- Agregado

        public EncuestaServicio(
            IEncuestaRepositorio encuestaRepositorio,
            IFabricaPregunta fabricaPregunta,
            IMapper mapper,
            EncuestaDbContext context)   // <-- Agregado
        {
            _encuestaRepositorio = encuestaRepositorio;
            _fabricaPregunta = fabricaPregunta;
            _mapper = mapper;
            _context = context;  // <-- Agregado
        }

        public async Task<bool> CrearEncuestaAsync(EncuestaCrearDto dto, int IdUsuario)
        {
            var encuesta = new Encuesta
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                CreadorId = IdUsuario,
                EsPrivada = dto.EsPrivada,
                FechaExpiracion = dto.FechaExpiracion,
                Preguntas = dto.Preguntas?
                    .Select(p => _fabricaPregunta.CrearPregunta(p))
                    .ToList()
            };

            return await _encuestaRepositorio.GuardarEncuestaAsync(encuesta);
        }

        public async Task<bool> EliminarEncuestaAsync(int encuestaId)
        {
            var encuesta = await _context.Encuestas.FindAsync(encuestaId);
            if (encuesta == null)
                return false;

            _context.Encuestas.Remove(encuesta);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
