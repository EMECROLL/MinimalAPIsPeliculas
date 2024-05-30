
namespace MinimalAPIsPeliculas.Servicios
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        // En el constructor inyecto los servicios que utilizare
        // IWebHostEnvironment para obtener la dirección de la carpeta de las imágenes
        // IHttpContextAccessor para tener acceso al contexto HTTP
        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            // Obtener la extension del archivo
            var extension = Path.GetExtension(archivo.FileName);
            // Crear nombre aleatorio para no tener errores de duplicidad - peligro de colisiones
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            // env.WebRootPath = Hace referencia a una carpeta por defecto de ASP.NET donde guardamos archivos (wwwroot)
            string folder = Path.Combine(env.WebRootPath, contenedor); // Combinamos estas carpetas

            // Si no existe la carpeta se creara
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // Definir la ruta del archivo
            string ruta = Path.Combine(folder, nombreArchivo);

            using (var ms = new MemoryStream())
            {
                await archivo.CopyToAsync(ms);
                var contenido = ms.ToArray();
                await File.WriteAllBytesAsync(ruta, contenido);
            }

            // Contruir la URL

            // URL con HTTP y dominio
            var url = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            // URL con el nombre del archivo
            var urlArchivo = Path.Combine(url, contenedor, nombreArchivo).Replace("\\","/"); // Remplazamos esto ya que las \\ se usan en sistemas de archivos pero la / es en URLs

            return urlArchivo;

        }

        public Task Borrar(string? ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return Task.CompletedTask;
            }

            var nombreArchivo = Path.GetFileName(ruta);
            // Construir el directorio del archivo
            var directorioArchivo = Path.Combine(env.WebRootPath, contenedor, nombreArchivo);

            // Si existe en el directorio lo eliminamos
            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.CompletedTask;
        }
    }
}
