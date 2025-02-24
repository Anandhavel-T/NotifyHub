using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using NotifyHub.Models.ViewModels;
using NotifyHub.Infrastructure.Services.Interfaces;
using System;
using Microsoft.Ajax.Utilities;
using System.Web.Helpers;
using NotifyHub.Infrastructure.Services.Implementations;

namespace NotifyHub.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(IUserService userService, IEmailService emailService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _emailService = emailService;
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

            try
            {
                var user = _userService.GetByEmail(model.Email);
                if (user == null)
                {
                    // Don't reveal that the email doesn't exist
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                // Generate password reset token
                string resetToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                //user.PasswordResetToken = resetToken;
                //user.PasswordResetExpiry = DateTime.UtcNow.AddHours(24);

                await _userService.UpdateUserAsync(user);

                // Send password reset email
                string resetLink = Url.Action("ResetPassword", "Auth",
                    new { email = model.Email, token = resetToken },
                    protocol: Request.Url.Scheme);

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Password Reset Request",
                    $@"Dear {user.FullName},<br><br>
            To reset your password, please click the following link:<br>
            <a href='{resetLink}'>Reset Password</a><br><br>
            This link will expire in 24 hours.<br><br>
            If you did not request this password reset, please ignore this email."
                );

                return RedirectToAction("ForgotPasswordConfirmation");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View(model);
            }
        }

        //[HttpGet]
        //public ActionResult ResetPassword(string email, string token)
        //{
        //    var model = new ResetPasswordViewModel
        //    {
        //        Email = email,
        //        Token = token
        //    };
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var user = _userService.GetByEmail(model.Email);
        //    if (user == null || user.PasswordResetToken != model.Token ||
        //        user.PasswordResetExpiry < DateTime.UtcNow)
        //    {
        //        ModelState.AddModelError("", "Invalid password reset token.");
        //        return View(model);
        //    }

        //    // Reset password
        //    user.PasswordHash = _userService.HashPassword(model.NewPassword);
        //    //user.PasswordResetToken = null;
        //    //user.PasswordResetExpiry = null;
        //    user.UpdatedAt = DateTime.UtcNow;

        //    await _userService.UpdateUserAsync(user);

        //    return RedirectToAction("ResetPasswordConfirmation");
        //}
    }
}