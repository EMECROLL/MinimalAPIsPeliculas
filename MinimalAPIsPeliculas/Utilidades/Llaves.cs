using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIsPeliculas.Utilidades
{
    public static class Llaves
    {
        public const string IssuerPropio = "MinimalAPIsPeliculas";
        private const string SeccionLlaves = "Authentication:Schemes:Bearer:SigningKeys";
        private const string SeccionLlaves_Emisor = "Issuer";
        private const string SeccionLlaves_Valor = "Value";

        // Método auxiliar
        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration) => ObtenerLlave(configuration, IssuerPropio);

        // Método para obtener las llaves
        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration, string issuer)
        {
            var signingKey = configuration.GetSection(SeccionLlaves)
                                          .GetChildren().SingleOrDefault(llave => llave[SeccionLlaves_Emisor] == issuer); // Obtener llaves que el Issuer sea igual al que tenemos


            if (signingKey is not null && signingKey[SeccionLlaves_Valor] is string valorLlave)
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
            }
        }

        // Método para obtener todas las llaves
        public static IEnumerable<SecurityKey> ObtenerTodasLasLlaves(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(SeccionLlaves).GetChildren();

            foreach (var signingKey in signingKeys)
            {
                if (signingKey[SeccionLlaves_Valor] is string valorLlave)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
                }
            }
        }
    }
}
