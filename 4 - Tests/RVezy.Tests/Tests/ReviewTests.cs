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
using DomainReview = RVezy.Domain.Domain.Entities.Review;
using InfraReview = RVezy.Infra.Infra.Entities.Review;
using RVezy.Infra.Infra.Repositories;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace RVezy.Tests.Tests
{
    public class ReviewTests
    {
        #region GET
        #region Controller
        [Fact]
        public async Task ShouldReturnListOfReviewsFromController()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            var mockRepository = new Mock<IReviewRepository>();
            var mockLogger = new Mock<ILogger<ReviewController>>();

            var review = new DomainReview(1, 1, DateTime.Now, 1, "reviewer_name", "reviewer_comments");

            List<DomainReview> reviews = new List<DomainReview>();
            reviews.Add(review);
            mockRepository.Setup(s => s.GetReviews(It.IsAny<PageOptions>())).ReturnsAsync(reviews);

            #endregion Arrange

            #region  Act
            var mockController = new ReviewController(mockLogger.Object, mapper, mockRepository.Object);
            var result = await mockController.GetReviews(null);
            #endregion Act

            #region Assert
            var okresult = result as OkObjectResult;
            okresult.Value.Should().Be(reviews);
            #endregion Assert
        }
        #endregion Controller
        #region Repository
        [Fact]
        public async Task ShouldReturnListOfReviewsFromRepository()
        {
            #region Arrange
            IMapper mapper = GetMapper();
            List<InfraListing> listings = CreateListings();
            List<InfraReview> reviews = CreateReviews(listings);
            var dbContext = await SetupDbContext(reviews);
            var mockLogger = new Mock<ILogger<ReviewRepository>>();
            var reviewRepository = new ReviewRepository(dbContext, mapper, mockLogger.Object);
            #endregion Arrange

            #region  Act
            var domainReviews = await reviewRepository.GetReviews();
            #endregion Act

            #region Assert
            var infraReviews = mapper.Map<IEnumerable<InfraReview>>(domainReviews);
            reviews.ToList().ForEach(e => e.Listing = null);
            reviews.Should().BeEquivalentTo(infraReviews);
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
            var mockRepository = new Mock<IReviewRepository>();
            var mockLogger = new Mock<ILogger<ReviewController>>();

            var mockController = new ReviewController(mockLogger.Object, mapper, mockRepository.Object);
            string fileName = "reviews.csv";
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
        private static List<InfraReview> CreateReviews(List<InfraListing> listings)
        {
            return new List<InfraReview>
            {
                new InfraReview { Id = 1, ListingId = 1, Date = DateTime.Now, ReviewerId = 1, ReviewerName = "reviewer_name1", Comments = "reviewer_comments1", Listing = listings.First(f => f.Id == 1) },
                new InfraReview { Id = 2, ListingId = 1, Date = DateTime.Now.AddDays(1), ReviewerId = 2, ReviewerName = "reviewer_name2", Comments = "reviewer_comments2", Listing = listings.First(f => f.Id == 1) },
                new InfraReview { Id = 3, ListingId = 2, Date = DateTime.Now.AddHours(5), ReviewerId = 3, ReviewerName = "reviewer_name3", Comments = "reviewer_comments3",  Listing = listings.First(f => f.Id == 2) }
            };
        }

        private static async Task<ApplicationDbContext> SetupDbContext(List<InfraReview> reviews)
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            var dbContext = new ApplicationDbContext(options);
            await dbContext.Reviews.AddRangeAsync(reviews);
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
