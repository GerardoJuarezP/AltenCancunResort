using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltenCancunResort.Data.Entities
{
    public class ReservationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; private set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestID { get; set; }
        public bool Active { get; set; }
    }
}