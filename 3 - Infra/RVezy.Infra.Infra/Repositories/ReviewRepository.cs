using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RVezy.Infra.Infra.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfraReview = RVezy.Infra.Infra.Entities.Review;
using DomainReview = RVezy.Domain.Domain.Entities.Review;
using System.Threading;
using RVezy.Domain.Domain.Models;
using RVezy.Domain.Domain.Interfaces;

namespace RVezy.Infra.Infra.Repositories
{
    public class ReviewRepository : BaseRepository, IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewRepository> _logger;
        public ReviewRepository(ApplicationDbContext context, IMapper mapper, ILogger<ReviewRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateReviews(IEnumerable<DomainReview> reviews, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<IEnumerable<InfraReview>>(reviews);
            await _context.Reviews.AddRangeAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<DomainReview>> GetReviews(PageOptions pageOptions)
        {
            var result = await Pagination(_context.Reviews.AsNoTracking(), pageOptions);
            return _mapper.Map<IEnumerable<DomainReview>>(result);
        }
    }
}
