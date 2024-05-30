namespace MinimalAPIsPeliculas.DTOs
{
    public class CrearPeliculaDTO
    {
        public string Titulo { get; set; } = null!; // Perdonar el nulo, es decir no deberia de serlo
        public bool EnCines { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public IFormFile? Poster { get; set; } // IFormFile? para decir que SI PUEDE ser nulo
    }
}
