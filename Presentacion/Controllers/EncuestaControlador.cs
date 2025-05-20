
using Entidades.DTOs;
using Logica.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentacion.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class EncuestaControlador : ControllerBase
    {




        private readonly IEncuestaServicio _encuestaServicio;

        public EncuestaControlador(IEncuestaServicio encuestaServicio)
        {
            _encuestaServicio = encuestaServicio;
        }




        [Authorize]
        [HttpGet("perfil")]
        public IActionResult ObtenerPerfil()
        {
            return Ok(new { mensaje = "Perfil de usuario" });
        }



        [Authorize(Roles = "Administrador")]
        [HttpPost("crear-encuesta")]
        public async Task<IActionResult> CrearEncuesta([FromBody] EncuestaCrearDto dto)
        {
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var resultado = await _encuestaServicio.CrearEncuestaAsync(dto, idUsuario);



            if (resultado)
                return Ok(new { mensaje = "Encuesta creada correctamente" });

            return BadRequest(new { mensaje = "Ocurrió un error al crear la encuesta" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEncuesta(int id)
        {
            var eliminado = await _encuestaServicio.EliminarEncuestaAsync(id);

            if (!eliminado)
                return NotFound("Encuesta no encontrada o no se pudo eliminar.");

            return Ok("Encuesta eliminada correctamente.");
        }














    }
}
