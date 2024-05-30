using FluentValidation;
using MinimalAPIsPeliculas.DTOs;

namespace MinimalAPIsPeliculas.Validaciones
{
    public class CredencialesUsuarioDTOValidador : AbstractValidator<CredencialesUsuarioDTO>
    {
        public CredencialesUsuarioDTOValidador()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
                                 .MaximumLength(256).WithMessage(Utilidades.MaxLengthMensaje)
                                 .EmailAddress().WithMessage(Utilidades.EmailMensaje);

            RuleFor(x => x.Password).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje);
        }
    }
}
