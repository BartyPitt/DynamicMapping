using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMapping.Models
{
    /// <summary>
    /// A class containing the reservation infomation.
    /// </summary>
    public class Reservation
    {
        public string? ClientName { get; set; }
        public int RoomNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
