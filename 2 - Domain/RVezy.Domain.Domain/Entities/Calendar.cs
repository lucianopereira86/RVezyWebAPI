using System;

namespace RVezy.Domain.Domain.Entities
{
    public class Calendar
    {
        public Calendar()
        {

        }

        public Calendar(int id, int listingId, DateTime date, bool available, float price)
        {
            Id = id;
            ListingId = listingId;
            Date = date;
            Available = available;
            Price = price;
        }

        public int Id { get; private set; }
        public int ListingId { get; private set; }
        public DateTime Date { get; private set; }
        public bool Available{ get; private set; }
        public float Price{ get; private set; }
    }
}
