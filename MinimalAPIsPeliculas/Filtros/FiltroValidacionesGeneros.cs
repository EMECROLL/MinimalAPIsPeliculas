
using FluentValidation;
using MinimalAPIsPeliculas.DTOs;

namespace MinimalAPIsPeliculas.Filtros
{
    public class FiltroValidacionesGeneros : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validador = context.HttpContext.RequestServices.GetService<IValidator<CrearGeneroDTO>>();

            if (validador is null)
            {
                return await next(context);
            }

            var insumoAValidar = context.Arguments.OfType<CrearGeneroDTO>().FirstOrDefault();

            if (insumoAValidar is null)
            {
                return TypedResults.Problem("No se encontro la entidad a validar");
            }

            var resultadoValidacion = await validador.ValidateAsync(insumoAValidar);

            // Si la validacion no es valida
            if (!resultadoValidacion.IsValid)
            {
                // Nos regresa los distintos errores de validación que encuentre
                return TypedResults.ValidationProblem(resultadoValidacion.ToDictionary());
            }

            return await next(context);
        }
    }
}
