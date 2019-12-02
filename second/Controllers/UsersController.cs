using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using second.Models;
using second.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace second.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("Users/Index")]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            List<IndexUserViewModel> indexUsers = new List<IndexUserViewModel>();
            IndexUserViewModel model = new IndexUserViewModel();
            foreach (var user in users)
            {
                var RestName = _context.RestaurantsList.Where(u => u.ID == user.RestaurantID).FirstOrDefault();
                model = new IndexUserViewModel { Id = user.Id, Email = user.Email, FullName = user.FullName, RestaurantName = RestName != null?RestName.Name:"", PersonPhone=user.PersonPhone };
                indexUsers.Add(model);
            }
            return View(indexUsers);
        }

        public IActionResult Create()
        {
            SelectList restaurantList = new SelectList(_context.RestaurantsList, "ID", "Name");
            ViewBag.Restaurants = restaurantList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.Email, FullName = model.FullName, PersonPhone=model.PersonPhone, RestaurantID=model.RestaurantID };
                var result = await _userManager.CreateAsync(user, model.Password);
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
            return View(model);
        }

        // управление назначением менеджера на ресторан
        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            SelectList restaurantList = new SelectList(_context.RestaurantsList, "ID", "Name");
            ViewBag.Restaurants = restaurantList;
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email,  FullName = user.FullName, PersonPhone = user.PersonPhone, RestaurantID = user.RestaurantID };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {

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


        public async Task<IActionResult> Change(string id)
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
        public async Task<IActionResult> Change(EditUserViewModel model)
        {
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
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> ChangePassword(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}
