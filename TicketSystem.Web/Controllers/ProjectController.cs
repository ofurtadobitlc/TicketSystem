using Microsoft.AspNetCore.Mvc;

namespace TicketSystem.Web.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
