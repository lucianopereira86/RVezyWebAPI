using System;

namespace RVezy.Infra.Infra.Entities
{
    public class Calendar
    {
        public int ListingId { get; set; }
        public DateTime Date { get; set; }
        public bool Available{ get; set; }
        public float Price{ get; set; }
    }
}
