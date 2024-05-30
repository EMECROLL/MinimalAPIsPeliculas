using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsPeliculas.Repositorios;

namespace MinimalAPIsPeliculas.DTOs
{
    public class CrearGeneroPeticionDTO
    {
        public IRepositorioGeneros repositorioGeneros { get; set; }
        public IOutputCacheStore outputCacheStore {  get; set; }
        public IMapper mapper { get; set; }
    }
}
