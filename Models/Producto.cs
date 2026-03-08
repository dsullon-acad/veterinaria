using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VeterinariaWeb.Models
{
    public class Producto
    {
        public int ID { get; set; }

        [DisplayName("Nombre del producto")]
        [MinLength(10, ErrorMessage = "La longitud mínima del nombre es de 10 caracteres.")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        [MinLength(10, ErrorMessage = "La longitud mínima para la descripción es de 10 caracteres.")]
        [Required(ErrorMessage = "Debe agregar una descripción.")]
        public string Descripcion { get; set; }

        [DisplayName("Url Imagen")]
        public string Imagen { get; set; } = string.Empty;

        [Range(1, double.MaxValue, ErrorMessage = "El precio del producto debe ser de al menos s/ 1.00.")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal Precio { get; set; }

        [DisplayName("Categoría")]
        [Required(ErrorMessage = "Debe indicar la categoría del producto.")]
        public int CategoriaID { get; set; }

        public bool Activo { get; set; }

        public Categoria Categoria { get; set; }
    }
}
