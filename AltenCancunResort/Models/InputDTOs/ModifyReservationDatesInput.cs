using System;

namespace AltenCancunResort.Models
{
    public class ModifyReservationDatesInput
    {
        public int ReservationID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}