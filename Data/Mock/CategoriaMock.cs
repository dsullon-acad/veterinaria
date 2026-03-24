using VeterinariaWeb.Data.Infrastructure;
using VeterinariaWeb.Models;

namespace VeterinariaWeb.Data.Mock
{
    public class CategoriaMock : ICategoria
    {
        public bool Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Categoria> Listar()
        {
            var listado = new List<Categoria>();
            listado.Add(new Categoria
            {
                ID = 1,
                Nombre = "DEMO ID 1",
                Activo = "1"
            });
            listado.Add(new Categoria
            {
                ID = 2,
                Nombre = "DEMO ID 2",
                Activo = "1"
            });
            return listado;
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
    }
}
