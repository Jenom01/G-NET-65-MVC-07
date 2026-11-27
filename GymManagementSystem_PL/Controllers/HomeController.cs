using GymManagementSystem_BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagementSystem_PL.Controllers
{
    public class HomeController(IAnalyticsService analytics) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await analytics.GetDashboardAsync(ct));

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
