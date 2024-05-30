using AutoMapper;
using MinimalAPIsPeliculas.Repositorios;

namespace MinimalAPIsPeliculas.DTOs
{
    public class ObtenerGeneroPorIdPeticionDTO
    {
        public int id {  get; set; } 
        public IRepositorioGeneros repositorioGeneros { get; set; }
        public IMapper mapper { get; set; }
    }
}
