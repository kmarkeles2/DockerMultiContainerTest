using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                // Call *mywebapi*, and display its response in the page
                var request = new System.Net.Http.HttpRequestMessage();
                // webapi is the container name
                request.RequestUri = new Uri("http://api/WeatherForecast");
                var response = await client.SendAsync(request);
                string forecast = await response.Content.ReadAsStringAsync();
                ViewData["Message"] = $"Weather forecast :{forecast}";
            }
        }
    }
}
