using second.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace second.ViewModels
{
    public class ManageBookingViewModel
    {
       // public string RestarauntID { get; set; }
        public string BookingID { get; set; }
        public string TableID { get; set; }
        public string TableName { get; set; }
        public string HallID { get; set; }
        public string HallName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Guests { get; set; }
        public bool Completed { get; set; }
        public bool Paid { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

       // public string OtherRequests { get; set; }

    }

    public class HallSimple
    {
        public string HallName { get; set; }
        public string HallID { get; set; }

    }
}
