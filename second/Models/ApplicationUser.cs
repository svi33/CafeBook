using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string PersonPhone { get; set; }
        public string AppRole { get; set; }
        public string RestaurantID { get; set; }
    }
}
