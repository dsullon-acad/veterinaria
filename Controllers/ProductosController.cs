using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VeterinariaWeb.Models;

namespace VeterinariaWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly string cadenaConexion = "Server=localhost;database=veterinaria;Integrated Security=true;TrustServerCertificate=true";

        public IActionResult Index()
        {
            var listaProductos = obtenerProductos();
            return View(listaProductos);
        }

        #region . Private methods .

        private List<Producto> obtenerProductos()
        {
            var listaProductos = new List<Producto>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using(var comando = new SqlCommand("SELECT * FROM Productos", conexion))
                {
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if(lector!= null && lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                listaProductos.Add(new Producto
                                {
                                    ID = lector.GetInt32(0),
                                    Nombre = lector.GetString(1),
                                    Descripcion = lector.GetString(2)
                                });
                            }
                        }
                    }
                }
            }            
            return listaProductos;
        }

        #endregion
    }
}
