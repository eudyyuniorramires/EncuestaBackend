using Data.Context;
using Data.Repositories.Interfaces;
using Entidades.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Repositories.Implementaciones
{
    public class RespuestaRepositorio : IRespuestaRepositorio
    {
        private readonly EncuestaDbContext _context;

        public RespuestaRepositorio(EncuestaDbContext context)
        {
            _context = context;
        }

        public Task<bool> EncuestaRespondida(int encuestaId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GuardarRespuestasUsuarioAsync(List<Respuesta> respuestas)
        {
            _context.Respuestas.AddRange(respuestas);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<List<Encuesta>> ObtenerEncuestasConRespuestasAsync()
        {
            throw new NotImplementedException();
        }


        public async Task<bool> UsuarioYaRespondioEncuestaAsync(int usuarioId, int encuestaId)
        {
            return await _context.Respuestas
                .Include(r => r.Pregunta) // Importante para poder filtrar por EncuestaId
                .AnyAsync(r => r.UsuarioId == usuarioId && r.Pregunta.EncuestaId == encuestaId);
        }





    }
}
