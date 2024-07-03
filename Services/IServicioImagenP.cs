namespace proyectoFinal.Services
{
    public interface IServicioImagenP
    {
        Task<string> SubirImagen(Stream archivo, string nombre);
    }
}
