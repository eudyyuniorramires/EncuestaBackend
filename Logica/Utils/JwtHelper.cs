using Entidades.Models;
using Microsoft.Extensions.Configuration;
using Entidades.Models.Enumeraciones;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Logica.Utils
{

    public interface IJwtHelper
    {
        string GenerarToken(Usuario usuario);

    }
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _config;


        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }

        public string GenerarToken(Usuario usuario)
        {
            var claves = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:ClaveSecreta"]));

            var credenciales = new SigningCredentials(claves, SecurityAlgorithms.HmacSha256);

            // Usa Enum.GetName para convertir el número al nombre del rol
            string nombreRol = Enum.GetName(typeof(Rol), usuario.Rol) ?? "Usuario"; // Valor por defecto si falla

            var claims = new[]
            {
                     new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                     new Claim(ClaimTypes.Email, usuario.Correo),
                     new Claim(ClaimTypes.Role, nombreRol) // Ejemplo: "Administrador"
    };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Emisor"],
                audience: _config["Jwt:Audiencia"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
