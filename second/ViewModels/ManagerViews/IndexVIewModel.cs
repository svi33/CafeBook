using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.ViewModels.ManagerViews
{
    public class IndexVIewModel
    {

        public IEnumerable<ManageBookingViewModel> Bookings { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}
