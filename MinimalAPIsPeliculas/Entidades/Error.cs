namespace MinimalAPIsPeliculas.Entidades
{
    public class Error
    {
        public Guid Id { get; set; } // String aleatorio universal
        public string MensajeDeError { get; set; } = null!;
        public string? StackTrace { get; set; }
        public DateTime Fecha { get; set; } 
    }
}
