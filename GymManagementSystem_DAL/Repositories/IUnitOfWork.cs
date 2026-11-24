using GymManagementSystem_DAL.Models;

namespace GymManagementSystem_DAL.Repositories
{
    public interface IUnitOfWork
    {
        IMemberRepository Members { get; }
        IPlanRepository Plans { get; }
        ISessionRepository Sessions { get; }
        ITrainerRepository Trainers { get; }
        IGenericRepository<Booking> Bookings { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<HealthRecord> HealthRecords { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
