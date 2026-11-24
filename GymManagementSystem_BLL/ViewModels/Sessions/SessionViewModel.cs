using GymManagementSystem_BLL.Helpers;
using GymManagementSystem_DAL.Enums;
using System.Globalization;

namespace GymManagementSystem_BLL.ViewModels.Sessions
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = default!;
        public string TrainerName { get; set; } = default!;
        public string CategoryName { get; set; } = default!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Capacity { get; set; }
        public int BookedCount { get; set; }
        public SessionStatus Status { get; set; }

        public int AvailableSlots => Math.Max(Capacity - BookedCount, 0);
        public bool IsFull => AvailableSlots == 0;

        public string AvailabilityDisplay
            => IsFull ? "Full" : $"{AvailableSlots} of {Capacity} slots left";

        public string DateDisplay
            => StartDate.ToString(DateFormats.Display, CultureInfo.InvariantCulture);

        public string TimeRangeDisplay
            => $"{StartDate.ToString("hh:mm tt", CultureInfo.InvariantCulture)} - {EndDate.ToString("hh:mm tt", CultureInfo.InvariantCulture)}";

        public string StartDisplay
            => StartDate.ToString($"{DateFormats.Display}, hh:mm tt", CultureInfo.InvariantCulture);

        public string EndDisplay
            => EndDate.ToString($"{DateFormats.Display}, hh:mm tt", CultureInfo.InvariantCulture);

        public string DurationDisplay
        {
            get
            {
                var d = EndDate - StartDate;
                return d.TotalHours >= 1 ? $"{(int)d.TotalHours}h {d.Minutes}m" : $"{d.Minutes}m";
            }
        }
    }
}
