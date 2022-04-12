using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RVezy.Domain.Domain.Interfaces;
using RVezy.Domain.Domain.Models;
using RVezy.Domain.Domain.Models.Csv;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace RVezyWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {

        private readonly ILogger<CalendarController> _logger;
        private readonly IMapper _mapper;
        private readonly ICalendarRepository _calendarRepository;

        public CalendarController(ILogger<CalendarController> logger, IMapper mapper, ICalendarRepository calendarRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _calendarRepository = calendarRepository;
        }

        #region GET
        [HttpGet]
        public async Task<IActionResult> GetCalendars([FromQuery] PageOptions pageOptions)
        {
            var result = await _calendarRepository.GetCalendars(pageOptions);
            _logger.LogInformation($"GetCalendars => result: {JsonConvert.SerializeObject(result)}");
            return Ok(result);
        }
        #endregion GET

        #region POST
        [HttpPost("upload")]
        public async Task<IActionResult> PostUpload(IFormFile formFile)
        {
            if (formFile.Length > 0)
            {
                var filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    await formFile.CopyToAsync(stream);
                    using (var reader = new StreamReader(stream))
                    {
                        var content = reader.ReadToEnd();
                        using (TextReader textReader = new StringReader(content))
                        {
                            using (var csv = new CsvReader(textReader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                            {
                                var calendarsCsv = csv.GetRecords<CalendarCsv>();
                                var calendars = _mapper.Map<IEnumerable<RVezy.Domain.Domain.Entities.Calendar>>(calendarsCsv);
                                await _calendarRepository.CreateCalendars(calendars);
                            }
                        }
                    }
                }
            }

            return Ok();
        }
        #endregion POST
    }
}
