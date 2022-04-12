using System;

namespace RVezy.Infra.Infra.Entities
{
    public class Review
    {
        public int ListingId { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string Comments { get; set; }

        public Listing Listing { get; set; }
    }
}
