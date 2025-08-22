

using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services
{
    internal interface IInventoryService
    {
        void AddProduct(Product product);
        Product? FindProductById(int productId);
        IReadOnlyList<Product> GetAllProducts();
        string GetProductDetails(int productId);

        IEnumerable<Product> GetProductsWithLowStock(int threshold);
        decimal GetTotalInventoryValue();
        IEnumerable<Product> SearchProducts(Func<Product, bool> filter);
        Dictionary<string, int> GetProductCountByCategory();
    }
}
