using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VeterinariaWeb.Models;

namespace VeterinariaWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly string cadenaConexion = "Server=localhost;database=veterinaria;User=sa; password=sqladmin;TrustServerCertificate=true";

        public IActionResult Index()
        {
            var listaProductos = obtenerProductos();
            return View(listaProductos);
        }

        public IActionResult Detail(int id)
        {
            var productoBuscado = obtenerProductoPorId(id);
            return View(productoBuscado);
        }

        #region . Private methods .

        private List<Producto> obtenerProductos()
        {
            var listaProductos = new List<Producto>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using(var comando = new SqlCommand("SELECT P.*, CP.Nombre AS NombreCategoria FROM Productos P INNER JOIN CategoriaProductos CP ON P.CategoriaID = CP.ID", conexion))
                {
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if(lector!= null && lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                listaProductos.Add(convertirReaderEnProducto(lector));
                            }
                        }
                    }
                }
            }            
            return listaProductos;
        }

        private Producto obtenerProductoPorId(int id)
        {
            var producto = new Producto();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("SELECT P.*, CP.Nombre AS NombreCategoria FROM Productos P INNER JOIN CategoriaProductos CP ON P.CategoriaID = CP.ID WHERE P.ID = @ID", conexion))
                {
                    comando.Parameters.AddWithValue("@ID", id);
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if(lector!= null && lector.HasRows)
                        {
                            lector.Read();
                            producto = convertirReaderEnProducto(lector);
                        }
                    }
                }
            }

            return producto;
        }

        private Producto convertirReaderEnProducto(SqlDataReader lector)
        {
            return new Producto()
            {
                ID = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.GetString(2),
                Imagen = lector.GetString(3),
                Precio = lector.GetDecimal(4),
                CategoriaID = lector.GetInt32(5),
                Categoria = new Categoria()
                {
                    ID = lector.GetInt32(5),
                    Nombre = lector.GetString(7)
                }
            };
        }

        #endregion
    }
}
