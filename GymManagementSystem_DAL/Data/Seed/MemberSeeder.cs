using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Data.Seed
{
    public static class MemberSeeder
    {
        public static async Task SeedAsync(GymDbContext dbContext)
        {
            if (await dbContext.Set<Member>().AnyAsync()) return;

            var rows = await SeedReader.ReadAsync<MemberSeed>("members.json");
            var now = DateTime.UtcNow;

            var members = rows.Select(r => new Member
            {
                Name = r.Name,
                Email = r.Email,
                Phone = r.Phone,
                DateOfBirth = r.DateOfBirth,
                Gender = r.Gender,
                Address = r.Address,
                JoinDate = r.JoinedMonthsAgo is { } months ? now.AddMonths(-months) : null,
                HealthRecord = new HealthRecord
                {
                    Height = r.HealthRecord.Height,
                    Weight = r.HealthRecord.Weight,
                    BloodType = r.HealthRecord.BloodType,
                    Note = r.HealthRecord.Note
                }
            }).ToList();

            await dbContext.Set<Member>().AddRangeAsync(members);
            await dbContext.SaveChangesAsync();
        }
    }
}
