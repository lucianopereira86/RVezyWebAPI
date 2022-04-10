using RVezy.Domain.Domain.Entities;
using RVezy.Domain.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RVezy.Domain.Domain.Interfaces
{
    public interface ICalendarRepository
    {
        Task CreateCalendars(IEnumerable<Calendar> calendars, CancellationToken cancellationToken = default);
        Task<IEnumerable<Calendar>> GetCalendars(PageOptions pageOptions);
    }
}