using AutoMapper;
using GymManagementSystem_BLL.Errors;
using GymManagementSystem_BLL.Helpers;
using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.ViewModels.HealthRecords;
using GymManagementSystem_BLL.ViewModels.Members;
using GymManagementSystem_DAL.Models;
using GymManagementSystem_DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem_BLL.Services
{
    public class MemberService(IUnitOfWork uow, IMapper mapper, ILogger<MemberService> logger)
        : IMemberService
    {
        public async Task<IEnumerable<MemberIndexViewModel>> GetAllAsync(CancellationToken ct = default)
            => mapper.Map<IEnumerable<MemberIndexViewModel>>(await uow.Members.GetAllAsync(ct));

        public async Task<Result> CreateAsync(MemberCreateViewModel model, CancellationToken ct = default)
        {
            model.Email = model.Email.Trim().ToLowerInvariant();
            model.Phone = model.Phone.Trim();

            if (await uow.Members.IsEmailTakenAsync(model.Email, ct: ct))
                return logger.Rejected("Create member", MemberErrors.EmailExists);

            if (await uow.Members.IsPhoneTakenAsync(model.Phone, ct: ct))
                return logger.Rejected("Create member", MemberErrors.PhoneExists);

            var member = mapper.Map<Member>(model);
            uow.Members.Add(member);
            await uow.SaveChangesAsync(ct);

            logger.LogInformation("Member {MemberId} created", member.Id);
            return new Ok();
        }

        public async Task<Result<MemberDetailsViewModel>> GetDetailsAsync(int id, CancellationToken ct = default)
        {
            var member = await uow.Members.GetDetailsAsync(id, ct);

            if (member is null) return MemberErrors.NotFound;

            var now = DateTime.UtcNow;

            var activeMembership = member.Memberships
                .Where(ms => ms.StartDate <= now && ms.EndDate > now)
                .OrderByDescending(ms => ms.EndDate)
                .FirstOrDefault();

            return new MemberDetailsViewModel
            {
                Id = id,
                Name = member.Name.Trim(),
                PhotoUrl = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",

                PlanName = activeMembership?.Plan.Name ?? "No Active Plan",
                MembershipStartDate = activeMembership?.StartDate.ToShortDateString() ?? "N/A",
                MembershipEndDate = activeMembership?.EndDate.ToShortDateString() ?? "N/A"
            };
        }

        public async Task<Result<HealthRecordDetailsViewModel>> GetHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var healthRecord = await uow.HealthRecords.GetWhereAsync(hr => hr.MemberId == memberId, ct);

            if (healthRecord is null) return MemberErrors.HealthRecordNotFound;

            return mapper.Map<HealthRecordDetailsViewModel>(healthRecord);
        }

        public async Task<Result<MemberEditViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default)
        {
            var member = await uow.Members.GetByIdAsync(id, ct);

            if (member is null) return MemberErrors.NotFound;

            return mapper.Map<MemberEditViewModel>(member);
        }

        public async Task<Result> UpdateAsync(int id, MemberEditViewModel model, CancellationToken ct = default)
        {
            var member = await uow.Members.GetByIdAsync(id, ct);

            if (member is null) return MemberErrors.NotFound;

            model.Email = model.Email.Trim().ToLowerInvariant();
            model.Phone = model.Phone.Trim();

            if (await uow.Members.IsEmailTakenAsync(model.Email, member.Id, ct))
                return logger.Rejected("Update member", MemberErrors.EmailExists, id);

            if (await uow.Members.IsPhoneTakenAsync(model.Phone, member.Id, ct))
                return logger.Rejected("Update member", MemberErrors.PhoneExists, id);

            mapper.Map(model, member);
            await uow.SaveChangesAsync(ct);

            logger.LogInformation("Member {MemberId} updated", id);
            return new Ok();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        {
            var member = await uow.Members.GetByIdAsync(id, ct, m => m.HealthRecord);

            if (member is null) return MemberErrors.NotFound;

            if (await uow.Members.HasActiveBookingsAsync(id, ct))
                return logger.Rejected("Delete member", MemberErrors.HasActiveBookings, id);

            if (member.HealthRecord is not null)
                uow.HealthRecords.Delete(member.HealthRecord);

            uow.Members.Delete(member);
            await uow.SaveChangesAsync(ct);

            logger.LogInformation("Member {MemberId} deleted", id);
            return new Ok();
        }
    }
}
