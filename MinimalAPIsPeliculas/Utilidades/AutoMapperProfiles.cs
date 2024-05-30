using AutoMapper;
using MinimalAPIsPeliculas.DTOs;
using MinimalAPIsPeliculas.Entidades;

namespace MinimalAPIsPeliculas.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Mapeos

            // Se utiliza en los endpoints donde necesitas convertir los datos de entrada del cliente a una entidad que se puede almacenar en la base de datos
            CreateMap<CrearGeneroDTO, Genero>();
            // Se utiliza en los endpoints donde necesitas convertir una entidad de la base de datos a un DTO que se envía al cliente
            CreateMap<Genero, GeneroDTO>();

            // Como tenemos una diferencia ya que en uno la Foto esta como string y en otro esta como IFormFile se lo tenemos que decir a AutoMapper
            CreateMap<CrearActorDTO, Actor>().ForMember(x => x.Foto, opciones => opciones.Ignore());
            CreateMap<Actor, ActorDTO>();

            CreateMap<CrearPeliculaDTO, Pelicula>().ForMember(x => x.Poster, opciones => opciones.Ignore());
            CreateMap<PeliculaDTO, Pelicula>();
            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember(x => x.Generos, entidad =>
                    entidad.MapFrom(p =>
                        p.GenerosPeliculas.Select(gp =>
                            new Genero { Id = gp.GeneroId, Nombre = gp.Genero.Nombre })))
                .ForMember(x => x.Actores, entidad =>
                    entidad.MapFrom(p =>
                        p.ActoresPeliculas.Select(ap =>
                            new ActorPeliculaDTO { Id = ap.ActorId, Nombre = ap.Actor.Nombre, Personaje = ap.Personaje })));

            CreateMap<CrearComentarioDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();

            CreateMap<AsignarActorPeliculaDTO, ActorPelicula>();

        }
    }
}
