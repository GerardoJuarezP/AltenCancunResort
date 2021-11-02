using System.ComponentModel.DataAnnotations.Schema;

namespace AltenCancunResort.Models
{
    public class Guest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuestID { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
    }
}