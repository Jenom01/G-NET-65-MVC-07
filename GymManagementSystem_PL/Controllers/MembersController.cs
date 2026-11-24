using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.Services;
using GymManagementSystem_BLL.ViewModels.HealthRecords;
using GymManagementSystem_BLL.ViewModels.Members;
using GymManagementSystem_PL.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem_PL.Controllers
{
    public class MembersController(IMemberService members) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await members.GetAllAsync(ct));

        [HttpGet]
        public IActionResult Create()
            => View(new MemberCreateViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberCreateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await members.CreateAsync(model, ct);

            if (result is Error error)
            {
                ModelState.AddError(error);
                return View(model);
            }

            TempData["Success"] = "Member created successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
            => await members.GetDetailsAsync(id, ct) switch
            {
                MemberDetailsViewModel member => View(member),
                Error => NotFound()
            };

        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
            => await members.GetHealthRecordAsync(id, ct) switch
            {
                HealthRecordDetailsViewModel record => View(record),
                Error => NotFound()
            };

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
            => await members.GetForUpdateAsync(id, ct) switch
            {
                MemberEditViewModel member => View(member),
                Error => NotFound()
            };

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, MemberEditViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await members.UpdateAsync(id, model, ct);

            if (result is Error error)
            {
                if (error.Type == ErrorType.NotFound) return NotFound();

                ModelState.AddError(error);
                return View(model);
            }

            TempData["Success"] = "Member updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => await members.GetDetailsAsync(id, ct) switch
            {
                MemberDetailsViewModel member => View(member),
                Error => NotFound()
            };

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await members.DeleteAsync(id, ct);

            if (result is Error { Type: ErrorType.NotFound }) return NotFound();

            if (result is Error error)
                TempData["Error"] = error.Message;
            else
                TempData["Success"] = "Member deleted successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
