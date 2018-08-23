
//using Access2Justice.Tools.BusinessLogic;
//using System;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;

//namespace Access2Justice.Tools
//{

//    public class Product
//    {
//        public string Id { get; set; }
//        public string Name { get; set; }
//        public decimal Price { get; set; }
//        public string Category { get; set; }
//    }

//    class Program2
//    {
//        static HttpClient client = new HttpClient();

//        static void ShowProduct(Product product)
//        {
//            Console.WriteLine($"Name: {product.Name}\tPrice: " +
//                $"{product.Price}\tCategory: {product.Category}");
//        }

//        static async Task<Uri> CreateProductAsync(Product product)
//        {
//            HttpResponseMessage response = await client.PostAsJsonAsync(
//                "api/products", product).ConfigureAwait(false);
//            response.EnsureSuccessStatusCode();

//            // return URI of the created resource.
//            return response.Headers.Location;
//        }

//        static async Task<Product> GetProductAsync(string path)
//        {
//            Product product = null;
//            HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
//            if (response.IsSuccessStatusCode)
//            {
//                product = await response.Content.ReadAsAsync<Product>().ConfigureAwait(false);
//            }
//            return product;
//        }

//        static async Task<Product> UpdateProductAsync(Product product)
//        {
//            HttpResponseMessage response = await client.PutAsJsonAsync(
//                $"api/products/{product.Id}", product).ConfigureAwait(false);
//            response.EnsureSuccessStatusCode();

//            // Deserialize the updated product from the response body.
//            product = await response.Content.ReadAsAsync<Product>().ConfigureAwait(false);
//            return product;
//        }

//        static async Task<HttpStatusCode> DeleteProductAsync(string id)
//        {
//            HttpResponseMessage response = await client.DeleteAsync(
//                $"api/products/{id}").ConfigureAwait(false);
//            return response.StatusCode;
//        }

//        static void Main()
//        {
//            RunAsync().GetAwaiter().GetResult();
//        }

//        static async Task RunAsync()
//        {
//            // Update port # in the following line.
//            client.BaseAddress = new Uri("http://localhost:4200/");
//            client.DefaultRequestHeaders.Accept.Clear();
//            client.DefaultRequestHeaders.Accept.Add(
//                new MediaTypeWithQualityHeaderValue("application/json"));

//            try
//            {
//                // Create a new product
//                Product product = new Product
//                {
//                    Name = "Gizmo",
//                    Price = 100,
//                    Category = "Widgets"
//                };

//                var url = await CreateProductAsync(product).ConfigureAwait(false);
//                Console.WriteLine($"Created at {url}");

//                // Get the product
//                product = await GetProductAsync(url.PathAndQuery).ConfigureAwait(false);
//                ShowProduct(product);

//                // Update the product
//                Console.WriteLine("Updating price...");
//                product.Price = 80;
//                await UpdateProductAsync(product).ConfigureAwait(false);

//                // Get the updated product
//                product = await GetProductAsync(url.PathAndQuery).ConfigureAwait(false);
//                ShowProduct(product);

//                // Delete the product
//                var statusCode = await DeleteProductAsync(product.Id).ConfigureAwait(false);
//                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }

//            Console.ReadLine();
//        }
//    }
//}




////    class Program
////    {
////        static HttpClient client = new HttpClient();

////        static void Main(string[] args)
////        {
////            Console.WriteLine("Executing the script...");        

////            TopicBusinessLogic p = new TopicBusinessLogic();
////            p.GetTopics().Wait();
////            Console.WriteLine("Topics created.");
////            ResourceBusinessLogic q = new ResourceBusinessLogic();
////            //q.GetResources().Wait();
////            Console.WriteLine("Resources created.");
////            Console.WriteLine("Script completed.");
////            Console.ReadLine();
////        }
////    }
////}