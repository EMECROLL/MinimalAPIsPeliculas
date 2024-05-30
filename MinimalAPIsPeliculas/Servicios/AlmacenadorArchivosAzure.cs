
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MinimalAPIsPeliculas.Servicios
{
    public class AlmacenadorArchivosAzure : IAlmacenadorArchivos
    {
        private string connectionString;

        // IConfiguration me ayuda a obtener información de los proveedores de servicio (appsettings.json)
        public AlmacenadorArchivosAzure(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage")!;
        }
        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync(); // Si el contenedor/carpeta no existe, la creara, si ya existe, entonces no hara nada
            
            // Tener acceso al archivo
            cliente.SetAccessPolicy(PublicAccessType.Blob);

            // Tener la extension del archivo
            var extension = Path.GetExtension(archivo.FileName);
            // Crear nombre aleatorio para no tener errores de duplicidad
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(nombreArchivo); // Blob (Binary Large Object) se refiere a un servicio de almacenamiento en la nube proporcionado por Azure

            // Configurar el Content-Type ya que si es una imagen para que se pueda visualizar en el navegador
            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = archivo.ContentType;

            // Subir el archivo
            await blob.UploadAsync(archivo.OpenReadStream(), blobHttpHeaders);

            // Regreso la URL
            return blob.Uri.ToString();
        }

        public async Task Borrar(string? ruta, string contenedor)
        {
            // Si la ruta es nula quiere decir que no existe
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }

            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync(); // Si el contenedor/carpeta no existe, la creara, si ya existe, entonces no hara nada

            // Buscar el archivo
            var nombreArchivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(nombreArchivo);

            // Se elimina
            await blob.DeleteIfExistsAsync();

        }
    }
}
