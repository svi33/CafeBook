using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using second.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using second.ViewModels;

namespace second.Controllers
{
    [Authorize]
    public class HallsController : Controller
    {
        private readonly ApplicationDbContext _context;
        IHostingEnvironment _appEnvironment;
        UserManager<ApplicationUser> _userManager;

        public HallsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index(string id)
        {
            Restaurants cafe = _context.RestaurantsList.Find(id);
            TempData["CafeID"] = id;

            if (cafe == null)
            {
                return NotFound();
            }
            ViewBag.Cafe = cafe;
            var HallsList = _context.Halls.Where(h => h.RestaurantID == cafe.ID).ToList();
            return View(HallsList);
        }


        public IActionResult Create()
        {
            //string id = (string)TempData.Peek("CafeID");
            //Restaurants cafe = _context.RestaurantsList.Find(id);
            var users = _userManager.Users.ToList();
            ApplicationUser user = new ApplicationUser();
            if (User.Identity.IsAuthenticated)
            {
                user = users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            }
            if (user == null) return Redirect("/Home/Contact");
            var cafe = _context.RestaurantsList.Where(t => t.ID == user.RestaurantID).FirstOrDefault();
            if (cafe == null)
            {
                return NotFound();
            }
            ViewBag.Cafe = cafe;
            ViewBag.Ca = cafe.ID;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Hall model)
        {
            if (model != null)
            {
                model.RestaurantID = (string)TempData["CafeID"];
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = model.RestaurantID });
            }
            return View(model);
        }

        public IActionResult Edit(string id)
        {
            Hall hall = _context.Halls.Find(id);
            if (hall == null)
            {
                return NotFound();
            }
            ViewBag.CafeID = hall.RestaurantID;
            return View(hall);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Hall model)
        {
            ViewBag.CafeID = model.RestaurantID;
            if (ModelState.IsValid)
            {
                Hall hall = _context.Halls.Where(u => u.ID == model.ID).FirstOrDefault();
                if (hall != null)
                {
                    hall.Name = model.Name;
                    hall.Description = model.Description;

                    _context.Halls.Update(hall);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction("Index", new { id = model.RestaurantID });
            }
            return View(model);
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall != null)
            {
                return View(hall);
            }
            return NotFound();
            

        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var hall = await _context.Halls.FindAsync(id);
            
            if (hall != null)
            {
                var tables = _context.TablesList.Where(t => t.HallID == id);
                if(tables.Count()>0)
                {
                    foreach (Tables t in tables)
                        _context.TablesList.Remove(t);
                }
                _context.Halls.Remove(hall);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = hall.RestaurantID });
            }
            return NotFound();
           
        }

        public IActionResult Details (string id)
        {
            Hall hall = _context.Halls.Find(id);
            if (hall == null)
            {
                return NotFound();
            }

            ViewBag.CafeID = hall.RestaurantID;
            var TableList = _context.TablesList.Where(t => t.HallID == hall.ID).ToList();

            return View(hall);
        }

        public IActionResult SeeDetails(string id)
        {

            var hall =_context.Halls.Where(t => t.ID == id).Include(h=> h.tables).FirstOrDefault();
            if (hall == null)
            {
                return NotFound();
            }

            ViewBag.CafeID = hall.RestaurantID;
            var TableList = _context.TablesList.Where(t => t.HallID == hall.ID).ToList();

            return View(hall);
        }

        [AllowAnonymous]
        public IActionResult BookingHall (string id)
        {
            HallBokingViewModel HB = new HallBokingViewModel();
            Hall hall = _context.Halls.Where(t => t.ID == id).Include(h => h.tables).FirstOrDefault();
            if (hall == null)
            {
                return NotFound();
            }
            HB.hall = hall;
            

            return View(HB);
        }

    }
}

