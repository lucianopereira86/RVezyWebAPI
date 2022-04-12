using System;

namespace RVezy.Domain.Domain.Models.Csv
{
    public class CalendarCsv
    {
        public int id { get; set; }
        public int listing_id { get; set; }
        public DateTime date { get; set; }
        public bool available { get; set; }
        public float price { get; set; }
    }
}
