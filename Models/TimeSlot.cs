using System;
using System.Collections.Generic;

namespace ProjectPRN221_2.Models
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            Bookings = new HashSet<Booking>();
        }

        public int TimeSlotId { get; set; }
        public string? Slot { get; set; }
        public string? Available { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
