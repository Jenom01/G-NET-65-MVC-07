using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Data.Seed
{
    public static class TrainerSeeder
    {
        public static async Task SeedAsync(GymDbContext dbContext)
        {
            if (await dbContext.Set<Trainer>().AnyAsync()) return;

            var rows = await SeedReader.ReadAsync<TrainerSeed>("trainers.json");
            var now = DateTime.UtcNow;

            var trainers = rows.Select(r => new Trainer
            {
                Name = r.Name,
                Email = r.Email,
                Phone = r.Phone,
                DateOfBirth = r.DateOfBirth,
                Gender = r.Gender,
                Address = r.Address,
                Speciality = r.Speciality,
                HireDate = now.AddMonths(-r.HiredMonthsAgo)
            }).ToList();

            await dbContext.Set<Trainer>().AddRangeAsync(trainers);
            await dbContext.SaveChangesAsync();
        }
    }
}
