using AutoMapper;
using GymManagementSystem_BLL.Errors;
using GymManagementSystem_BLL.Helpers;
using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.ViewModels.Common;
using GymManagementSystem_BLL.ViewModels.Sessions;
using GymManagementSystem_DAL.Enums;
using GymManagementSystem_DAL.Models;
using GymManagementSystem_DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem_BLL.Services
{
    public class SessionService(IUnitOfWork uow, IMapper mapper, ILogger<SessionService> logger, TimeProvider clock)
        : ISessionService
    {
        private DateTime NowUtc => clock.GetUtcNow().UtcDateTime;

        public async Task<IEnumerable<SessionViewModel>> GetAllAsync(CancellationToken ct = default)
        {
            var sessions = await uow.Sessions.GetAllWithDetailsAsync(ct);
            var now = NowUtc;

            return sessions
                .Select(s => ToViewModel(s, now))
                .OrderBy(v => v.Status == SessionStatus.Completed)
                .ThenBy(v => v.StartDate)
                .ToList();
        }

        public async Task<Result<SessionViewModel>> GetDetailsAsync(int id, CancellationToken ct = default)
        {
            var session = await uow.Sessions.GetDetailsAsync(id, ct);
            if (session is null) return SessionErrors.NotFound;

            return ToViewModel(session, NowUtc);
        }

        public async Task<IReadOnlyList<LookupItem>> GetCategoryLookupsAsync(CancellationToken ct = default)
        {
            var categories = await uow.Categories.GetAllAsync(ct);
            return categories.OrderBy(c => c.Name).Select(c => new LookupItem(c.Id, c.Name)).ToList();
        }

        public async Task<IReadOnlyList<LookupItem>> GetTrainerLookupsAsync(CancellationToken ct = default)
        {
            var trainers = await uow.Trainers.GetAllAsync(ct);
            return trainers.OrderBy(t => t.Name)
                           .Select(t => new LookupItem(t.Id, $"{t.Name} ({t.Speciality.GetDisplayName()})"))
                           .ToList();
        }

        public async Task<Result> CreateAsync(SessionCreateViewModel model, CancellationToken ct = default)
        {
            var (error, startUtc, endUtc) = await ValidateScheduleAsync(
                model.CategoryId!.Value, model.TrainerId!.Value,
                model.StartDate!.Value, model.EndDate!.Value,
                excludeSessionId: null, NowUtc, ct);

            if (error is not null) return Reject(error, "create");

            var session = mapper.Map<Session>(model);
            session.StartDate = startUtc;
            session.EndDate = endUtc;

            uow.Sessions.Add(session);
            await uow.SaveChangesAsync(ct);

            logger.LogInformation(
                "Session {SessionId} created: trainer {TrainerId}, category {CategoryId}, {StartUtc:u} to {EndUtc:u}",
                session.Id, session.TrainerId, session.CategoryId, startUtc, endUtc);

            return new Ok();
        }

        public async Task<Result<SessionEditViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default)
        {
            var session = await uow.Sessions.GetByIdAsync(id, ct, s => s.Category);
            if (session is null) return SessionErrors.NotFound;

            if (!IsEditable(session, NowUtc)) return SessionErrors.NotEditable;

            var vm = mapper.Map<SessionEditViewModel>(session);
            vm.StartDate = GymTime.ToLocalMinute(session.StartDate);
            vm.EndDate = GymTime.ToLocalMinute(session.EndDate);
            return vm;
        }

        public async Task<Result> UpdateAsync(int id, SessionEditViewModel model, CancellationToken ct = default)
        {
            var session = await uow.Sessions.GetByIdAsync(id, ct);
            if (session is null) return SessionErrors.NotFound;

            var now = NowUtc;
            if (!IsEditable(session, now)) return Reject(SessionErrors.NotEditable, "update");

            var (error, startUtc, endUtc) = await ValidateScheduleAsync(
                session.CategoryId, model.TrainerId!.Value,
                model.StartDate!.Value, model.EndDate!.Value,
                excludeSessionId: session.Id, now, ct);

            if (error is not null) return Reject(error, "update");

            session.TrainerId = model.TrainerId.Value;
            session.Description = model.Description;
            session.StartDate = startUtc;
            session.EndDate = endUtc;

            uow.Sessions.Update(session);
            await uow.SaveChangesAsync(ct);

            logger.LogInformation(
                "Session {SessionId} updated: trainer {TrainerId}, {StartUtc:u} to {EndUtc:u}",
                session.Id, session.TrainerId, startUtc, endUtc);

            return new Ok();
        }

        public async Task<Result<SessionViewModel>> GetForDeleteAsync(int id, CancellationToken ct = default)
        {
            var session = await uow.Sessions.GetDetailsAsync(id, ct);
            if (session is null) return SessionErrors.NotFound;

            var now = NowUtc;
            if (!IsDeletable(session, now)) return SessionErrors.CannotDeleteOngoing;

            return ToViewModel(session, now);
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        {
            var session = await uow.Sessions.GetByIdAsync(id, ct, s => s.Bookings);
            if (session is null) return SessionErrors.NotFound;

            if (!IsDeletable(session, NowUtc)) return Reject(SessionErrors.CannotDeleteOngoing, "delete");

            var bookingCount = session.Bookings.Count;

            foreach (var booking in session.Bookings.ToList())
                uow.Bookings.Delete(booking);

            uow.Sessions.Delete(session);
            await uow.SaveChangesAsync(ct);

            logger.LogInformation("Session {SessionId} deleted with {BookingCount} bookings", id, bookingCount);

            return new Ok();
        }

        private static bool IsEditable(Session session, DateTime nowUtc)
            => session.GetStatus(nowUtc) == SessionStatus.Upcoming;

        private static bool IsDeletable(Session session, DateTime nowUtc)
            => session.GetStatus(nowUtc) != SessionStatus.Ongoing;

        private SessionViewModel ToViewModel(Session session, DateTime nowUtc)
        {
            var model = mapper.Map<SessionViewModel>(session);

            model.Status = session.GetStatus(nowUtc);
            model.StartDate = GymTime.ToLocal(session.StartDate);
            model.EndDate = GymTime.ToLocal(session.EndDate);

            return model;
        }

        private async Task<(Error? Error, DateTime StartUtc, DateTime EndUtc)> ValidateScheduleAsync(
            int categoryId, int trainerId, DateTime start, DateTime end,
            int? excludeSessionId, DateTime nowUtc, CancellationToken ct)
        {
            if (end <= start) return (SessionErrors.InvalidDateRange, default, default);

            if (GymTime.IsInvalid(start) || GymTime.IsInvalid(end))
                return (SessionErrors.InvalidLocalTime, default, default);

            var startUtc = GymTime.ToUtc(start);
            var endUtc = GymTime.ToUtc(end);

            if (startUtc <= nowUtc) return (SessionErrors.StartInPast, default, default);

            var category = await uow.Categories.GetByIdAsync(categoryId, ct);
            if (category is null) return (SessionErrors.CategoryNotFound, default, default);

            var trainer = await uow.Trainers.GetByIdAsync(trainerId, ct);
            if (trainer is null) return (SessionErrors.TrainerNotFound, default, default);

            if (!string.Equals(trainer.Speciality.GetDisplayName(), category.Name, StringComparison.OrdinalIgnoreCase))
                return (SessionErrors.SpecialityMismatch, default, default);

            if (await uow.Sessions.HasTrainerConflictAsync(trainerId, startUtc, endUtc, excludeSessionId, ct))
                return (SessionErrors.TrainerBusy, default, default);

            return (null, startUtc, endUtc);
        }

        private Error Reject(Error error, string operation)
        {
            logger.LogInformation("Session {Operation} rejected: {ErrorCode}", operation, error.Code);
            return error;
        }
    }
}
