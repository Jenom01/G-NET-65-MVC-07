using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Data.Seed
{
    public static class MembershipSeeder
    {
        public static async Task SeedAsync(GymDbContext dbContext)
        {
            if (await dbContext.Memberships.AnyAsync()) return;

            var rows = await SeedReader.ReadAsync<MembershipSeed>("memberships.json");

            var members = await dbContext.Set<Member>()
                .AsNoTracking()
                .ToDictionaryAsync(m => m.Email, m => m.Id);

            var plans = await dbContext.Plans
                .AsNoTracking()
                .ToDictionaryAsync(p => p.Name, p => new { p.Id, p.DurationDays });

            var now = DateTime.UtcNow;
            var problems = new SeedProblems("memberships.json");
            var memberships = new List<Membership>();

            foreach (var row in rows)
            {
                if (!members.TryGetValue(row.MemberEmail, out var memberId))
                {
                    problems.Add($"unknown member '{row.MemberEmail}'");
                    continue;
                }
                if (!plans.TryGetValue(row.Plan, out var plan))
                {
                    problems.Add($"unknown plan '{row.Plan}' (member {row.MemberEmail})");
                    continue;
                }

                var start = now.AddDays(row.StartOffsetDays);
                memberships.Add(new Membership
                {
                    MemberId = memberId,
                    PlanId = plan.Id,
                    StartDate = start,
                    EndDate = start.AddDays(plan.DurationDays)
                });
            }

            problems.ThrowIfAny();

            await dbContext.Memberships.AddRangeAsync(memberships);
            await dbContext.SaveChangesAsync();
        }
    }
}
