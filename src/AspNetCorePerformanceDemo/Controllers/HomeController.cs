using AspNetCorePerformanceDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace AspNetCorePerformanceDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {            
            return View(GetIndexModel());
        }

        [ResponseCache(Duration = 30)]
        public IActionResult IndexCached()
        {
            return View("Index", GetIndexModel());
        }

        private IndexModel GetIndexModel()
        {
            var model = new IndexModel();
            model.CurrentTime = DateTime.Now;
            model.CachedCurrentTime = _memoryCache.GetOrCreate<DateTime>("CurrentTime", cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(20);
                return DateTime.Now;
            });
            return model;
        }

        public IActionResult ElementCached()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}