using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace proyectoFinal.Models.Entidades
{
    public class Factura
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFactura { get; set; }

        [Required]
        public string NombreFactura { get; set; }


        public string NombreCliente { get; set; }

        public string ApellidoCliente { get; set; }

        public string CedulaCliente { get; set; }

        public string DireccionCliente { get; set; }


        public DateTime Fecha { get; set; }

       
        public double Total { get; set; }


        public double DescuentoTotal { get; set; }

        
        public double Iva { get; set; }

       
        public double SubTotal { get; set; }

        // Clave Foránea


        public Cliente Cliente { get; set; }
       

        [Range(1, int.MaxValue, ErrorMessage = "Debes de ingresar un usuario")]

        public int IdCliente { get; set; }




        [NotMapped]
        public List<DetalleFacturas> DetallesFactura { get; set; }
    }


    public class DetalleFacturas
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Descuento { get; set; }
        public double Total { get; set; }

        // Propiedades adicionales para mostrar el subtotal e IVA en cada detalle
        public double SubTotalDetalle { get; set; }
        public double IVADetalle { get; set; }

        public Producto Producto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes de ingresar un Producto")]
        public int IdProducto { get; set; }  // Define la clave foránea hacia Producto

        

    }


}

