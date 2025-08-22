using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem
{
    class Program
    {
        private static readonly IInventoryService _inventoryService;

        static Program()
        {
            var productRepository = new GenericRepository<Product>();
            var categoryRepository = new GenericRepository<Category>();

            _inventoryService = new InventoryService(productRepository, categoryRepository);

            SeedData(categoryRepository, productRepository);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Inventory Management System");
            Console.WriteLine("===========================");

            bool running = true;
            while (running)
            {
                DisplayMenu();
                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1": ViewAllProducts(); break;
                    case "2": AddNewProduct(); break;
                    case "3": SearchForProductByName(); break;
                    case "4": RunLowStockReport(); break;
                    case "5": RunCategoryCountReport(); break;
                    case "6": Console.WriteLine($"Total Inventory Value: {_inventoryService.GetTotalInventoryValue():C}"); break;
                    case "7": running = false; break;
                    default: Console.WriteLine("Invalid option, please try again."); break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. View All Products");
            Console.WriteLine("2. Add a New Product");
            Console.WriteLine("3. Search for a Product (by name)");
            Console.WriteLine("4. Run Low Stock Report");
            Console.WriteLine("5. Run Category Count Report");
            Console.WriteLine("6. Get Total Inventory Value");
            Console.WriteLine("7. Exit");
        }

        private static void ViewAllProducts()
        {
            Console.WriteLine("\n--- All Products ---");
            var products = _inventoryService.GetAllProducts();
            if (!products.Any())
            {
                Console.WriteLine("No products in inventory.");
                return;
            }
            foreach (var product in products)
            {
                Console.WriteLine(_inventoryService.GetProductDetails(product.Id));
            }
        }

        private static void SearchForProductByName()
        {
            Console.Write("Enter product name to search for: ");
            string searchTerm = Console.ReadLine() ?? "";

            // DELEGATES/LAMBDAS: Passing a lambda expression to our flexible search method.
            var results = _inventoryService.SearchProducts(p =>
                p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            Console.WriteLine($"\n--- Search Results for '{searchTerm}' ---");
            if (!results.Any())
            {
                Console.WriteLine("No products found matching that name.");
                return;
            }
            foreach (var product in results)
            {
                Console.WriteLine(_inventoryService.GetProductDetails(product.Id));
            }
        }

        private static void RunLowStockReport()
        {
            Console.Write("Enter stock threshold (e.g., 10): ");
            if (int.TryParse(Console.ReadLine(), out int threshold))
            {
                var lowStockProducts = _inventoryService.GetProductsWithLowStock(threshold);
                Console.WriteLine($"\n--- Products with stock below {threshold} ---");
                if (!lowStockProducts.Any())
                {
                    Console.WriteLine("All products are above the stock threshold.");
                    return;
                }
                foreach (var product in lowStockProducts)
                {
                    Console.WriteLine(_inventoryService.GetProductDetails(product.Id));
                }
            }
            else
            {
                Console.WriteLine("Invalid number.");
            }
        }

        private static void RunCategoryCountReport()
        {
            Console.WriteLine("\n--- Product Count by Category ---");
            var report = _inventoryService.GetProductCountByCategory();
            foreach (var entry in report)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value} product(s)");
            }
        }

        private static void AddNewProduct()
        {
            // This is a simplified method. A real app would have more robust input validation.
            try
            {
                Console.WriteLine("\n--- Add New Product ---");
                Console.Write("Name: ");
                string name = Console.ReadLine() ?? "";
                Console.Write("Description: ");
                string desc = Console.ReadLine() ?? "";
                Console.Write("Price: ");
                decimal price = decimal.Parse(Console.ReadLine() ?? "0");
                Console.Write("Stock Quantity: ");
                int quantity = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Category ID (1=Electronics, 2=Books, 3=Groceries): ");
                int catId = int.Parse(Console.ReadLine() ?? "1");

                var newProduct = new Product
                {
                    Name = name,
                    Description = desc,
                    Price = price,
                    StockQuantity = quantity,
                    CategoryId = catId
                };
                _inventoryService.AddProduct(newProduct);
                Console.WriteLine("Product added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
            }
        }


        private static void SeedData(IGenericRepository<Category> categoryRepo, IGenericRepository<Product> productRepo)
        {
            // Categories
            var cat1 = new Category { Name = "Electronics" };
            var cat2 = new Category { Name = "Books" };
            var cat3 = new Category { Name = "Groceries" };
            categoryRepo.Add(cat1);
            categoryRepo.Add(cat2);
            categoryRepo.Add(cat3);

            // Products
            productRepo.Add(new Product { Name = "Laptop", Description = "A powerful laptop", Price = 1200.50m, StockQuantity = 15, CategoryId = cat1.Id });
            productRepo.Add(new Product { Name = "C# in Depth", Description = "A great book for C#", Price = 49.99m, StockQuantity = 50, CategoryId = cat2.Id });
            productRepo.Add(new Product { Name = "Mouse", Description = "A wireless mouse", Price = 25.00m, StockQuantity = 5, CategoryId = cat1.Id });
            productRepo.Add(new Product { Name = "Apples", Description = "A bag of fresh apples", Price = 5.99m, StockQuantity = 120, CategoryId = cat3.Id });
            productRepo.Add(new Product { Name = "The Pragmatic Programmer", Description = "A classic software book", Price = 55.49m, StockQuantity = 8, CategoryId = cat2.Id });
        }

    }
}