using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIsPeliculas.Utilidades
{
    public static class HttpContextExtensionsUtilidades
    {
        public static T ExtraerValorDefecto<T>(this HttpContext context, string nombreDelCampo,
            T valorPorDefecto) where T : IParsable<T>
        {
            var valor = context.Request.Query[nombreDelCampo];

            if (valor.IsNullOrEmpty())
            {
                return valorPorDefecto;
            }

            return T.Parse(valor!, null);
        }
    }
}
