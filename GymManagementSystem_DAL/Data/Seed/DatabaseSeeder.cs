namespace GymManagementSystem_DAL.Data.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAllAsync(GymDbContext dbContext)
        {
            await PlanSeeder.SeedAsync(dbContext);
            await CategorySeeder.SeedAsync(dbContext);
            await MemberSeeder.SeedAsync(dbContext);
            await TrainerSeeder.SeedAsync(dbContext);
            await MembershipSeeder.SeedAsync(dbContext);
            await SessionSeeder.SeedAsync(dbContext);
            await BookingSeeder.SeedAsync(dbContext);
        }
    }
}
