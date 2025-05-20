using Data.Context;
using Entidades.DTOs;
using Entidades.Models;
using Entidades.Models.Enumeraciones;
using Logica.Services.Interfaces;
using Logica.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logica.Services.Implementaciones
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly EncuestaDbContext _contexto;
        private readonly IJwtHelper _jwtHelper;
        private readonly PasswordHasher<Usuario> _hasher;

        public UsuarioServicio(EncuestaDbContext contexto, IJwtHelper jwtHelper)
        {
            _contexto = contexto;
            _jwtHelper = jwtHelper;
            _hasher = new PasswordHasher<Usuario>();
        }

        public async Task<string> RegistrarUsuarioAsync(UsuarioRegistroDto dto)
        {
            if (await _contexto.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
                return "Ya existe un usuario con ese correo.";

            var usuario = new Usuario
            {
                Correo = dto.Correo,
                Rol = Enum.TryParse<Rol>(dto.Rol, out var rol) ? rol : Rol.Usuario
            };

            usuario.PasswordHash = _hasher.HashPassword(usuario, dto.Password);

            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();

            return "Usuario registrado correctamente.";
        }

        public async Task<string> IniciarSesionAsync(UsuarioLoginDTo dto)
        {
            var usuario = await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == dto.Correo);

            if (usuario == null)
                return null;

            var resultado = _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Password);
            if (resultado == PasswordVerificationResult.Failed)
                return null;

            return _jwtHelper.GenerarToken(usuario);
        }
    }
}
