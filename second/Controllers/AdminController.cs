using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using second.Models;
using second.ViewModels;

namespace second.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RestaurantsList()
        {
            return View(_context.RestaurantsList.ToList());
        }


        public IActionResult Create()
        {
            SelectList restaurantList = new SelectList(_context.RestaurantsList, "ID", "Name");
            ViewBag.Restaurants = restaurantList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Restaurants model)
        {
            if (model != null)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("RestaurantsList");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

           
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, FullName = user.FullName, PersonPhone = user.PersonPhone, RestaurantID = user.RestaurantID };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            SelectList UserList = new SelectList(_context.RestaurantsList, "ID", "Email");
            ViewBag.Users = UserList;
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.FullName = model.FullName;
                    user.PersonPhone = model.PersonPhone;
                    user.FullName = model.FullName;
                    user.RestaurantID = model.RestaurantID;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var cafe = await _context.RestaurantsList.FindAsync(id);
            if (cafe != null)
            {
                _context.RestaurantsList.Remove(cafe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("RestaurantsList");
            
        }

    }
}