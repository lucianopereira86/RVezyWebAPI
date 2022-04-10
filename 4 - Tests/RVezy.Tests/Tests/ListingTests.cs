using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using RVezy.Domain.Domain.Entities;
using RVezy.Domain.Domain.Interfaces;
using RVezy.Domain.Domain.Mappers;
using RVezy.Domain.Domain.Models;
using RVezy.Infra.Infra.Mappers;
using RVezyWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using DomainListing = RVezy.Domain.Domain.Entities.Listing;
using InfraListing = RVezy.Infra.Infra.Entities.Listing;

namespace RVezy.Tests.Tests
{
    public class ListingTests
    {
        #region Facts
        [Fact]
        public async Task ShouldReturnPaginatedListOfListings()
        {
            #region Assert
            var services = new ServiceCollection();
            services.AddAutoMapper(Assembly.GetEntryAssembly());
            services.AddAutoMapper(typeof(AutoMapperProfileCsv));
            services.AddAutoMapper(typeof(AutoMapperProfileEf));
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();
            var mockRepository = new Mock<IListingRepository>();
            var mockLogger = new Mock<ILogger<ListingController>>();

            var listing = new DomainListing(1, "listingUrl", "name", "description", "propertyType");
            
            List<DomainListing> listings = new List<DomainListing>();
            listings.Add(listing);
            mockRepository.Setup(s => s.GetListings(It.IsAny<PageOptions>())).ReturnsAsync(listings);
            
            #endregion Assert

            #region  Act
            var mockController = new ListingController(mockLogger.Object, mapper, mockRepository.Object);
            var result = await mockController.GetListings(null);
            #endregion Act

            #region Assert
            var okresult = result as OkObjectResult;
            okresult.Value.Should().Be(listings);
            #endregion Assert
        }
        [Fact]
        public async Task ShouldReturnListingByListingId()
        {

            #region Assert
            var services = new ServiceCollection();
            services.AddAutoMapper(Assembly.GetEntryAssembly());
            services.AddAutoMapper(typeof(AutoMapperProfileCsv));
            services.AddAutoMapper(typeof(AutoMapperProfileEf));
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();
            var mockRepository = new Mock<IListingRepository>();
            var mockLogger = new Mock<ILogger<ListingController>>();

            var listing = new DomainListing(1, "listingUrl", "name", "description", "propertyType");

            mockRepository.Setup(s => s.GetListingByListingId(It.IsAny<int>())).ReturnsAsync(listing);

            #endregion Assert

            #region  Act
            var mockController = new ListingController(mockLogger.Object, mapper, mockRepository.Object);
            var result = await mockController.GetListingByListingId(1);
            #endregion Act

            #region Assert
            var okresult = result as OkObjectResult;
            okresult.Value.Should().Be(listing);
            #endregion Assert
        }
        [Fact]
        public async Task ShouldReturnListingsByPropertyType()
        {
            #region Assert
            var services = new ServiceCollection();
            services.AddAutoMapper(Assembly.GetEntryAssembly());
            services.AddAutoMapper(typeof(AutoMapperProfileCsv));
            services.AddAutoMapper(typeof(AutoMapperProfileEf));
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();
            var mockRepository = new Mock<IListingRepository>();
            var mockLogger = new Mock<ILogger<ListingController>>();

            var listing = new DomainListing(1, "listingUrl", "name", "description", "propertyType");

            List<DomainListing> listings = new List<DomainListing>();
            listings.Add(listing);
            mockRepository.Setup(s => s.GetListingsByPropertyType(It.IsAny<string>(), It.IsAny<PageOptions>())).ReturnsAsync(listings);

            #endregion Assert

            #region  Act
            var mockController = new ListingController(mockLogger.Object, mapper, mockRepository.Object);
            var result = await mockController.GetListingsByPropertyType("propertyType", null);
            #endregion Act

            #region Assert
            var okresult = result as OkObjectResult;
            okresult.Value.Should().Be(listings);
            #endregion Assert

        }
        #endregion Facts
    }
}
