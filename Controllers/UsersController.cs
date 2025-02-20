using NotifyHub.Models.Domain;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using NotifyHub.Infrastructure.Services.Interfaces;
using NotifyHub.Models.ViewModels;

namespace NotifyHub.Controllers
{
    //[Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User
            {
                FullName = model.FullName,
                UserName = model.Username,
                Email = model.Email,
                //CreatedById = User.Identity.GetUserId<int>()
            };

            try
            {
                await _userService.CreateUserAsync(user, model.Password);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User
            {
                FullName = model.FullName,
                UserName = model.Username,
                Email = model.Email,
                //CreatedById = User.Identity.GetUserId<int>()
            };

            try
            {
                await _userService.CreateUserAsync(user, model.Password);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Json(new { success = true });
        }
    }
}