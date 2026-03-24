using Microsoft.AspNetCore.Mvc;

namespace TicketSystem.Web.Controllers
{
    public class TicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
