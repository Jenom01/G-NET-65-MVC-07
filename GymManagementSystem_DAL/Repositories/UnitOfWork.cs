using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Models;

namespace GymManagementSystem_DAL.Repositories
{
    public class UnitOfWork(GymDbContext dbContext) : IUnitOfWork
    {
        private IMemberRepository? _members;
        private IPlanRepository? _plans;
        private ISessionRepository? _sessions;
        private ITrainerRepository? _trainers;
        private IGenericRepository<Booking>? _bookings;
        private IGenericRepository<Category>? _categories;
        private IGenericRepository<HealthRecord>? _healthRecords;

        public IMemberRepository Members
            => _members ??= new MemberRepository(dbContext);

        public IPlanRepository Plans
            => _plans ??= new PlanRepository(dbContext);

        public ISessionRepository Sessions
            => _sessions ??= new SessionRepository(dbContext);

        public ITrainerRepository Trainers
            => _trainers ??= new TrainerRepository(dbContext);

        public IGenericRepository<Booking> Bookings
            => _bookings ??= new GenericRepository<Booking>(dbContext);

        public IGenericRepository<Category> Categories
            => _categories ??= new GenericRepository<Category>(dbContext);

        public IGenericRepository<HealthRecord> HealthRecords
            => _healthRecords ??= new GenericRepository<HealthRecord>(dbContext);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await dbContext.SaveChangesAsync(ct);
    }
}
