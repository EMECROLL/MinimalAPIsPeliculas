using FluentValidation;
using MinimalAPIsPeliculas.DTOs;

namespace MinimalAPIsPeliculas.Validaciones
{
    public class EditarClaimDTOValidador : AbstractValidator<EditarClaimDTO>
    {
        public EditarClaimDTOValidador()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
                                 .MaximumLength(256).WithMessage(Utilidades.MaxLengthMensaje)
                                 .EmailAddress().WithMessage(Utilidades.EmailMensaje);
        }
    }
}
