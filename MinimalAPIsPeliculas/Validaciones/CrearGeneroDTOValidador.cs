using FluentValidation;
using MinimalAPIsPeliculas.DTOs;
using MinimalAPIsPeliculas.Repositorios;

namespace MinimalAPIsPeliculas.Validaciones
{
    public class CrearGeneroDTOValidador : AbstractValidator<CrearGeneroDTO>
    {
        public CrearGeneroDTOValidador(IRepositorioGeneros repositorioGeneros, IHttpContextAccessor httpContextAccessor)
        {
            // Obtener el Id de la URL
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            // Si el valorDeRutaId es un string entramos
            if (valorDeRutaId is string valorString)
            {
                // Tratamos de convertirlo a int
                int.TryParse(valorString, out id );
            }

            // Reglas
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
                                  .MaximumLength(50).WithMessage(Utilidades.MaxLengthMensaje)
                                  .Must(Utilidades.PrimeraLetraEnMayusculas).WithMessage(Utilidades.PrimeraLetraMayusculaMensaje)
                                  .MustAsync(async (nombre, _) =>
                                  {
                                      var existe = await repositorioGeneros.Existe(id, nombre);
                                      return !existe;
                                  }).WithMessage(g => $"Ya existe un género con el nombre {g.Nombre}");
        }
    }
}
