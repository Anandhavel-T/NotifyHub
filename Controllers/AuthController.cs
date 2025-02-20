using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using NotifyHub.Models.ViewModels;
using NotifyHub.Infrastructure.Services.Interfaces;
using System;
using Microsoft.Ajax.Utilities;
using System.Web.Helpers;

namespace NotifyHub.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            var token = _userService.GenerateJwtToken(user);
            Response.Cookies.Add(new HttpCookie("AuthToken", token));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //var user = await _userService.AuthenticateAsync(model.Username, model.Password);
            //if (user == null)
            //{
            //    ModelState.AddModelError("", "Invalid username or password");
            //    return View(model);
            //}

            //var token = _userService.GenerateJwtToken(user);
            //Response.Cookies.Add(new HttpCookie("AuthToken", token));

            return RedirectToAction("Index", "Home");
        }
    }
}