using AutoMapper;
using GymManagementSystem_BLL.Errors;
using GymManagementSystem_BLL.Helpers;
using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.ViewModels.Plans;
using GymManagementSystem_DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem_BLL.Services
{
    public class PlanService(IUnitOfWork uow, IMapper mapper, ILogger<PlanService> logger)
        : IPlanService
    {
        public async Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken ct = default)
            => mapper.Map<IEnumerable<PlanViewModel>>(await uow.Plans.GetAllAsync(ct));

        public async Task<Result<PlanViewModel>> GetDetailsAsync(int id, CancellationToken ct = default)
        {
            var plan = await uow.Plans.GetByIdAsync(id, ct);

            if (plan is null) return PlanErrors.NotFound;

            return mapper.Map<PlanViewModel>(plan);
        }

        public async Task<Result<PlanEditViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default)
        {
            var plan = await uow.Plans.GetByIdAsync(id, ct);

            if (plan is null) return PlanErrors.NotFound;

            return mapper.Map<PlanEditViewModel>(plan);
        }

        public async Task<Result> UpdateAsync(int id, PlanEditViewModel model, CancellationToken ct = default)
        {
            var plan = await uow.Plans.GetByIdAsync(id, ct);

            if (plan is null) return PlanErrors.NotFound;

            if (await uow.Plans.HasActiveMembershipsAsync(id, ct))
                return logger.Rejected("Update plan", PlanErrors.CannotUpdateWithActiveMemberships, id);

            mapper.Map(model, plan);
            await uow.SaveChangesAsync(ct);

            logger.LogInformation("Plan {PlanId} updated", id);
            return new Ok();
        }

        public async Task<Result> ToggleStatusAsync(int id, CancellationToken ct = default)
        {
            var plan = await uow.Plans.GetByIdAsync(id, ct);

            if (plan is null) return PlanErrors.NotFound;

            if (plan.IsActive && await uow.Plans.HasActiveMembershipsAsync(id, ct))
                return logger.Rejected("Deactivate plan", PlanErrors.CannotDeactivateWithActiveMemberships, id);

            plan.IsActive = !plan.IsActive;
            await uow.SaveChangesAsync(ct);

            logger.LogInformation("{PlanName} plan is now {Status}", plan.Name, plan.IsActive ? "active" : "inactive");
            return new Ok();
        }
    }
}
