using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Models;
using proyectoFinal.Models.Entidades;
using proyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proyectoFinal.Services
{
    public class ServicioLista : IServicioLista
    {
        private readonly SMedicoContext _context;

        public ServicioLista(SMedicoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }






        public async Task<IEnumerable<SelectListItem>> GetListaUsuario()
        {
            List<SelectListItem> list = await _context.Usuarios.Select(x => new SelectListItem
            {
                Text = x.Nombre,
                Value = $"{x.UsuarioID}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Usuario...]",
                Value = "0"
            });

            return list;
        }






        public async Task<Usuarios> BuscarUsuarioPorCedula(string cedula)
        {
            // Lógica para buscar el usuario en la base de datos
            // Puedes utilizar Entity Framework u otra tecnología de acceso a datos
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Cedula == cedula);

            return usuario;
        }




        public async Task<IEnumerable<SelectListItem>> GetListaCategoria()
        {

            List<SelectListItem> list = await _context.Categorias.Select(x => new SelectListItem
            {
                Text = x.Nombre,
                Value = $"{x.IdCategoria}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una CATEGORIA...]",
                Value = "0"
            });

            return list;
        }

        

        public async Task<IEnumerable<SelectListItem>> GetListaCliente()
        {

            List<SelectListItem> list = await _context.Clientes.Select(x => new SelectListItem
            {
                Text = x.Nombres,
                Value = $"{x.IdCliente}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un cliente...]",
                Value = "0"
            });

            return list;
        }


        public async Task<IEnumerable<SelectListItem>> GetListaProducto()
        {

            List<SelectListItem> list = await _context.Productos.Select(x => new SelectListItem
            {
                Text = x.Nombre,
                Value = $"{x.IdProducto}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un producto..]",
                Value = "0"
            });

            return list;
        }


        public async Task<IEnumerable<SelectListItem>> GetListaFactura()
        {

            List<SelectListItem> list = await _context.Facturas.Select(x => new SelectListItem
            {
                Text = x.NombreFactura,
                Value = $"{x.IdFactura}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una factura...]",
                Value = "0"
            });

            return list;
        }

        public async Task<Cliente> BuscarClientePorCedula(string cedula)
        {
            // Puedes utilizar Entity Framework u otra tecnología de acceso a datos
            var cliente = await _context.Clientes.FirstOrDefaultAsync(u => u.Cedula == cedula);

            return cliente;
        }

        public async Task<Producto> BuscarProductoPorCodigo(string codigopro)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.CodigoProducto == codigopro);

            return producto; // Devuelve el producto encontrado, no el código del producto
        }

    }


}


