using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Data
{
    internal interface IGenericRepository<T> where T : class, IEntity
    {
        T? GetById(int id);
        IReadOnlyList<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Remove(int id);
    }
}
