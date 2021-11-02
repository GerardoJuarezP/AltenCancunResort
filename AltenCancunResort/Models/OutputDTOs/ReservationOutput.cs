using System;
using System.ComponentModel.DataAnnotations;

namespace AltenCancunResort.Models
{
    public class ReservationOutput
    {
        public int ReservationID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestID { get; set; }
        public bool Active { get; set; }
    }
}