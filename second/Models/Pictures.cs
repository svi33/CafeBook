using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.Models
{
    public class Pictures
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string TableID { get; set; }
        public string RestaurantID { get; set; }
        public string Description { get; set; }
        public string PathToImg { get; set; }

        public virtual Tables table { get; set; }
        public virtual Restaurants restaurant { get; set; }
    }
}
