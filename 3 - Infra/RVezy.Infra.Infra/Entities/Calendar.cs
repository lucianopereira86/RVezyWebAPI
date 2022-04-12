using System;

namespace RVezy.Infra.Infra.Entities
{
    public class Calendar
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public DateTime Date { get; set; }
        public bool Available{ get; set; }
        public float Price{ get; set; }
        
        public virtual Listing Listing { get; set; }
    }
}
