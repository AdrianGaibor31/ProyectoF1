using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace proyectoFinal.Models.Entidades
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProducto { get; set; }



        public string CodigoProducto { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Precio { get; set; }

        [Required]
        public int CantidadEnStock { get; set; }


        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        public bool Activo { get; set; } = true;

        public string? URLFoto { get; set; }

        /// Clave Foranea 

        [ForeignKey("Categoriaid")]
        public Categoria Categorias { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar una Categoria .")]

        public int Categoriaid { get; set; }


        [NotMapped]

        public IEnumerable<SelectListItem> Categoria  { get; set; }


        


    }
}
