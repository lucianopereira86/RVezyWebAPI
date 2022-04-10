using RVezy.Domain.Domain.Entities;
using RVezy.Domain.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RVezy.Domain.Domain.Interfaces
{
    public interface IListingRepository
    {
        Task CreateListings(IEnumerable<Listing> listings, CancellationToken cancellationToken = default);
        Task<IEnumerable<Listing>> GetListings(PageOptions pageOptions = null);
        Task<Listing> GetListingByListingId(int listingId);
        Task<IEnumerable<Listing>> GetListingsByPropertyType(string propertyType, PageOptions pageOptions = null);
    }
}
