using proyectoFinal.Models.Entidades;

namespace proyectoFinal.Services
{
    public interface IServicioUsuarios
    {
        Task<Usuarios> SaveUsuarios(Usuarios usuarios);

        Task<Usuarios> GetUsuarios(string Nombre);

        Task<Usuarios> GetUsuarios(string correo, string clave);

        
            // ... otras firmas de métodos ...

       Task<Usuarios> ObtenerUsuarioPorId(int usuarioId);
        

    }
}
