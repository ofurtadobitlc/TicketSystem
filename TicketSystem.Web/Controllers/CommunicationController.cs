using Microsoft.AspNetCore.Mvc;

namespace TicketSystem.Web.Controllers
{
    public class CommunicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
