using GymManagementSystem_DAL.Enums;
using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Data.Seed
{
    public static class SessionSeeder
    {
        public static async Task SeedAsync(GymDbContext dbContext)
        {
            if (await dbContext.Sessions.AnyAsync()) return;

            var rows = await SeedReader.ReadAsync<SessionSeed>("sessions.json");

            var trainers = await dbContext.Set<Trainer>().AsNoTracking()
                .ToDictionaryAsync(t => t.Email);
            var categories = await dbContext.Categories.AsNoTracking()
                .ToDictionaryAsync(c => c.Name);

            var now = DateTime.UtcNow;
            var problems = new SeedProblems("sessions.json");
            var sessions = new List<Session>();

            foreach (var duplicate in rows.GroupBy(r => r.Description).Where(g => g.Count() > 1))
                problems.Add($"description '{duplicate.Key}' is used {duplicate.Count()} times (bookings find sessions by description)");

            foreach (var row in rows)
            {
                if (!trainers.TryGetValue(row.TrainerEmail, out var trainer))
                {
                    problems.Add($"'{row.Description}': unknown trainer '{row.TrainerEmail}'");
                    continue;
                }
                if (!categories.TryGetValue(row.Category, out var category))
                {
                    problems.Add($"'{row.Description}': unknown category '{row.Category}'");
                    continue;
                }

                if (!SpecialityMatches(trainer.Speciality, category.Name))
                    problems.Add($"'{row.Description}': {trainer.Name} ({trainer.Speciality}) cannot teach {category.Name}");

                if (row.Capacity is < 1 or > 25)
                    problems.Add($"'{row.Description}': capacity {row.Capacity} is outside 1-25");

                if (row.DurationMinutes <= 0)
                    problems.Add($"'{row.Description}': duration must be positive");

                var start = now.AddHours(row.StartOffsetHours);
                sessions.Add(new Session
                {
                    Description = row.Description,
                    Capacity = row.Capacity,
                    StartDate = start,
                    EndDate = start.AddMinutes(row.DurationMinutes),
                    TrainerId = trainer.Id,
                    CategoryId = category.Id
                });
            }

            // A trainer can't have overlapping sessions (back-to-back is fine: end == next start)
            var trainerNames = trainers.Values.ToDictionary(t => t.Id, t => t.Name);
            foreach (var group in sessions.GroupBy(s => s.TrainerId))
            {
                var latestEnd = DateTime.MinValue;
                var latestEndDescription = "";

                foreach (var session in group.OrderBy(s => s.StartDate))
                {
                    if (session.StartDate < latestEnd)
                        problems.Add($"{trainerNames[group.Key]} is double-booked: '{session.Description}' overlaps '{latestEndDescription}'");

                    if (session.EndDate > latestEnd)
                    {
                        latestEnd = session.EndDate;
                        latestEndDescription = session.Description;
                    }
                }
            }

            problems.ThrowIfAny();

            await dbContext.Sessions.AddRangeAsync(sessions);
            await dbContext.SaveChangesAsync();
        }

        // Category names equal the enum names, apart from spaces ("Strength Training" == StrengthTraining)
        private static bool SpecialityMatches(Speciality speciality, string categoryName)
            => string.Equals(speciality.ToString(), categoryName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase);
    }
}
