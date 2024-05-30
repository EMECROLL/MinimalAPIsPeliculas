using Microsoft.IdentityModel.Tokens;
using MinimalAPIsPeliculas.Utilidades;

namespace MinimalAPIsPeliculas.DTOs
{
    public class PaginacionDTO
    {
        private const int paginaValorInicial = 1;
        private const int recordsPorPaginaValorInicial = 10;
        public int Pagina { get; set; } = paginaValorInicial;
        private int recordsPorPagina { get; set; } = recordsPorPaginaValorInicial; // Registros por página
        private readonly int cantidadMaximaRecordsPorPagina = 50; // Limite de registros por página

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                // Si el value = 1000 entonces esto lo que hara es cambiar el valor a 50 (Que es mi limite),
                // Pero si el value es menor a 50 entonces lo deja con ese value
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value;
            }
        }

        public static ValueTask<PaginacionDTO> BindAsync(HttpContext context)
        {
            var pagina = context.ExtraerValorDefecto(nameof(Pagina), paginaValorInicial);
            var recordsPorPagina = context.ExtraerValorDefecto(nameof(RecordsPorPagina), recordsPorPaginaValorInicial);

            var resultado = new PaginacionDTO
            {
                Pagina = pagina,
                RecordsPorPagina = recordsPorPagina
            };

            return ValueTask.FromResult(resultado);
        }
    }
}
