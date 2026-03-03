namespace VeterinariaWeb.Models
{
    public class Producto
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public decimal Precio { get; set; }
        public int CategoriaID { get; set; }
        public bool Activo { get; set; }

        public Categoria Categoria { get; set; }
    }
}
