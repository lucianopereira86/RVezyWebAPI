using RVezy.Domain.Domain.Entities;
using RVezy.Domain.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RVezy.Domain.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task CreateReviews(IEnumerable<Review> reviews, CancellationToken cancellationToken = default);
        Task<IEnumerable<Review>> GetReviews(PageOptions pageOptions);
    }
}