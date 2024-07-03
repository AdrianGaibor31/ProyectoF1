using proyectoFinal.Models.Entidades;

namespace proyectoFinal.Services
{
    public interface IServicioProducto
    {

        Task<Producto> SaveProducto(Producto producto);

       
    }
}
