using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.Services;
using GymManagementSystem_BLL.ViewModels.Common;
using GymManagementSystem_BLL.ViewModels.Sessions;
using GymManagementSystem_PL.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem_PL.Controllers
{
    public class SessionsController(ISessionService sessions) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await sessions.GetAllAsync(ct));

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
            => await sessions.GetDetailsAsync(id, ct) switch
            {
                SessionViewModel session => View(session),
                Error => NotFound()
            };

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
            => await CreateFormAsync(new SessionCreateViewModel(), ct);

        [HttpPost]
        public async Task<IActionResult> Create(SessionCreateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return await CreateFormAsync(model, ct);

            return await sessions.CreateAsync(model, ct) switch
            {
                Ok _ => RedirectWithSuccess("Session created successfully"),
                Error error => await CreateFormAsync(model, ct, error)
            };
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
            => await sessions.GetForUpdateAsync(id, ct) switch
            {
                SessionEditViewModel model => await EditFormAsync(model, ct),
                Error { Type: ErrorType.NotFound } => NotFound(),
                Error error => RedirectWithError(error)
            };

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SessionEditViewModel model, CancellationToken ct)
        {
            model.Id = id;

            if (!ModelState.IsValid) return await EditFormAsync(model, ct);

            return await sessions.UpdateAsync(id, model, ct) switch
            {
                Ok _ => RedirectWithSuccess("Session updated successfully"),
                Error { Type: ErrorType.NotFound } => NotFound(),
                Error { Field: null } error => RedirectWithError(error),   // not about one input, e.g. NotEditable
                Error error => await EditFormAsync(model, ct, error)       // belongs to a field: show it on the form
            };
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => await sessions.GetForDeleteAsync(id, ct) switch
            {
                SessionViewModel session => View(session),
                Error { Type: ErrorType.NotFound } => NotFound(),
                Error error => RedirectWithError(error)
            };

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
            => await sessions.DeleteAsync(id, ct) switch
            {
                Ok _ => RedirectWithSuccess("Session deleted successfully"),
                Error { Type: ErrorType.NotFound } => NotFound(),
                Error error => RedirectWithError(error)
            };

        private async Task<IActionResult> CreateFormAsync(SessionCreateViewModel model, CancellationToken ct, Error? error = null)
        {
            if (error is not null) ModelState.AddError(error);

            ViewBag.Categories = ToSelectList(await sessions.GetCategoryLookupsAsync(ct));
            ViewBag.Trainers = ToSelectList(await sessions.GetTrainerLookupsAsync(ct));
            return View("Create", model);
        }

        private async Task<IActionResult> EditFormAsync(SessionEditViewModel model, CancellationToken ct, Error? error = null)
        {
            if (error is not null) ModelState.AddError(error);

            ViewBag.Trainers = ToSelectList(await sessions.GetTrainerLookupsAsync(ct));
            return View("Edit", model);
        }

        private IActionResult RedirectWithSuccess(string message)
        {
            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        private IActionResult RedirectWithError(Error error)
        {
            TempData["Error"] = error.Message;
            return RedirectToAction(nameof(Index));
        }

        private static SelectList ToSelectList(IReadOnlyList<LookupItem> items)
            => new(items, nameof(LookupItem.Id), nameof(LookupItem.Name));
    }
}
