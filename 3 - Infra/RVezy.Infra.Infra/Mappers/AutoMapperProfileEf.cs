using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainListing = RVezy.Domain.Domain.Entities.Listing;
using DomainCalendar = RVezy.Domain.Domain.Entities.Calendar;
using DomainReview = RVezy.Domain.Domain.Entities.Review;
using InfraListing = RVezy.Infra.Infra.Entities.Listing;
using InfraCalendar = RVezy.Infra.Infra.Entities.Calendar;
using InfraReview = RVezy.Infra.Infra.Entities.Review;

namespace RVezy.Infra.Infra.Mappers
{
    public class AutoMapperProfileEf: Profile
    {
        public AutoMapperProfileEf()
        {
            CreateMap<DomainListing, InfraListing>().ReverseMap();
            CreateMap<DomainCalendar, InfraCalendar>().ReverseMap();
            CreateMap<DomainReview, InfraReview>().ReverseMap();
        }
    }
}
