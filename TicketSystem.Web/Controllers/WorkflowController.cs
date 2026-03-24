using Microsoft.AspNetCore.Mvc;

namespace TicketSystem.Web.Controllers
{
    public class WorkflowController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
