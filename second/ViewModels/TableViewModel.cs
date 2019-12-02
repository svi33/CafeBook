using second.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.ViewModels
{
    public class TableViewModel
    {
        public string hall { get; set; }
        public List<Desk> tables { get; set; }
        
    }

    public class Desk
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double CoordinateX { get; set; } 
        public double CoordinateY { get; set; } 
       
     
    }

    public class Check
    {
        public string hall { get; set; }
        public string bookdate { get; set; }
    }
}
