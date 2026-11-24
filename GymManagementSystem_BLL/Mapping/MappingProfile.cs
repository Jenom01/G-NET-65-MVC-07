using AutoMapper;
using GymManagementSystem_BLL.Helpers;
using GymManagementSystem_BLL.ViewModels.HealthRecords;
using GymManagementSystem_BLL.ViewModels.Members;
using GymManagementSystem_BLL.ViewModels.Plans;
using GymManagementSystem_BLL.ViewModels.Sessions;
using GymManagementSystem_BLL.ViewModels.Trainers;
using GymManagementSystem_DAL.Models;
using GymManagementSystem_DAL.Models.ValueObjects;

namespace GymManagementSystem_BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ---------- Plans ----------
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, PlanEditViewModel>()
                .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Name));
            CreateMap<PlanEditViewModel, Plan>(MemberList.None)
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description.Trim()));

            // ---------- Trainers ----------
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(d => d.Speciality, o => o.MapFrom(s => s.Speciality.GetDisplayName()));

            CreateMap<Trainer, TrainerDetailsViewModel>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
                .ForMember(d => d.Speciality, o => o.MapFrom(s => s.Speciality.GetDisplayName()))
                .ForMember(d => d.Address, o => o.MapFrom(
                    s => $"{s.Address.BuildingNumber} - {s.Address.Street} - {s.Address.City}"));

            CreateMap<Trainer, TrainerEditViewModel>()
                .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));

            CreateMap<TrainerCreateViewModel, Trainer>(MemberList.None)
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
                .ForMember(d => d.Address, o => o.MapFrom(s => new Address
                {
                    City = s.City.Trim(),
                    Street = s.Street.Trim(),
                    BuildingNumber = s.BuildingNumber!.Value
                }));

            CreateMap<TrainerEditViewModel, Trainer>(MemberList.None)
                .ForMember(d => d.Name, o => o.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Address.BuildingNumber = s.BuildingNumber!.Value;
                    d.Address.Street = s.Street.Trim();
                    d.Address.City = s.City.Trim();
                });

            // ---------- Members ----------
            CreateMap<Member, MemberIndexViewModel>()
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photo))
                .ForMember(d => d.JoinDate, o => o.MapFrom(s => s.JoinDate ?? s.CreatedAt));

            CreateMap<Member, MemberEditViewModel>()
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photo))
                .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));

            CreateMap<MemberCreateViewModel, Member>(MemberList.None)
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
                .ForMember(d => d.Address, o => o.MapFrom(s => new Address
                {
                    City = s.City.Trim(),
                    Street = s.Street.Trim(),
                    BuildingNumber = s.BuildingNumber!.Value
                }));

            CreateMap<MemberEditViewModel, Member>(MemberList.None)
                .ForMember(d => d.Name, o => o.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Address.BuildingNumber = s.BuildingNumber!.Value;
                    d.Address.Street = s.Street.Trim();
                    d.Address.City = s.City.Trim();
                });

            // ---------- Health records ----------
            CreateMap<HealthRecordCreateViewModel, HealthRecord>(MemberList.None)
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note == null ? null : s.Note.Trim()));

            CreateMap<HealthRecord, HealthRecordDetailsViewModel>()
                .ForMember(d => d.BloodType, o => o.MapFrom(s => s.BloodType.GetDisplayName()))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note ?? "No Notes"));

            // ---------- Sessions ----------
            CreateMap<Session, SessionViewModel>()
                .ForMember(d => d.TrainerName, o => o.MapFrom(s => s.Trainer.Name))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.BookedCount, o => o.MapFrom(s => s.Bookings.Count))
                .ForMember(d => d.Status, o => o.Ignore());

            CreateMap<SessionCreateViewModel, Session>()
                .ForMember(d => d.StartDate, o => o.Ignore())
                .ForMember(d => d.EndDate, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.LastUpdatedAt, o => o.Ignore())
                .ForMember(d => d.DeletedAt, o => o.Ignore())
                .ForMember(d => d.IsDeleted, o => o.Ignore())
                .ForMember(d => d.Trainer, o => o.Ignore())
                .ForMember(d => d.Category, o => o.Ignore())
                .ForMember(d => d.Bookings, o => o.Ignore());

            CreateMap<Session, SessionEditViewModel>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.StartDate, o => o.Ignore())
                .ForMember(d => d.EndDate, o => o.Ignore());
        }
    }
}
