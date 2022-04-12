using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.EntityFrameworkCore;
using RVezy.Domain.Domain.Interfaces;
using RVezy.Domain.Domain.Mappers;
using RVezy.Domain.Domain.Models;
using RVezy.Infra.Infra.Context;
using RVezy.Infra.Infra.Mappers;
using RVezyWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using DomainListing = RVezy.Domain.Domain.Entities.Listing;
using InfraListing = RVezy.Infra.Infra.Entities.Listing;
using RVezy.Infra.Infra.Repositories;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace RVezy.Tests.Tests
{
    public class ListingTests
    {
        #region GET
        #region Controller
        [Fact]
        public async Task ShouldReturnListOfListingsFromController()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            var mockRepository = new Mock<IListingRepository>();
            var mockLogger = new Mock<ILogger<ListingController>>();

            var listing = new DomainListing(1, "listingUrl", "name", "description", "propertyType");

            List<DomainListing> listings = new List<DomainListing>();
            listings.Add(listing);
            mockRepository.Setup(s => s.GetListings(It.IsAny<PageOptions>())).ReturnsAsync(listings);

            #endregion Arrange

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
        public async Task ShouldReturnListingByListingIdFromController()
        {

            #region Arrange
            IMapper mapper = GetMapper();
            var mockRepository = new Mock<IListingRepository>();
            var mockLogger = new Mock<ILogger<ListingController>>();

            var listing = new DomainListing(1, "listingUrl", "name", "description", "propertyType");

            mockRepository.Setup(s => s.GetListingByListingId(It.IsAny<int>())).ReturnsAsync(listing);

            #endregion Arrange

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
        public async Task ShouldReturnListingsByPropertyTypeFromController()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            var mockRepository = new Mock<IListingRepository>();
            var mockLogger = new Mock<ILogger<ListingController>>();

            var listing = new DomainListing(1, "listingUrl", "name", "description", "propertyType");

            List<DomainListing> listings = new List<DomainListing>();
            listings.Add(listing);
            mockRepository.Setup(s => s.GetListingsByPropertyType(It.IsAny<string>(), It.IsAny<PageOptions>())).ReturnsAsync(listings);

            #endregion Arrange

            #region  Act
            var mockController = new ListingController(mockLogger.Object, mapper, mockRepository.Object);
            var result = await mockController.GetListingsByPropertyType("propertyType", null);
            #endregion Act

            #region Assert
            var okresult = result as OkObjectResult;
            okresult.Value.Should().Be(listings);
            #endregion Assert

        }
        #endregion Controller
        #region Repository
        [Fact]
        public async Task ShouldReturnListOfListingsFromRepository()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            List<InfraListing> listings = CreateListings();
            var dbContext = await SetupDbContext(listings);
            var mockLogger = new Mock<ILogger<ListingRepository>>();
            var listingRepository = new ListingRepository(dbContext, mapper, mockLogger.Object);
            #endregion Arrange

            #region  Act
            var domainListings = await listingRepository.GetListings();
            #endregion Act

            #region Assert
            var infraListings = mapper.Map<IEnumerable<InfraListing>>(domainListings);
            listings.Should().BeEquivalentTo(infraListings);
            #endregion Assert
        }

        [Fact]
        public async Task ShouldReturnListingByListingIdFromRepository()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            List<InfraListing> listings = CreateListings();
            var dbContext = await SetupDbContext(listings);
            var mockLogger = new Mock<ILogger<ListingRepository>>();
            var listingRepository = new ListingRepository(dbContext, mapper, mockLogger.Object);
            int listingId = 1;
            #endregion Arrange

            #region  Act
            var domainListing = await listingRepository.GetListingByListingId(listingId);
            #endregion Act

            #region Assert
            Assert.Equal(listingId, domainListing.Id);
            #endregion Assert
        }
        [Fact]
        public async Task ShouldReturnListingsByPropertyTypeFromRepository()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            List<InfraListing> listings = CreateListings();
            var dbContext = await SetupDbContext(listings);
            var mockLogger = new Mock<ILogger<ListingRepository>>();
            var listingRepository = new ListingRepository(dbContext, mapper, mockLogger.Object);
            string propertyType = "type_A";
            #endregion Arrange

            #region  Act
            var domainListings = await listingRepository.GetListingsByPropertyType(propertyType);
            #endregion Act

            #region Assert
            int countType = listings.Count(c => c.PropertyType == propertyType);
            Assert.Equal(domainListings.Count(), countType);
            #endregion Assert
        }
        #endregion Repository
        #endregion GET

        #region POST
        #region Controller
        [Fact]
        public async Task ShouldUploadFileFromController()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            var mockRepository = new Mock<IListingRepository>();
            var mockLogger = new Mock<ILogger<ListingController>>();

            var mockController = new ListingController(mockLogger.Object, mapper, mockRepository.Object);
            string fileName = "listings.csv";
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + $"\\Files\\{fileName}";
            using var stream = new MemoryStream(File.ReadAllBytes(filePath).ToArray());
            var formFile = new FormFile(stream, 0, stream.Length, fileName, filePath.Split(@"\").Last());
            #endregion Arrange

            #region  Act
            var result = await mockController.PostUpload(formFile);
            #endregion Act

            #region Assert
            result.GetType().Should().Be(typeof(OkResult));
            #endregion Assert
        }
        #endregion Controller
        #endregion POST

        #region Private Methods 
        private static List<InfraListing> CreateListings()
        {
            return new List<InfraListing>
            {
                new InfraListing { Id = 1, ListingUrl = "http://url1", Name = "listing_name1", Description = "listing_description1", PropertyType = "type_A" },
                new InfraListing { Id = 2, ListingUrl = "http://url2", Name = "listing_name2", Description = "listing_description2", PropertyType = "type_A" },
                new InfraListing { Id = 3, ListingUrl = "http://url3", Name = "listing_name3", Description = "listing_description3", PropertyType = "type_B" }
            };
        }

        private static async Task<ApplicationDbContext> SetupDbContext(List<InfraListing> listings)
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            var dbContext = new ApplicationDbContext(options);
            dbContext.Listings.AddRange(listings);
            dbContext.SaveChanges();

            return await Task.FromResult(dbContext);
        }

        private static IMapper GetMapper()
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(Assembly.GetEntryAssembly());
            services.AddAutoMapper(typeof(AutoMapperProfileCsv));
            services.AddAutoMapper(typeof(AutoMapperProfileEf));
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();
            return mapper;
        }
        #endregion Private Methods 
    }
}
