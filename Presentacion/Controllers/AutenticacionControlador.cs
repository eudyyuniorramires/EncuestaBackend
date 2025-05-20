using Entidades.DTOs;
using Logica.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Controllers;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionControlador : ControllerBase
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public AutenticacionControlador(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDto dto)
        {
            var resultado = await _usuarioServicio.RegistrarUsuarioAsync(dto);
            return Ok(new { mensaje = resultado });
        }

        [HttpPost("login")]
        public async Task<IActionResult> IniciarSesion([FromBody] UsuarioLoginDTo dto)
        {
            var token = await _usuarioServicio.IniciarSesionAsync(dto);
            if (token == null)
                return Unauthorized();

            return Ok(new { token });
        }
    }
}
