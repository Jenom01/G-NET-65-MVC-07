using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.Services;
using GymManagementSystem_BLL.ViewModels.Trainers;
using GymManagementSystem_PL.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem_PL.Controllers
{
    public class TrainersController(ITrainerService trainers) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await trainers.GetAllAsync(ct));

        [HttpGet]
        public IActionResult Create()
            => View(new TrainerCreateViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(TrainerCreateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await trainers.CreateAsync(model, ct);

            if (result is Error error)
            {
                ModelState.AddError(error);
                return View(model);
            }

            TempData["Success"] = "Trainer created successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
            => await trainers.GetDetailsAsync(id, ct) switch
            {
                TrainerDetailsViewModel trainer => View(trainer),
                Error => NotFound()
            };

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
            => await trainers.GetForUpdateAsync(id, ct) switch
            {
                TrainerEditViewModel trainer => View(trainer),
                Error => NotFound()
            };

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, TrainerEditViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await trainers.UpdateAsync(id, model, ct);

            if (result is Error error)
            {
                if (error.Type == ErrorType.NotFound) return NotFound();

                ModelState.AddError(error);
                return View(model);
            }

            TempData["Success"] = "Trainer updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => await trainers.GetDetailsAsync(id, ct) switch
            {
                TrainerDetailsViewModel trainer => View(trainer),
                Error => NotFound()
            };

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await trainers.DeleteAsync(id, ct);

            if (result is Error { Type: ErrorType.NotFound }) return NotFound();

            if (result is Error error)
                TempData["Error"] = error.Message;
            else
                TempData["Success"] = "Trainer deleted successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
