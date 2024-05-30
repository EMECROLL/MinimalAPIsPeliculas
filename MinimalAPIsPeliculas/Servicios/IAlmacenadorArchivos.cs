namespace MinimalAPIsPeliculas.Servicios
{
    public interface IAlmacenadorArchivos
    {
        Task Borrar(string? ruta, string contenedor); // contenedor = carpeta
        Task <string> Almacenar (string contenedor, IFormFile archivo); // archivo = imagen, pdf, excel, etc...

        // Esto es una implementación por defecto de editar, ya que al final editar es borrar lo que tenemos y crear uno nuevo
        async Task<string> Editar (string? ruta, string contenedor, IFormFile archivo)
        {
            await Borrar(ruta, contenedor);
            return await Almacenar(contenedor, archivo);
        }
    }
}
