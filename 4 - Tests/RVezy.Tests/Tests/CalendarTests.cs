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
using InfraListing = RVezy.Infra.Infra.Entities.Listing;
using DomainCalendar = RVezy.Domain.Domain.Entities.Calendar;
using InfraCalendar = RVezy.Infra.Infra.Entities.Calendar;
using RVezy.Infra.Infra.Repositories;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace RVezy.Tests.Tests
{
    public class CalendarTests
    {
        #region GET
        #region Controller
        [Fact]
        public async Task ShouldReturnListOfCalendarsFromController()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            var mockRepository = new Mock<ICalendarRepository>();
            var mockLogger = new Mock<ILogger<CalendarController>>();

            var calendar = new DomainCalendar(1, 1, DateTime.Now, true, 10.00f);

            List<DomainCalendar> calendars = new List<DomainCalendar>();
            calendars.Add(calendar);
            mockRepository.Setup(s => s.GetCalendars(It.IsAny<PageOptions>())).ReturnsAsync(calendars);

            #endregion Arrange

            #region  Act
            var mockController = new CalendarController(mockLogger.Object, mapper, mockRepository.Object);
            var result = await mockController.GetCalendars(null);
            #endregion Act

            #region Assert
            var okresult = result as OkObjectResult;
            okresult.Value.Should().Be(calendars);
            #endregion Assert
        }
        #endregion Controller
        #region Repository
        [Fact]
        public async Task ShouldReturnListOfCalendarsFromRepository()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            List<InfraListing> listings = CreateListings();
            List<InfraCalendar> calendars = CreateCalendars(listings);
            var dbContext = await SetupDbContext(calendars);
            var mockLogger = new Mock<ILogger<CalendarRepository>>();
            var calendarRepository = new CalendarRepository(dbContext, mapper, mockLogger.Object);
            #endregion Arrange

            #region  Act
            var domainCalendars = await calendarRepository.GetCalendars();
            #endregion Act

            #region Assert
            var infraCalendars = mapper.Map<IEnumerable<InfraCalendar>>(domainCalendars);
            calendars.ToList().ForEach(e => e.Listing = null);
            calendars.Should().BeEquivalentTo(infraCalendars);
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
            var mockRepository = new Mock<ICalendarRepository>();
            var mockLogger = new Mock<ILogger<CalendarController>>();

            var mockController = new CalendarController(mockLogger.Object, mapper, mockRepository.Object);
            string fileName = "calendars.csv";
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
        private static List<InfraCalendar> CreateCalendars(List<InfraListing> listings)
        {
            return new List<InfraCalendar>
            {
                new InfraCalendar { Id = 1, ListingId = 1, Date = DateTime.Now, Available = true, Price = 100, Listing = listings.First(f => f.Id == 1) },
                new InfraCalendar { Id = 2, ListingId = 1, Date = DateTime.Now.AddDays(1), Available = true, Price = 20, Listing = listings.First(f => f.Id == 1) },
                new InfraCalendar { Id = 3, ListingId = 2, Date = DateTime.Now.AddHours(5), Available = false, Price = 55, Listing = listings.First(f => f.Id == 2) }
            };
        }

        private static async Task<ApplicationDbContext> SetupDbContext(List<InfraCalendar> calendars)
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            var dbContext = new ApplicationDbContext(options);
            await dbContext.Calendars.AddRangeAsync(calendars);
            await dbContext.SaveChangesAsync();

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
