using NotifyHub.Models.Domain;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using NotifyHub.Infrastructure.Services.Interfaces;
using NotifyHub.Models.ViewModels;

namespace NotifyHub.Controllers
{
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

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = new User
                {
                    FullName = model.FullName,
                    UserName = model.Username,
                    Email = model.Email
                };

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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _userService.GetUserByIdAsync(model.Id);
                if (user == null)
                    return HttpNotFound();

                user.FullName = model.FullName;
                user.UserName = model.Username;
                user.Email = model.Email;

                await _userService.UpdateUserAsync(user);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Json(new { success = true });
        }
    }
}