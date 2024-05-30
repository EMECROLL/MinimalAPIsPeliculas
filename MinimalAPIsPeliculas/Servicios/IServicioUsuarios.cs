using Microsoft.AspNetCore.Identity;

namespace MinimalAPIsPeliculas.Servicios
{
    public interface IServicioUsuarios
    {
        Task<IdentityUser?> ObtenerUsuario();
    }
}