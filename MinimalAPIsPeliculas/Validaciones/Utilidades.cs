namespace MinimalAPIsPeliculas.Validaciones
{
    public static class Utilidades
    {
        // Mensajes
        public static string CampoRequeridoMensaje = "El campo {PropertyName} es requerido.";
        public static string MaxLengthMensaje = "El campo {PropertyName} debe tener menos de {MaxLength} caracteres.";
        public static string PrimeraLetraMayusculaMensaje = "El campo {PropertyName} debe comenzar con mayúsculas.";
        public static string EmailMensaje = "El campo {PropertyName} debe ser un email válido.";

        public static string GreaterThanOrEqualToMensaje (DateTime fechaMinima)
        {
            return "El campo {PropertyName} debe ser posterios a " + fechaMinima.ToString("yyyy-MM-dd");
        }

        // Validaciones personalizadas
        public static bool PrimeraLetraEnMayusculas(string valor)
        {
            // Si el string es vacio retorno verdadero y me salgo ya que no puedo repetir validaciones para eso esta la validación de .NotEmpty()
            if (string.IsNullOrWhiteSpace(valor))
            {
                return true;
            }

            var primeraLetra = valor[0].ToString();

            return primeraLetra == primeraLetra.ToUpper();
        }
    }
}
