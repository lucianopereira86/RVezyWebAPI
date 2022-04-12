using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RVezy.Domain.Domain.Entities;
using RVezy.Domain.Domain.Interfaces;
using RVezy.Domain.Domain.Models;
using RVezy.Domain.Domain.Models.Csv;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RVezyWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {

        private readonly ILogger<ReviewController> _logger;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(ILogger<ReviewController> logger, IMapper mapper, IReviewRepository reviewRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        #region GET
        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] PageOptions pageOptions)
        {
            var result = await _reviewRepository.GetReviews(pageOptions);
            _logger.LogInformation($"GetReviews => result: {JsonConvert.SerializeObject(result)}");
            return Ok(result);
        }
        #endregion GET

        #region POST
        [HttpPost("upload")]
        public async Task<IActionResult> PostUpload(IFormFile formFile)
        {
            if (formFile.Length == 0)
                throw new System.Exception("File is empty");
            
            using var memoryStream = new MemoryStream(new byte[formFile.Length]);
            await formFile.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var reader = new StreamReader(memoryStream);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null
            };
            using var csvReader = new CsvReader(reader, config);
            var records = csvReader.GetRecords<ReviewCsv>().ToList();
            var reviews = _mapper.Map<IEnumerable<Review>>(records);
            if (!reviews.Any())
                throw new System.Exception("No data obtained from file");
            await _reviewRepository.CreateReviews(reviews);
            
            return Ok();
        }
        #endregion POST
    }
}
