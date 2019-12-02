using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using second.Models;
using second.ViewModels;

namespace second.Controllers
{
    
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager= userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET api/users/5
         //[HttpGet]
        // public IEnumerable<Tables> Get(string id)
         [HttpGet("{id}")]
         public ActionResult<Tables> Get(string id)
        {
           var tables = _context.TablesList.Where(t => t.HallID == id) ;
           if (tables == null) return NotFound();
           return new ObjectResult(tables.ToList());
           //return tables.ToList();
        }

        [HttpPost]
        public ActionResult BookingTable ([FromBody] TableBookingViewModel model)
        {
            if (model != null)
            {
                Booking newBooking = new Booking();
                newBooking.CheckIn = model.CheckIn;
                newBooking.CheckOut = model.CheckIn.AddHours(model.Time);
                newBooking.CustomerName = model.CustomerName;
                newBooking.CustomerPhone = model.CustomerPhone;
                newBooking.Guests = model.Guests;
                newBooking.Paid = false;
                newBooking.Completed = false;
                newBooking.DateCreated = DateTime.Now;
                newBooking.TotalFee = 0;
                newBooking.TableID = model.TableID;

                var BookingList = _context.Bookings;
                BookingList.Add(newBooking);
                _context.SaveChanges();
                string message = "Бронирование успешно завершено. Ближайшее время с Вами свяжется сотрудник ресторана для уточнения деталей.";
                return Json(message);
            }

            else return Json("Error");
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString(); 
            Booking _booking = await _context.Bookings.FirstOrDefaultAsync(p => p.ID == id); ;
            if (_booking != null)
            {
                string table = _context.TablesList.Where(t => t.ID == _booking.TableID).First().Name;
                ViewBag.TableName = table;
                return View(_booking);
               
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Booking model, string returnUrl)
        {
           if (ModelState.IsValid)
           {
                _context.Bookings.Update(model);
                await _context.SaveChangesAsync();

                
                return Redirect(returnUrl);
            }
            
            return View(model);
        }

        [Authorize]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();

            if (id != null)
            {
                Booking _booking = await _context.Bookings.FirstOrDefaultAsync(p => p.ID == id);
                string table = _context.TablesList.Where(t => t.ID == _booking.TableID).First().Name;
                ViewBag.TableName = table;
                if (_booking != null)
                    return View(_booking);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id, string returnUrl)
        {
            
            if (id != null)
            {
                Booking booking = await _context.Bookings.FirstOrDefaultAsync(p => p.ID == id);
                if (booking != null)
                {
                    _context.Bookings.Remove(booking);
                    await _context.SaveChangesAsync();
                    return Redirect("/Restaurant/Manage");
                }
            }
            return NotFound();
        }

        [Authorize]
        public IActionResult Create()
        {
            ApplicationUser user = new ApplicationUser();
            var users = _userManager.Users.ToList();

            if (User.Identity.IsAuthenticated)
            {
                user = users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            }
            if (user == null) return NotFound();
            var Restaraunt = _context.RestaurantsList.Where(t => t.ID == user.RestaurantID).Include(cafe => cafe.pictures).Include(cafe => cafe.halls).FirstOrDefault();
            if (Restaraunt == null) return View("/Home/Contact");
            var halls = _context.Halls.Where(h => h.RestaurantID == Restaraunt.ID);
            var tables = _context.TablesList.Join(halls, T => T.HallID,
                           H => H.ID,
                           (T, H) => new { id = T.ID, hallid = H.ID, tablename = T.Name, hallname = H.Name }
                           ).ToList();
            if (tables.Count < 1) return Redirect("/Restaurant/Index");
            SelectList Tables = new SelectList(tables, "id", "tablename");
            ViewBag.SelectTable = Tables;

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Booking model)
        {
            if (ModelState.IsValid)
            {
                _context.Bookings.Add(model);
                await _context.SaveChangesAsync();
                return Redirect("/Restaurant/Manage");
            }
            return View(model);
        }
    }
}