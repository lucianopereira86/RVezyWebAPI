using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RVezy.Domain.Domain.Interfaces;
using RVezy.Infra.Infra.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfraListing = RVezy.Infra.Infra.Entities.Listing;
using DomainListing = RVezy.Domain.Domain.Entities.Listing;
using System.Threading;
using RVezy.Domain.Domain.Models;

namespace RVezy.Infra.Infra.Repositories
{
    public class ListingRepository : BaseRepository, IListingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingRepository> _logger;
        public ListingRepository(ApplicationDbContext context, IMapper mapper, ILogger<ListingRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateListings(IEnumerable<DomainListing> listings, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<IEnumerable<InfraListing>>(listings);
            await _context.Listings.AddRangeAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<DomainListing>> GetListings(PageOptions pageOptions = null)
        {
            var result = (await Pagination(_context.Listings.AsNoTracking(), pageOptions)).ToList();
            return _mapper.Map<IEnumerable<DomainListing>>(result);
        }

        public async Task<DomainListing> GetListingByListingId(int listingId)
        {
            var result = await _context.Listings.AsNoTracking().FirstOrDefaultAsync(f => f.Id == listingId);
            return _mapper.Map<DomainListing>(result);
        }

        public async Task<IEnumerable<DomainListing>> GetListingsByPropertyType(string propertyType, PageOptions pageOptions = null)
        {
            var result = (await Pagination(_context.Listings.AsNoTracking().Where(w => w.PropertyType == propertyType), pageOptions)).ToList();
            return _mapper.Map<IEnumerable<DomainListing>>(result);
        }
    }
}
