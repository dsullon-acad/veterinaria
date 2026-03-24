namespace VeterinariaWeb.Data.Infrastructure
{
    public interface IGeneric<Entity> where Entity : class
    {
        List<Entity> Listar();
        Entity ObtenerPorID(int id);
        bool Registrar(Entity entity);
        bool Modificar(Entity entity);
        bool Eliminar(int id);
    }
}
