using proyectoFinal.Models.Entidades;

namespace proyectoFinal.Services
{
    public interface IServicioFactura
    {

        Task<List<Factura>> ObtenerAccionesPersonalesPorCliente(int clienteId);

    }
}
