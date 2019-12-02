using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace second.ViewModels
{
    public class TableBookingViewModel
    {
        public string TableID { get; set; }
        public DateTime CheckIn { get; set; }
        public int Guests { get; set; }
        public int Time { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerPhone { get; set; }
    }
}
