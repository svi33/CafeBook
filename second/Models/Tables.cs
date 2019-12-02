using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.Models
{
    public class Tables
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string HallID { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public string Description { get; set; }
        public int MaxSit { get; set; }
        public int MinSit { get; set; }

        public virtual Hall hall { get; set; }
        public virtual ICollection<Pictures> pictures { get; set; }
    }
}
