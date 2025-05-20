using EncuestaBackend.Services.Interfaces;
using Entidades.DTOs;
using Logica.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RespuestaController : ControllerBase
    {
        private readonly IRespuestaServicio _respuestaServicio;

        public RespuestaController(IRespuestaServicio respuestaServicio)
        {
            _respuestaServicio = respuestaServicio;
        }

        [HttpPost("responder")]
        public async Task<IActionResult> ResponderEncuesta([FromBody] EncuestaResueltaDto dto)
        {
            var resultado = await _respuestaServicio.GuardarRespuestasUsuarioAsync(dto);

            if (!resultado)
                return BadRequest("El usuario ya respondió esta encuesta.");

            return Ok("Respuestas guardadas correctamente");
        }



    }
}
