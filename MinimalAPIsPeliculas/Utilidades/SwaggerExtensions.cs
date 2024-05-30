using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MinimalAPIsPeliculas.Utilidades
{
    public static class SwaggerExtensions
    {
        public static TBuilder AgregarParametrosPaginacionOpenAPI<TBuilder>(this TBuilder builder)
            where TBuilder : IEndpointConventionBuilder
        {
            return builder.WithOpenApi(opciones =>
            {
                // Modificar la metadata que se va a emitir y Swagger sepa que mostrar en la interfaz
                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "pagina", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(1) // Valor por defecto
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "recordsPorPagina", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(10) // Valor por defecto
                    }
                });

                return opciones;
            });
        }

        public static TBuilder AgregarParametrosPeliculasOpenAPI<TBuilder> (this TBuilder builder)
            where TBuilder : IEndpointConventionBuilder
        {
            return builder.WithOpenApi(opciones =>
            {
                // Modificar la metadata que se va a emitir y Swagger sepa que mostrar en la interfaz
                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "pagina", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(1) // Valor por defecto
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "recordsPorPagina", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(10) // Valor por defecto
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "titulo", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "string",
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "enCines", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "boolean",
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "proximosEstrenos", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "boolean",
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "generoId", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "integer",
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "campoOrdenar", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "string",
                        // Opciones de filtrado
                        Enum = new List<IOpenApiAny>()
                        {
                            new OpenApiString("Titulo"),
                            new OpenApiString("FechaLanzamiento")
                        }
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "ordenAscendente", // Nombre del parametro
                    In = ParameterLocation.Query, // De donde viene el parametro
                    Schema = new OpenApiSchema // Tipo de dato del parametro
                    {
                        Type = "boolean",
                        Default = new OpenApiBoolean(true) // Valor por defecto
                    }
                });

                return opciones;
            });
        }
    }
}
