using System;

namespace RVezy.Domain.Domain.Entities
{
    public class Review
    {
        public Review(int listingId, int id, DateTime date, int reviewerId, string reviewerName, string comments)
        {
            ListingId = listingId;
            Id = id;
            Date = date;
            ReviewerId = reviewerId;
            ReviewerName = reviewerName;
            Comments = comments;
        }

        public int ListingId { get; private set; }
        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public int ReviewerId { get; private set; }
        public string ReviewerName { get; private set; }
        public string Comments { get; private set; }
    }
}
