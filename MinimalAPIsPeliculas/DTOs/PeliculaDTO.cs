namespace MinimalAPIsPeliculas.DTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!; // Perdonar el nulo, es decir no deberia de serlo
        public bool EnCines { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string? Poster { get; set; } // string? para decir que SI PUEDE ser nulo
        public List<ComentarioDTO> Comentarios { get; set; } = new List<ComentarioDTO>();
        public List<GeneroDTO> Generos { get; set; } = new List<GeneroDTO>();
        public List<ActorPeliculaDTO> Actores { get; set; } = new List<ActorPeliculaDTO>();
    }
}
