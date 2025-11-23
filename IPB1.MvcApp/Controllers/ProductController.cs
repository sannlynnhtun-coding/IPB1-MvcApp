using Microsoft.AspNetCore.Mvc;

namespace IPB1.MvcApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7243/api/product");
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);

            return View();
        }
    }
}
