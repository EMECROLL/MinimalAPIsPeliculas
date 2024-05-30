using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MinimalAPIsPeliculas.Swagger
{
    public class FiltroAutorizacion : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Si el endpoint NO tiene .RequireAuthorization();
            if (!context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().Any())
            {
                return;
            }

            // Si el endpoint tiene .RequireAuthorization(); le coloca el candado al endpoint
            operation.Security = new List<OpenApiSecurityRequirement>{
                new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            }, new string[]{}
                        }
                    }
            };
        }
    }
}
