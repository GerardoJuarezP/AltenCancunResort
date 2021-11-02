using System;

namespace AltenCancunResort.Models
{
    public class CreateReservationInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestID { get; set; }
    }
}