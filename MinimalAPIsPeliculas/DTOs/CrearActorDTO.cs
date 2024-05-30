namespace MinimalAPIsPeliculas.DTOs
{
    public class CrearActorDTO
    {
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }

        // El Form file en ASP.NET Core básicamente es una representación de un archivo.
        // Ese archivo puede ser lo que sea un PDF, una foto, un archivo de Excel, lo que sea.
        public IFormFile? Foto { get; set; }
    }
}
