using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using second.Models;
using second.ViewModels;
using Newtonsoft.Json;
using second.ViewModels.ManagerViews;

namespace second.Controllers
{
    [Authorize]
    public class RestaurantController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        IHostingEnvironment _appEnvironment;
        public RestaurantController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet("Restaurant/Index")]
        public IActionResult Index()
        {
            ApplicationUser user = new ApplicationUser();
            var users = _userManager.Users.ToList();

            if (User.Identity.IsAuthenticated)
            {
                user = users.Where(u=>u.UserName== User.Identity.Name).FirstOrDefault();
            }
            if (user==null) return Redirect("/Home/Contact");
            var Restaraunt = _context.RestaurantsList.Where(t=>t.ID==user.RestaurantID).Include(cafe=>cafe.pictures).Include(cafe => cafe.halls).FirstOrDefault();
            if (Restaraunt==null) return Redirect("/Home/Contact");
            return View(Restaraunt);
        }

        //редактирование текстового описания ресторана
        public async Task<IActionResult> Edit(string id)
        {
            Restaurants cafe = _context.RestaurantsList.Find(id);
            if (cafe == null)
            {
                return NotFound();
            }
            return View(cafe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Restaurants model)
        {
            if (ModelState.IsValid)
            {
                Restaurants cafe = await _context.RestaurantsList.FirstOrDefaultAsync(u=>u.ID==model.ID);
                if (cafe != null)
                {
                    cafe.Adres = model.Adres;
                    cafe.Description = model.Description;
                    cafe.Mail = model.Mail;
                    cafe.Name = model.Name;
                    cafe.Phone = model.Phone;
                    cafe.Site = model.Site;
                    cafe.MoreOption= model.MoreOption;


                    _context.RestaurantsList.Update(cafe);
                    await _context.SaveChangesAsync();
                    return Redirect("/Restaurant/Index");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFileCollection uploads, string id)
        {
            if (uploads != null)
            {
                foreach (var uploadedFile in uploads)
                {
                    
                    if (!Directory.Exists(_appEnvironment.WebRootPath+"/Fotos/" + id))
                    {
                        Directory.CreateDirectory(_appEnvironment.WebRootPath+"/Fotos/" + id);
                    }
                    string path = "/Fotos/" + id + "/" + Path.GetRandomFileName() + uploadedFile.FileName;


                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    Restaurants restaurant = await _context.RestaurantsList.FirstOrDefaultAsync(u => u.ID == id);

                    Pictures pic = new Pictures { Name = uploadedFile.FileName, PathToImg = path, RestaurantID = id };
                    _context.TablesImages.Add(pic);
                    restaurant.pictures.Add(pic);
                }
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        //[HttpPost]
        public async Task<IActionResult> DelFile(string id)
        {
            if (id != null && id!="")
            {
                Pictures pic = await _context.TablesImages.FirstOrDefaultAsync(u => u.ID == id);

                // путь к папке Files
                string path = pic.PathToImg;
                FileInfo fileInfo= new FileInfo(path);
                if (!fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                _context.TablesImages.Remove(pic);
                
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        //for home page details 
        [AllowAnonymous]
        public IActionResult Details(string id)
        {
            Restaurants Cafe = _context.RestaurantsList.Where(c=>c.ID==id)
               .Include(cafe=>cafe.pictures)
               .Include(cafe => cafe.halls)
               .ThenInclude(hall=>hall.tables)
               .First();
            
            
            if (Cafe == null)
            {
                return NotFound();
            }

     
            return View(Cafe);
        }


        public async Task<IActionResult> Manage (string company, string date, int page = 1,
            SortState sortOrder = SortState.CheckInAsc)
        {
            int pageSize = 10;
            ApplicationUser user = new ApplicationUser();
            var users = _userManager.Users.ToList();

            if (User.Identity.IsAuthenticated)
            {
                user = users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            }
            if (user == null) return NotFound();

            var Restaraunt = _context.RestaurantsList.Where(t => t.ID == user.RestaurantID).FirstOrDefault();
            if (Restaraunt == null) return Redirect("/Home/Contact");

            var halls = _context.Halls.Where(h => h.RestaurantID == Restaraunt.ID);
            if (halls.Count()<1) return Redirect("/Home/Contact");
            List<HallSimple> SelectedHallsList = new List<HallSimple>();
            foreach(Hall h in halls)
            {
                HallSimple Temp = new HallSimple { HallID = h.ID, HallName = h.Name };
                SelectedHallsList.Add(Temp);
            }
            var table = _context.TablesList.Join(halls, T => T.HallID,
                           H => H.ID,
                           (T, H) => new { id = T.ID, hallid = H.ID, tablename = T.Name, hallname = H.Name }
                           ).ToList();
            IQueryable<ManageBookingViewModel> bookings = _context.Bookings.Join(table, B => B.TableID,
                                   T => T.id,
                                   (B, T) => new ManageBookingViewModel
                                   {
                                       BookingID = B.ID,
                                       CheckIn = B.CheckIn,
                                       CheckOut = B.CheckOut,
                                       Completed = B.Completed,
                                       CustomerName = B.CustomerName,
                                       CustomerPhone = B.CustomerPhone,
                                       Guests = B.Guests,
                                       Paid = B.Paid,
                                       HallID = T.hallid,
                                       HallName = T.hallname,
                                       TableName = T.tablename,
                                       TableID = B.TableID
                                   }
                               );
           
            if (company != null && company != "0")
            {
                bookings = bookings.Where(p => p.HallID == company);
            }

            DateTime SD = DateTime.MinValue;
            DateTime dateValue;
            if (DateTime.TryParse(date, out dateValue))
            {
                SD = Convert.ToDateTime(date);
                bookings=bookings.Where(p => p.CheckIn >= SD);
            } 

            switch (sortOrder)
            {
                case SortState.CheckInDesc:
                    bookings = bookings.OrderByDescending(s => s.CheckIn);
                    break;
                case SortState.PhoneAsc:
                    bookings = bookings.OrderBy(s => s.CustomerPhone);
                    break;
                case SortState.PhoneDesc:
                    bookings = bookings.OrderByDescending(s => s.CustomerPhone);
                    break;
                case SortState.HallAsc:
                    bookings = bookings.OrderBy(s => s.HallName);
                    break;
                case SortState.HallDesc:
                    bookings = bookings.OrderByDescending(s => s.HallName);
                    break;
                default:
                    bookings = bookings.OrderBy(s => s.CheckIn);
                    break;
            }

            var count = await bookings.CountAsync();
            var items = await bookings.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            
            IndexVIewModel viewModel = new IndexVIewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(SelectedHallsList, company,SD),
                Bookings = items
            };


            return View(viewModel);
        }
    }
}