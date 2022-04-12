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
    public class ListingController : ControllerBase
    {

        private readonly ILogger<ListingController> _logger;
        private readonly IMapper _mapper;
        private readonly IListingRepository _listingRepository;

        public ListingController(ILogger<ListingController> logger, IMapper mapper, IListingRepository listingRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _listingRepository = listingRepository;
        }

        #region GET
        [HttpGet]
        public async Task<IActionResult> GetListings([FromQuery] PageOptions pageOptions)
        {
            var result = await _listingRepository.GetListings(pageOptions);
            _logger.LogInformation($"GetListings => result: {JsonConvert.SerializeObject(result)}");
            return Ok(result);
        }

        [HttpGet("{listingId}")]
        public async Task<IActionResult> GetListingByListingId([FromRoute] int listingId)
        {
            _logger.LogInformation($"GetListingByListingId => listingId: {listingId}");
            var result = await _listingRepository.GetListingByListingId(listingId);
            _logger.LogInformation($"GetListingByListingId => result: {JsonConvert.SerializeObject(result)}");
            return Ok(result);
        }

        [HttpGet("propertytypes/{propertyType}")]
        public async Task<IActionResult> GetListingsByPropertyType([FromRoute] string propertyType, [FromQuery] PageOptions pageOptions)
        {
            _logger.LogInformation($"GetListingsByPropertyType => propertyType: {propertyType}");
            var result = await _listingRepository.GetListingsByPropertyType(propertyType, pageOptions);
            _logger.LogInformation($"GetListingsByPropertyType => result: {JsonConvert.SerializeObject(result)}");
            return Ok(result);
        }
        #endregion GET

        #region POST
        [HttpPost("upload")]
        public async Task<IActionResult> PostUpload(IFormFile formFile)
        {
            if (formFile.Length > 0)
            {
                using var memoryStream = new MemoryStream(new byte[formFile.Length]);
                await formFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using var reader = new StreamReader(memoryStream);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null
                };
                using var csvReader = new CsvReader(reader, config);
                var records = csvReader.GetRecords<ListingCsv>().ToList();
                var listings = _mapper.Map<IEnumerable<Listing>>(records);
                if (!listings.Any())
                    throw new System.Exception("No data obtained from file");
                await _listingRepository.CreateListings(listings);
            }

            return Ok();
        }
        #endregion POST
    }
}
