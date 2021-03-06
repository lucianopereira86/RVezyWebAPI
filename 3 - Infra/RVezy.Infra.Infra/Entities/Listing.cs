using System.Collections.Generic;

namespace RVezy.Infra.Infra.Entities
{
    public class Listing
    {
        public int Id { get; set; }
        public string ListingUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PropertyType { get; set; }

        public IEnumerable<Calendar> Calendars { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
