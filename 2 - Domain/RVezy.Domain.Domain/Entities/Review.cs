using System;

namespace RVezy.Domain.Domain.Entities
{
    public class Review
    {
        public Review()
        {

        }
        public Review(int id, int listingId, DateTime date, int reviewerId, string reviewerName, string comments)
        {
            Id = id;
            ListingId = listingId;
            Date = date;
            ReviewerId = reviewerId;
            ReviewerName = reviewerName;
            Comments = comments;
        }

        public int Id { get; private set; }
        public int ListingId { get; private set; }
        public DateTime Date { get; private set; }
        public int ReviewerId { get; private set; }
        public string ReviewerName { get; private set; }
        public string Comments { get; private set; }
    }
}
