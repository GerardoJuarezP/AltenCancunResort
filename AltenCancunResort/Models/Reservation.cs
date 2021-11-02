using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltenCancunResort.Models
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Created { get; set; }
        public int GuestID { get; set; }
        public bool Active { get; set; }
    }
}