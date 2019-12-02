using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace second.Models
{
    public class Restaurants
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Adres { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Site { get; set; }
        public string Description { get; set; }
        public string MoreOption { get; set; }

        public virtual ICollection<Hall> halls { get; set; }
        public virtual ICollection<Pictures> pictures { get; set; }
    }
}
