using Microsoft.AspNetCore.Mvc.Rendering;
using proyectoFinal.Models.Entidades;

namespace proyectoFinal.Services
{
    public interface IServicioLista
    {




        Task<IEnumerable<SelectListItem>>
         GetListaUsuario();

      


      
        Task<IEnumerable<SelectListItem>>
  GetListaCategoria();


        

  

        Task<IEnumerable<SelectListItem>>
  GetListaFactura();

        Task<IEnumerable<SelectListItem>>
  GetListaCliente();

        Task<IEnumerable<SelectListItem>>
  GetListaProducto();


        Task<Usuarios> BuscarUsuarioPorCedula(string cedula);

        Task<Cliente> BuscarClientePorCedula(string cedula);

        Task<Producto> BuscarProductoPorCodigo(string codigopro);

    }
}
