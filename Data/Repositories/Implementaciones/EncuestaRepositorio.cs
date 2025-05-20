using Data.Context;
using Data.Repositories.Interfaces;
using Entidades.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementaciones
{
    public class EncuestaRepositorio : IEncuestaRepositorio
    {
        private readonly EncuestaDbContext _context;

        public EncuestaRepositorio(EncuestaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> GuardarEncuestaAsync(Encuesta encuesta)
        {
            await _context.Encuestas.AddAsync(encuesta);
            var result = await _context.SaveChangesAsync();
            return result > 0; // retorna true si al menos una fila fue afectada
        }

        public async Task<List<Encuesta>> ObtenerEncuestasConRespuestasAsync()
        {
            return await _context.Encuestas
                .Include(e => e.Descripcion)  // Incluye las respuestas de la encuesta
                .ToListAsync();  // Obtiene todas las encuestas con sus respuestas
        }
    }
}
