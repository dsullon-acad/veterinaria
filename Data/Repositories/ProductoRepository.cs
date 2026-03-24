using Microsoft.Data.SqlClient;
using VeterinariaWeb.Data.Infrastructure;
using VeterinariaWeb.Models;
using VeterinariaWeb.ViewModels;

namespace VeterinariaWeb.Data.Repositories
{
    public class ProductoRepository : IProducto
    {
        private readonly string cadenaConexion = string.Empty;

        public ProductoRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:DB"] ?? string.Empty;
        }

        public bool Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Producto> Listar()
        {
            List<Producto> listado  = new List<Producto>();
            using(var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("SELECT P.*, CP.Nombre AS NombreCategoria FROM Productos P INNER JOIN CategoriaProductos CP ON P.CategoriaID = CP.ID", conexion))
                {
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                listado.Add(convertirReaderEnProducto(lector));
                            }
                        }
                    }
                }
            }
            return listado;
        }

        public bool Modificar(Producto entity)
        {
            var exito = false;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("UPDATE Productos SET Nombre = @nombre, Descripcion = @descripcion, " +
                    "Precio = @precio, CategoriaID = @categoria WHERE ID = @id", conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", entity.Nombre);
                    comando.Parameters.AddWithValue("@descripcion", entity.Descripcion);
                    comando.Parameters.AddWithValue("@precio", entity.Precio);
                    comando.Parameters.AddWithValue("@categoria", entity.CategoriaID);
                    comando.Parameters.AddWithValue("@ID", entity.ID);
                    conexion.Open();
                    exito = comando.ExecuteNonQuery() > 0;
                }
            }
            return exito;
        }

        public Producto ObtenerPorID(int id)
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
                        if (lector != null && lector.HasRows)
                        {
                            lector.Read();
                            producto = convertirReaderEnProducto(lector);
                        }
                    }
                }
            }

            return producto;
        }

        public bool Registrar(Producto entity)
        {
            var exito = false;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("INSERT INTO Productos(Nombre, Descripcion, CategoriaID, Precio, PathImagen, Activo) " +
                    "VALUES(@nombre,@descripcion,@categoria,@precio, @imagen,'1')", conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", entity.Nombre);
                    comando.Parameters.AddWithValue("@descripcion", entity.Descripcion);
                    comando.Parameters.AddWithValue("@categoria", entity.CategoriaID);
                    comando.Parameters.AddWithValue("@precio", entity.Precio);
                    comando.Parameters.AddWithValue("@imagen", entity.Imagen);
                    conexion.Open();
                    exito = comando.ExecuteNonQuery() > 0;
                }
            }
            return exito;
        }


        #region . MÉTODOS PRIVADOS .

        private Producto convertirReaderEnProducto(SqlDataReader lector)
        {
            return new Producto()
            {
                ID = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.GetString(2),
                Imagen = lector["PathImagen"] == DBNull.Value ? "" : lector.GetString(3),
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
