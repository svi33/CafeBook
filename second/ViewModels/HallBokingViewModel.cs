using second.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.ViewModels
{
    public class HallBokingViewModel
    {
        public Hall hall { get; set; }
        public Booking TempBooking { get; set; }
        //public List<Booking> TableBooking { get; set; }
        //public HallBokingViewModel()
        //{
        //    TableBooking = new List<Booking>();
        //}
    }
}
