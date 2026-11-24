using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Data.Seed
{
    public static class BookingSeeder
    {
        public static async Task SeedAsync(GymDbContext dbContext)
        {
            if (await dbContext.Bookings.AnyAsync()) return;

            var rows = await SeedReader.ReadAsync<BookingSeed>("bookings.json");

            var members = await dbContext.Set<Member>().AsNoTracking()
                .ToDictionaryAsync(m => m.Email);
            var sessions = await dbContext.Sessions.AsNoTracking()
                .ToDictionaryAsync(s => s.Description);

            var now = DateTime.UtcNow;
            var membersWithActivePlan = (await dbContext.Memberships.AsNoTracking()
                    .Where(m => m.StartDate <= now && m.EndDate > now)
                    .Select(m => m.MemberId)
                    .Distinct()
                    .ToListAsync())
                .ToHashSet();

            var problems = new SeedProblems("bookings.json");
            var bookings = new List<(Booking Booking, Member Member, Session Session)>();
            var seenPairs = new HashSet<(int, int)>();

            foreach (var row in rows)
            {
                if (!members.TryGetValue(row.MemberEmail, out var member))
                {
                    problems.Add($"unknown member '{row.MemberEmail}'");
                    continue;
                }
                if (!sessions.TryGetValue(row.Session, out var session))
                {
                    problems.Add($"unknown session '{row.Session}' (member {row.MemberEmail})");
                    continue;
                }
                if (!seenPairs.Add((member.Id, session.Id)))
                {
                    problems.Add($"{member.Name} booked '{session.Description}' twice");
                    continue;
                }

                // Only sessions that haven't finished need an active membership
                if (session.EndDate > now && !membersWithActivePlan.Contains(member.Id))
                    problems.Add($"{member.Name} has no active membership but is booked into '{session.Description}'");

                bookings.Add((new Booking
                {
                    MemberId = member.Id,
                    SessionId = session.Id,
                    BookingDate = session.StartDate.AddDays(-1),
                    IsAttended = session.EndDate < now      // attended only if the session already finished
                }, member, session));
            }

            foreach (var group in bookings.GroupBy(b => b.Session.Id))
            {
                var session = group.First().Session;
                if (group.Count() > session.Capacity)
                    problems.Add($"'{session.Description}' has {group.Count()} bookings but capacity {session.Capacity}");
            }

            foreach (var group in bookings.GroupBy(b => b.Member.Id))
            {
                var latestEnd = DateTime.MinValue;
                var latestEndDescription = "";

                foreach (var (_, member, session) in group.OrderBy(b => b.Session.StartDate))
                {
                    if (session.StartDate < latestEnd)
                        problems.Add($"{member.Name} is booked into overlapping sessions: '{session.Description}' and '{latestEndDescription}'");

                    if (session.EndDate > latestEnd)
                    {
                        latestEnd = session.EndDate;
                        latestEndDescription = session.Description;
                    }
                }
            }

            problems.ThrowIfAny();

            await dbContext.Bookings.AddRangeAsync(bookings.Select(b => b.Booking));
            await dbContext.SaveChangesAsync();
        }
    }
}
