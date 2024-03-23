using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectPRN221_2.Models
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        [Required(ErrorMessage = "Customer Name is required")]
        public string? CustomerName { get; set; }
        public int? FieldId { get; set; }
        public int? TimeSlotId { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime? Date { get; set; }
        public string? Status { get; set; } = "0";
        [Required(ErrorMessage = "Customer Phone is required.")]
        [StringLength(10, ErrorMessage = "Customer Phone must be 10 characters.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Customer Phone must be numeric.")]
        public string? CustomerPhone { get; set; }
        public string? Message { get; set; }

        public virtual FootballField? Field { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? DateOnly => Date?.Date;
    }
}
