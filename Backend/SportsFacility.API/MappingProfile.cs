using AutoMapper;
using SportsFacility.Entity.Entities;
using SportsFacility.DTO;
using System.Linq;

namespace SportsFacility.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Facility <-> ActivityDto
            CreateMap<Facility, ActivityDto>()
                .ForMember(dest => dest.NumberOfCourts, opt => opt.MapFrom(src => src.Courts.Count))
                .ForMember(dest => dest.MaxMembersPerCourt, opt => opt.MapFrom(src => 4))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => "General"))
                .ForMember(dest => dest.Courts, opt => opt.Ignore());

            // SubscriptionPlan <-> MembershipPlanDto
            CreateMap<SubscriptionPlan, MembershipPlanDto>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.FacilityId.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.MembershipType))
                .ForMember(dest => dest.IsCouple, opt => opt.MapFrom(src => src.MaxMembers == 2))
                .ReverseMap()
                .ForMember(dest => dest.FacilityId, opt => opt.MapFrom(src => Guid.Parse(src.ActivityId)))
                .ForMember(dest => dest.MembershipType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.MaxMembers, opt => opt.MapFrom(src => src.IsCouple ? 2 : (src.Type == "Family" ? 4 : 1)));

            // UserMembership <-> MemberListDto
            CreateMap<UserMembership, MemberListDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.MobileNumber))
                .ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.SubscriptionPlan.Name))
                .ForMember(dest => dest.MembershipType, opt => opt.MapFrom(src => src.SubscriptionPlan.MembershipType));

            // ClassSchedule <-> ClassScheduleDto
            CreateMap<ClassSchedule, ClassScheduleDto>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.FullName))
                .ForMember(dest => dest.Attendance, opt => opt.MapFrom(src => src.Attendances.Count));

            // Booking <-> BookingListDto
            CreateMap<Booking, BookingListDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.FacilityName, opt => opt.MapFrom(src => src.Court.Facility.Name))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.PaymentMode, opt => opt.MapFrom(src => src.PaymentStatus));

            // Payment <-> PaymentListDto
            CreateMap<Payment, PaymentListDto>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.UTR, opt => opt.MapFrom(src => src.TransactionId));

            // User <-> StaffListDto
            CreateMap<User, StaffListDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.MobileNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
