using AutoMapper;
using RVezy.Domain.Domain.Entities;
using RVezy.Domain.Domain.Models.Csv;

namespace RVezy.Domain.Domain.Mappers
{
    public class AutoMapperProfileCsv : Profile
    {
        public AutoMapperProfileCsv()
        {
            CreateMap<ListingCsv, Listing>()
                .ForMember(f => f.Id, opt => opt.MapFrom(m => m.id))
                .ForMember(f => f.ListingUrl, opt => opt.MapFrom(m => m.listing_url))
                .ForMember(f => f.Name, opt => opt.MapFrom(m => m.name))
                .ForMember(f => f.Description, opt => opt.MapFrom(m => m.description))
                .ForMember(f => f.PropertyType, opt => opt.MapFrom(m => m.property_type));
            CreateMap<CalendarCsv, Calendar>()
                .ForMember(f => f.Id, opt => opt.MapFrom(m => m.id))
                .ForMember(f => f.Available, opt => opt.MapFrom(m => m.available))
                .ForMember(f => f.Date, opt => opt.MapFrom(m => m.date))
                .ForMember(f => f.ListingId, opt => opt.MapFrom(m => m.listing_id))
                .ForMember(f => f.Price, opt => opt.MapFrom(m => m.price));
            CreateMap<ReviewCsv, Review>()
                .ForMember(f => f.Comments, opt => opt.MapFrom(m => m.comments))
                .ForMember(f => f.Date, opt => opt.MapFrom(m => m.date))
                .ForMember(f => f.Id, opt => opt.MapFrom(m => m.id))
                .ForMember(f => f.ListingId, opt => opt.MapFrom(m => m.listing_id))
                .ForMember(f => f.ReviewerId, opt => opt.MapFrom(m => m.reviewer_id))
                .ForMember(f => f.ReviewerName, opt => opt.MapFrom(m => m.reviewer_name));
        }
    }
}
