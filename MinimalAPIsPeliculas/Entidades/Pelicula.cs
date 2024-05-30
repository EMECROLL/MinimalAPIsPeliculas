namespace MinimalAPIsPeliculas.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!; // Perdonar el nulo, es decir no deberia de serlo
        public bool EnCines { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string? Poster { get; set; } // string? para decir que SI PUEDE ser nulo
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>(); // Para obtener una lista de sus comentarios
        public List<GeneroPelicula> GenerosPeliculas { get; set; } = new List<GeneroPelicula>();
        public List<ActorPelicula> ActoresPeliculas { get; set; } = new List<ActorPelicula>();
    }
}
