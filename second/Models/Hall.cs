using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.Models
{
    public class Hall
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string RestaurantID { get; set; }
        public string Description { get; set; }

        public virtual Restaurants restaurant { get; set; }
        public virtual ICollection<Tables> tables { get; set; }
    }
}
