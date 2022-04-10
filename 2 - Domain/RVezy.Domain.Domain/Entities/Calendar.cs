using System;

namespace RVezy.Domain.Domain.Entities
{
    public class Calendar
    {
        public Calendar(int listingId, DateTime date, bool available, float price)
        {
            ListingId = listingId;
            Date = date;
            Available = available;
            Price = price;
        }

        public int ListingId { get; private set; }
        public DateTime Date { get; private set; }
        public bool Available{ get; private set; }
        public float Price{ get; private set; }
    }
}
