using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Visitors_Management.Models;

namespace Visitors_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult User_Master()
        {
            return View();
        }
        public IActionResult Visitorbetween()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Visitor_Master()
        {
            return View();
        }
        public IActionResult Visitor_Manage()
        {
            return View();
        }
        public IActionResult Chart()
        {
            return View();
        }
        public IActionResult Category()
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