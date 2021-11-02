using System;
using System.ComponentModel.DataAnnotations;

namespace AltenCancunResort.Models
{
    public class CheckAvailabilityInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}