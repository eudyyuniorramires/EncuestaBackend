
using Entidades.DTOs;

namespace Logica.Services.Interfaces
{
    public interface IUsuarioServicio
    {
        Task<string> RegistrarUsuarioAsync(UsuarioRegistroDto dto);
        Task<string> IniciarSesionAsync(UsuarioLoginDTo dto);
    }
}
