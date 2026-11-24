using GymManagementSystem_DAL.Enums;
using GymManagementSystem_DAL.Models.ValueObjects;

namespace GymManagementSystem_DAL.Data.Seed
{
    internal record HealthRecordSeed(decimal Height, decimal Weight, BloodType BloodType, string? Note);

    internal record MemberSeed(string Name, string Email, string Phone, DateOnly DateOfBirth, Gender Gender,
        Address Address, int? JoinedMonthsAgo, HealthRecordSeed HealthRecord);

    internal record TrainerSeed(string Name, string Email, string Phone, DateOnly DateOfBirth, Gender Gender,
        Address Address, Speciality Speciality, int HiredMonthsAgo);

    internal record MembershipSeed(string MemberEmail, string Plan, int StartOffsetDays);

    internal record SessionSeed(string Description, string Category, string TrainerEmail,
        double StartOffsetHours, int DurationMinutes, int Capacity);

    internal record BookingSeed(string MemberEmail, string Session);
}
