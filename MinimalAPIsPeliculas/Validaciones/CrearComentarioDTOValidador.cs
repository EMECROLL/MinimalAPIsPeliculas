using FluentValidation;
using MinimalAPIsPeliculas.DTOs;

namespace MinimalAPIsPeliculas.Validaciones
{
    public class CrearComentarioDTOValidador : AbstractValidator<CrearComentarioDTO>
    {
        public CrearComentarioDTOValidador()
        {
            RuleFor(x => x.Cuerpo).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje);
        }
    }
}
