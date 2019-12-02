using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using second.Models;
using second.ViewModels;

namespace second.Controllers
{
    public class DeskAddController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeskAddController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Changetable([FromBody] TableViewModel tables)
        {
            
            var listid = _context.TablesList.Where(t => t.HallID == tables.hall);
            if (listid.Count()>0)
            {
                foreach (Tables temp in listid)
                {
                    _context.TablesList.Remove(temp);
                }
            }
            if (tables.tables.Count>0) {
                foreach (Desk T in tables.tables)
                {
                    Tables Temp = new Tables
                    {
                        HallID = tables.hall,
                        Name=T.Name,
                        Description = T.Description,
                        CoordinateX = (int)T.CoordinateX,
                        CoordinateY = (int)T.CoordinateY,
                        MaxSit=1, MinSit=1
                    };
                    _context.TablesList.Add(Temp);
                }
            
            _context.SaveChanges();
            
            return Json(tables.tables[tables.tables.Count-1].Name.ToString());
            }
            else return Json("Error!!!");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTables([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tables = _context.TablesList.Where(t => t.HallID == id).ToList();

            if (tables == null)
            {
                return NotFound();
            }

            return Ok(tables);
        }


        [HttpPost]
        public ActionResult Checktable([FromBody] Check data)
        {
            if (data.bookdate == null || data.bookdate=="") return null;
            DateTime booking = Convert.ToDateTime(data.bookdate);
            var listtable = _context.TablesList.Where(t => t.HallID == data.hall);//.Select(v=>v.ID)
            List <string> booktable=new List<string>();
            var AllBooking = _context.Bookings.Where(b => (b.CheckIn- booking).TotalHours<1 && booking <= b.CheckOut );//.Select(V=>V.TableID) //&& b.CheckOut >= booking
            if (listtable!=null)
            {
               var X = AllBooking.Join(listtable,
                            A => A.TableID,
                            L => L.ID,
                            (A, L) => new { id = L.ID }
                            ).ToList();
                if (X != null)
                {
                    foreach (var I in X)
                    {
                        booktable.Add(I.id);
                    }
                }
            }
           // return Json(AllBooking.First().CheckIn);


 

            if (listtable.Count()!=0)
            { 
                return Json(booktable);
            }
            else return null;
        }


    }
}