using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIsPeliculas.DTOs;
using MinimalAPIsPeliculas.Filtros;
using MinimalAPIsPeliculas.Servicios;
using MinimalAPIsPeliculas.Utilidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MinimalAPIsPeliculas.Endpoints
{
    public static class UsuariosEndpoints
    {
        public static RouteGroupBuilder MapUsuarios(this RouteGroupBuilder group)
        {
            group.MapPost("/registrar", Registrar).AddEndpointFilter<FiltroValidaciones<CredencialesUsuarioDTO>>();
            group.MapPost("/login", Login).AddEndpointFilter<FiltroValidaciones<CredencialesUsuarioDTO>>();
            group.MapPost("/haceradmin", HacerAdmin).AddEndpointFilter<FiltroValidaciones<EditarClaimDTO>>()
                                                    .RequireAuthorization("admin");
            group.MapPost("/removerAdmin", RemoverAdmin).AddEndpointFilter<FiltroValidaciones<EditarClaimDTO>>()
                                                        .RequireAuthorization("admin");
            group.MapGet("/renovarToken", RenovarToken).RequireAuthorization();

            return group;
        }

        // Registro
        static async Task<Results<Ok<RespuestaAutenticacionDTO>, BadRequest<IEnumerable<IdentityError>>>> Registrar(
            CredencialesUsuarioDTO credencialesUsuarioDTO,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuarioDTO.Email,
                Email = credencialesUsuarioDTO.Email,
            };

            var resultado = await userManager.CreateAsync(usuario, credencialesUsuarioDTO.Password);

            // Regresar respuesta con el token
            if (resultado.Succeeded)
            {
                var credencialesRespuesta = await ConstruirToken(credencialesUsuarioDTO, configuration, userManager);
                return TypedResults.Ok(credencialesRespuesta);
            } 
            else
            {
                return TypedResults.BadRequest(resultado.Errors);
            }
        }

        // Iniciar Sesion
        static async Task<Results<Ok<RespuestaAutenticacionDTO>, BadRequest<string>>> Login (
            CredencialesUsuarioDTO credencialesUsuarioDTO, [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.Email);

            if (usuario is null)
            {
                return TypedResults.BadRequest("Login incorrecto");
            }

            var resultado = await signInManager.CheckPasswordSignInAsync(usuario,
                credencialesUsuarioDTO.Password, lockoutOnFailure: false); // lockoutOnFailure si es true activa la funcionalidad de bloque de cuenta

            // Si el resultado fue exitoso
            if (resultado.Succeeded)
            {
                var respuestaAutenticacion = await ConstruirToken(credencialesUsuarioDTO, configuration, userManager);
                return TypedResults.Ok(respuestaAutenticacion);
            }
            else
            {
                return TypedResults.BadRequest("Login incorrecto");
            }
        }


        // Método para construir el Token
        private async static Task<RespuestaAutenticacionDTO> ConstruirToken(CredencialesUsuarioDTO credencialesUsuarioDTO, 
            IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var claims = new List<Claim>
            {
                // Solo colocamos lo necesario de informacion sobre el usuario
                new Claim("email", credencialesUsuarioDTO.Email)
                // new Claim(Type, Value)
            };

            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario!);

            claims.AddRange(claimsDB);

            var llave = Llaves.ObtenerLlave(configuration);
            var creds = new SigningCredentials(llave.First(), SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            // Armar Token
            var tokenDeSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new RespuestaAutenticacionDTO
            {
                Token = token,
                Expiracion = expiracion,
            };

        }

        // Hacer admin a un usuario
        static async Task<Results<NoContent, NotFound>> HacerAdmin(EditarClaimDTO editarClaimDTO,
            [FromServices] UserManager<IdentityUser> userManager)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);

            if (usuario is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.AddClaimAsync(usuario, new Claim("admin", "true"));
            return TypedResults.NoContent();
        }

        // Quitar admin a un usuario
        static async Task<Results<NoContent, NotFound>> RemoverAdmin(EditarClaimDTO editarClaimDTO,
            [FromServices] UserManager<IdentityUser> userManager)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);

            if (usuario is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.RemoveClaimAsync(usuario, new Claim("admin", "true"));
            return TypedResults.NoContent();
        }

        // Renovar el token
        public async static Task<Results<Ok<RespuestaAutenticacionDTO>, NotFound>> RenovarToken (
            IServicioUsuarios servicioUsuarios, IConfiguration configuration,
            [FromServices] UserManager<IdentityUser> userManager)
        {
            var usuario = await servicioUsuarios.ObtenerUsuario();

            if (usuario is null)
            {
                return TypedResults.NotFound();
            }

            var credencialesUsuarioDTO = new CredencialesUsuarioDTO { Email = usuario.Email! };

            var respuestaAutenticacionDTO = await ConstruirToken(credencialesUsuarioDTO, configuration, userManager);

            return TypedResults.Ok(respuestaAutenticacionDTO);
        }
    }
}
