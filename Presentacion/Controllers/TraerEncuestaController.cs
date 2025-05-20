using Data.Context;
using Entidades.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraerEncuestaController : ControllerBase
    {

        private readonly EncuestaDbContext _context;
        public TraerEncuestaController(EncuestaDbContext context)
        {
            _context = context;
        }

        [HttpGet("resumen")]
        public async Task<ActionResult> ObtenerResumenEncuestas()
        {
            var resumenEncuestas = await _context.Encuestas
                .Select(e => new
                {
                    e.Id,
                    e.Titulo,
                    e.Descripcion,
                    e.FechaExpiracion,
                    e.EsPrivada
                })
                .ToListAsync();

            return Ok(resumenEncuestas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerEncuestaPorId(int id)
        {
            var encuesta = await _context.Encuestas
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    e.Id,
                    e.Titulo,
                    e.Descripcion,
                    e.EsPrivada,
                    e.FechaExpiracion,
                    Preguntas = e.Preguntas.Select(p => new
                    {
                        p.Id,
                        p.Texto,
                        p.Tipo,
                        Opciones = p.Opciones.Select(o => o.Texto).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (encuesta == null)
            {
                return NotFound();
            }

            return Ok(encuesta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEncuesta(int id, [FromBody] EncuestaUpdateDTO encuestaDto)
        {
            if (id != encuestaDto.Id)
            {
                return BadRequest("El ID de la URL y el del cuerpo no coinciden.");
            }

            var encuestaExistente = await _context.Encuestas.FindAsync(id);
            if (encuestaExistente == null)
            {
                return NotFound("Encuesta no encontrada.");
            }

            encuestaExistente.Titulo = encuestaDto.Titulo;
            encuestaExistente.Descripcion = encuestaDto.Descripcion;
            encuestaExistente.EsPrivada = encuestaDto.EsPrivada;
            encuestaExistente.FechaExpiracion = encuestaDto.FechaExpiracion;

            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpGet("{id}/preguntas")]
        public async Task<ActionResult> ObtenerPreguntasPorEncuesta(int id)
        {
            var encuesta = await _context.Encuestas
                .Include(e => e.Preguntas)
                    .ThenInclude(p => p.Opciones) // Incluye las opciones de cada pregunta
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encuesta == null)
            {
                return NotFound(new { mensaje = "Encuesta no encontrada" });
            }

            var preguntas = encuesta.Preguntas.Select(p => new
            {
                p.Id,
                p.Texto,
                p.Tipo,


                Opciones = p.Opciones.Select(o => new
                {
                    o.Id,
                    o.Texto
                })
            });

            return Ok(preguntas);
        }


        [HttpGet("estadisticas")]
        public async Task<ActionResult> ObtenerEstadisticas()
        {
            var totalEncuestas = await _context.Encuestas
                .Select(r => r.Id)
                .Distinct()
                .CountAsync();

            var totalUsuarios = await _context.Usuarios.CountAsync();

            var totalRespuesta = await _context.Respuestas.CountAsync();



            return Ok(new
            {
                totalEncuestas,
                totalUsuarios,
                totalRespuesta
            });
        }

        [HttpGet("encuesta/{encuestaId}/estadisticas")]
        public async Task<ActionResult> ObtenerEstadisticasPorEncuesta(int encuestaId)
        {
            var preguntas = await _context.Preguntas
                .Where(p => p.EncuestaId == encuestaId)
                .ToListAsync();

            if (!preguntas.Any())
                return NotFound(new { mensaje = "No se encontró la encuesta o no tiene preguntas." });

            var estadisticasEncuesta = new List<object>();

            foreach (var pregunta in preguntas)
            {
                if (pregunta.Tipo == 0) // Pregunta de texto / selección
                {
                    var respuestas = await _context.Respuestas
                        .Where(r => r.PreguntaId == pregunta.Id)
                        .Select(r => r.TextoRespuesta)
                        .ToListAsync();

                    var ranking = respuestas
                        .GroupBy(r => r)
                        .Select(g => new
                        {
                            Opcion = g.Key,
                            Cantidad = g.Count()
                        })
                        .OrderByDescending(g => g.Cantidad)
                        .ToList();

                    estadisticasEncuesta.Add(new
                    {
                        PreguntaId = pregunta.Id,
                        TextoPregunta = pregunta.Texto,
                        TipoPregunta = pregunta.Tipo,
                        TotalRespuestas = respuestas.Count,
                        Ranking = ranking
                    });
                }
                else if (pregunta.Tipo is (Entidades.Models.Enumeraciones.TipoPregunta)1 or (Entidades.Models.Enumeraciones.TipoPregunta)2) // Pregunta numérica
                {
                    var respuestas = await _context.Respuestas
                        .Where(r => r.PreguntaId == pregunta.Id)
                        .Select(r => (double)r.NumeroRespuesta)
                        .ToListAsync();

                    if (respuestas.Any())
                    {
                        var media = respuestas.Average();

                        var moda = respuestas
                            .GroupBy(v => v)
                            .OrderByDescending(g => g.Count())
                            .Select(g => g.Key)
                            .FirstOrDefault();

                        var ordenadas = respuestas.OrderBy(v => v).ToList();
                        double mediana;
                        int count = ordenadas.Count;
                        if (count % 2 == 0)
                            mediana = (ordenadas[count / 2 - 1] + ordenadas[count / 2]) / 2.0;
                        else
                            mediana = ordenadas[count / 2];

                        estadisticasEncuesta.Add(new
                        {
                            PreguntaId = pregunta.Id,
                            TextoPregunta = pregunta.Texto,
                            TipoPregunta = pregunta.Tipo,
                            TotalRespuestas = respuestas.Count,
                            Media = media,
                            Moda = moda,
                            Mediana = mediana
                        });
                    }
                    else
                    {
                        estadisticasEncuesta.Add(new
                        {
                            PreguntaId = pregunta.Id,
                            TextoPregunta = pregunta.Texto,
                            TipoPregunta = pregunta.Tipo,
                            Mensaje = "No hay respuestas para esta pregunta."
                        });
                    }
                }
                else
                {
                    estadisticasEncuesta.Add(new
                    {
                        PreguntaId = pregunta.Id,
                        TextoPregunta = pregunta.Texto,
                        TipoPregunta = pregunta.Tipo,
                        Mensaje = "Tipo de pregunta no soportado para estadísticas."
                    });
                }
            }

            return Ok(new
            {
                EncuestaId = encuestaId,
                TotalPreguntas = preguntas.Count,
                Estadisticas = estadisticasEncuesta
            });
        }








    }
}
