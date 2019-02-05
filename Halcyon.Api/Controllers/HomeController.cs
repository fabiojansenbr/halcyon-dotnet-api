using Microsoft.AspNetCore.Mvc;

namespace Halcyon.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("swagger");
        }
    }
}