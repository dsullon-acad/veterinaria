using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using VeterinariaWeb.Models;

namespace VeterinariaWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly string cadenaConexion = "Server=localhost;database=veterinaria;User Id=APPData; password=123456;TrustServerCertificate=true";

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

        public IActionResult Create()
        {
            var categorias = obtenerCategorias();
            ViewBag.Categorias = new SelectList(categorias, "ID", "Nombre");
            return View(new Producto());
        }

        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            var exito = CrearProducto(producto);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var productoBuscado = obtenerProductoPorId(id);
            var categorias = obtenerCategorias();
            ViewBag.Categorias = new SelectList(categorias, "ID", "Nombre");
            return View(productoBuscado);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            var exito = ActualizarProducto(producto);
            if(exito)
                return RedirectToAction("Detail", new { id = producto.ID});
            return View(producto);
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

        private List<Categoria> obtenerCategorias()
        {
            var listaCategorias = new List<Categoria>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("SELECT * FROM CategoriaProductos", conexion))
                {
                    conexion.Open();
                    using (var reader = comando.ExecuteReader())
                    {
                        if(reader!= null && reader.HasRows)
                        {
                            while (reader.Read())
                                listaCategorias.Add(convertirReaderEnCategoria(reader));
                        }
                    }
                }
            }
            return listaCategorias;
        }

        private bool CrearProducto(Producto producto)
        {
            var exito = false;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("INSERT INTO Productos(Nombre, Descripcion, CategoriaID, Precio, Activo) " +
                    "VALUES(@nombre,@descripcion,@categoria,@precio,'1')", conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", producto.Nombre);
                    comando.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    comando.Parameters.AddWithValue("@categoria", producto.CategoriaID);
                    comando.Parameters.AddWithValue("@precio", producto.Precio);
                    conexion.Open();
                    exito = comando.ExecuteNonQuery() > 0;
                }
            }
            return exito;
        }

        private bool ActualizarProducto(Producto producto)
        {
            var exito = false;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("UPDATE Productos SET Nombre = @nombre, Descripcion = @descripcion, " +
                    "Precio = @precio, CategoriaID = @categoria WHERE ID = @id", conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", producto.Nombre);
                    comando.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    comando.Parameters.AddWithValue("@precio", producto.Precio);
                    comando.Parameters.AddWithValue("@categoria", producto.CategoriaID);
                    comando.Parameters.AddWithValue("@ID", producto.ID);
                    conexion.Open();
                    exito = comando.ExecuteNonQuery() > 0;
                }
            }
            return exito;
        }

        private Producto convertirReaderEnProducto(SqlDataReader lector)
        {
            return new Producto()
            {
                ID = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.GetString(2),
                Imagen = lector["PathImagen"] == DBNull.Value ? "":  lector.GetString(3),
                Precio = lector.GetDecimal(4),
                CategoriaID = lector.GetInt32(5),
                Categoria = new Categoria()
                {
                    ID = lector.GetInt32(5),
                    Nombre = lector.GetString(7)
                }
            };
        }

        private Categoria convertirReaderEnCategoria(SqlDataReader lector)
        {
            return new Categoria
            {
                ID = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Activo = lector.GetString(2),
            };
        }

        #endregion
    }
}
