using Microsoft.Data.SqlClient;
using VeterinariaWeb.Data.Infrastructure;
using VeterinariaWeb.Models;

namespace VeterinariaWeb.Data.Repositories
{
    public class CategoriaRepository : ICategoria
    {
        private readonly string cadenaConexion = string.Empty;

        public CategoriaRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:DB"] ?? string.Empty;
        }
        public bool Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Categoria> Listar()
        {
            var listaCategorias = new List<Categoria>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("SELECT * FROM CategoriaProductos", conexion))
                {
                    conexion.Open();
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                                listaCategorias.Add(convertirReaderEnCategoria(reader));
                        }
                    }
                }
            }
            return listaCategorias;
        }

        public bool Modificar(Categoria entity)
        {
            throw new NotImplementedException();
        }

        public Categoria ObtenerPorID(int id)
        {
            throw new NotImplementedException();
        }

        public bool Registrar(Categoria entity)
        {
            throw new NotImplementedException();
        }


        #region . MÉTODOS PRIVADOS .

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
