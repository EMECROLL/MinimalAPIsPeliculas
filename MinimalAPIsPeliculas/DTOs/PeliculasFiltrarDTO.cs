using MinimalAPIsPeliculas.Utilidades;

namespace MinimalAPIsPeliculas.DTOs
{
    public class PeliculasFiltrarDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        public PaginacionDTO PaginacionDTO
        {
            get
            {
                return new PaginacionDTO()
                {
                    Pagina = Pagina,
                    RecordsPorPagina = RecordsPorPagina,
                };
            }
        }

        public string? Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool EnCines { get; set; }
        public bool ProximosEstrenos { get; set; }
        public string? CampoOrdenar { get; set; }
        public bool OrdenAscendente { get; set; } = true;

        public static ValueTask<PeliculasFiltrarDTO> BindAsync(HttpContext context)
        {
            var pagina = context.ExtraerValorDefecto(nameof(Pagina), 1);
            var recordsPorPagina = context.ExtraerValorDefecto(nameof(RecordsPorPagina), 10);
            var generoId = context.ExtraerValorDefecto(nameof(GeneroId), 0);

            var titulo = context.ExtraerValorDefecto(nameof(Titulo), string.Empty);
            var enCines = context.ExtraerValorDefecto(nameof(EnCines), false);
            var proximosEstrenos = context.ExtraerValorDefecto(nameof(ProximosEstrenos), false);
            var campoOrdernar = context.ExtraerValorDefecto(nameof(CampoOrdenar), string.Empty);
            var ordenAscendente = context.ExtraerValorDefecto(nameof(OrdenAscendente), true);

            var resultado = new PeliculasFiltrarDTO
            {
                Pagina = pagina,
                RecordsPorPagina = recordsPorPagina,
                Titulo = titulo,
                GeneroId = generoId,
                EnCines = enCines,
                ProximosEstrenos = proximosEstrenos,
                CampoOrdenar = campoOrdernar,
                OrdenAscendente = ordenAscendente,
            };

            return ValueTask.FromResult(resultado);
        }
    }
}
