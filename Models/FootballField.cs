using System;
using System.Collections.Generic;

namespace ProjectPRN221_2.Models
{
    public partial class FootballField
    {
        public FootballField()
        {
            Bookings = new HashSet<Booking>();
        }

        public int FieldId { get; set; }
        public string? FieldName { get; set; }
        public string? Address { get; set; }
        public string? Size { get; set; }
        public string? Image { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
