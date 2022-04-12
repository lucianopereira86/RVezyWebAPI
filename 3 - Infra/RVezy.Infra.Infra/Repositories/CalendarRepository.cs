using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RVezy.Infra.Infra.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfraCalendar = RVezy.Infra.Infra.Entities.Calendar;
using DomainCalendar = RVezy.Domain.Domain.Entities.Calendar;
using System.Threading;
using RVezy.Domain.Domain.Models;
using RVezy.Domain.Domain.Interfaces;

namespace RVezy.Infra.Infra.Repositories
{
    public class CalendarRepository : BaseRepository, ICalendarRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CalendarRepository> _logger;
        public CalendarRepository(ApplicationDbContext context, IMapper mapper, ILogger<CalendarRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateCalendars(IEnumerable<DomainCalendar> calendars, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<IEnumerable<InfraCalendar>>(calendars);
            await _context.Calendars.AddRangeAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<DomainCalendar>> GetCalendars(PageOptions pageOptions = null)
        {
            var result = await Pagination(_context.Calendars.AsNoTracking(), pageOptions);
            return _mapper.Map<IEnumerable<DomainCalendar>>(result);
        }
    }
}
