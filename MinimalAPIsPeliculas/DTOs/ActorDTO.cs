namespace MinimalAPIsPeliculas.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }

        // Esto se deja como string ya que es de lectura 
        public string? Foto { get; set; }
    }
}
