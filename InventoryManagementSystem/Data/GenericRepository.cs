using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Data
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity

    {
        private readonly List<T> _items = new();
        private readonly Dictionary<int, T> _itemDictionary = new();
        private int _nextId = 1;

        public void Add(T entity)
        {
            entity.Id = _nextId++;

            _items.Add(entity);
            _itemDictionary[entity.Id] = entity;
        }

        public IReadOnlyList<T> GetAll()
        {
            return _items.AsReadOnly();
        }

        public T? GetById(int id)
        {
            if (_itemDictionary.ContainsKey(id))
            {
                return _itemDictionary[id];
            }
            return null;
        }

        public void Remove(int id)
        {
            if (_itemDictionary.ContainsKey(id))
            {
                var item = _itemDictionary[id];
                _items.Remove(item);
                _itemDictionary.Remove(id);
            }
        }

        public void Update(T entity)
        {
            if (_itemDictionary.ContainsKey(entity.Id))
            {
                var existingItemIndex = _items.FindIndex(item => item.Id == entity.Id);
                if (existingItemIndex != -1)
                {
                    _items[existingItemIndex] = entity;
                    _itemDictionary[existingItemIndex] = entity;
                }
            }
        }
    }
}
