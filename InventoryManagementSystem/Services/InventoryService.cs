using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services
{
    internal class InventoryService : IInventoryService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Category> _categoryRepository;

        public InventoryService(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public void AddProduct(Product product) => _productRepository.Add(product);
        public Product? FindProductById(int productId) => _productRepository.GetById(productId);
        public IReadOnlyList<Product> GetAllProducts() => _productRepository.GetAll();

        public string GetProductDetails(int productId)
        {
            var product = _productRepository.GetById(productId);
            if (product == null) return "Product not found.";

            var category = _categoryRepository.GetById(product.CategoryId);
            string categoryName = category?.Name ?? "Unassigned";

            return $"ID: {product.Id}, Name: {product.Name}, Category: {categoryName}, Price: {product.Price:C}, Stock: {product.StockQuantity}";
        }

        public IEnumerable<Product> GetProductsWithLowStock(int threshold)
        {
            return _productRepository.GetAll().Where(p => p.StockQuantity < threshold);
        }

        public decimal GetTotalInventoryValue()
        {
            return _productRepository.GetAll().Sum(p => p.Price * p.StockQuantity);
        }

        public IEnumerable<Product> SearchProducts(Func<Product, bool> filter)
        {
            return _productRepository.GetAll().Where(filter);
        }

        public Dictionary<string, int> GetProductCountByCategory()
        {
            var categories = _categoryRepository.GetAll();
            return _productRepository.GetAll()

                .GroupBy(p => p.CategoryId)

                .ToDictionary(
                    g => categories.FirstOrDefault(c => c.Id == g.Key)?.Name ?? "Unassigned",
                    g => g.Count()
                );
        }
    }
}
