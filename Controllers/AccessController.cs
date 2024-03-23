using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectPRN221_2.Models;

namespace ProjectPRN221_2.Controllers
{
    public class AccessController : Controller
    {
        private readonly TestProjectPRN221Context _context;

        public AccessController(TestProjectPRN221Context context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Account modelLogin)
        {
            var email = _context.Accounts.First().Account1;
            var password = _context.Accounts.First().Password;

            if (modelLogin.Account1 == email && modelLogin.Password == password)
            {
                List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, modelLogin.Account1),
                new Claim("OtherProperties", "Example Role")
            };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    //IsPersistent = modelLogin.KeepLoggedIn,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }

            ViewData["ValidateMessage"] = "user not found";

            return View();
        }
    }
}
