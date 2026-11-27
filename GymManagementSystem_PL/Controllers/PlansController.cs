using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.Services;
using GymManagementSystem_BLL.ViewModels.Plans;
using GymManagementSystem_PL.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem_PL.Controllers
{
    public class PlansController(IPlanService plans) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await plans.GetAllAsync(ct));

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
            => await plans.GetDetailsAsync(id, ct) switch
            {
                PlanViewModel plan => View(plan),
                Error => NotFound()
            };

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
            => await plans.GetForUpdateAsync(id, ct) switch
            {
                PlanEditViewModel plan => View(plan),
                Error => NotFound()
            };

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, PlanEditViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await plans.UpdateAsync(id, model, ct);

            if (result is Error error)
            {
                if (error.Type == ErrorType.NotFound) return NotFound();

                ModelState.AddError(error);
                return View(model);
            }

            TempData["Success"] = "Plan updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await plans.ToggleStatusAsync(id, ct);

            if (result is Error { Type: ErrorType.NotFound }) return NotFound();

            if (result is Error error)
                TempData["Error"] = error.Message;
            else
                TempData["Success"] = "Plan Status Changed";

            return RedirectToAction(nameof(Index));
        }
    }
}
