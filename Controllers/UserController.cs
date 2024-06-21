using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModaECommerce.Models;
using System.Security.Claims;

namespace ModaECommerce.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ModaCommerceContext _sql;
        public UserController(ILogger<HomeController> logger, ModaCommerceContext sql)
        {
            _sql = sql;
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            var istifadeci = _sql.Users.FirstOrDefault(x => x.UserNickname == user.UserNickname && x.UserPassword == user.UserPassword);
            if (istifadeci != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Sid, istifadeci.UserId.ToString()),
                    new Claim(ClaimTypes.Role, istifadeci.UserRole),
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var princ = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princ, props).Wait();
                return RedirectToAction("index", "home");
            }
            return View();
        }
        public IActionResult Register(User user)
        {
            var istifadeci = _sql.Users.FirstOrDefault(x => x.UserPassword == user.UserPassword && x.UserNickname == user.UserNickname);
            if (istifadeci == null)
            {
                user.UserRole = "User";
                user.UserStatus = "unblock";
                _sql.Users.Add(user);
                _sql.SaveChanges();
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole),
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var princ = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princ, props).Wait();
                return RedirectToAction("index", "home");
            }
            return View("login", "user");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("index", "home");
        }
        [Authorize]
        public IActionResult AccountDashboard()
        {
            return View(_sql.Users.SingleOrDefault(x => x.UserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)));
        }
        [Authorize]
        public IActionResult AccountAddresses()
        {
            ViewBag.User = _sql.Users.SingleOrDefault(x => x.UserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
            ViewBag.AddressesList = _sql.Addresses.Where(x => x.AddressUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)).Include(x => x.AddressUser).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult AddAddress(Address address)
        {
            address.AddressUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            _sql.Addresses.Add(address);
            _sql.SaveChanges();
            return RedirectToAction("AccountAddresses", "user");
        }
        [Authorize]
        public IActionResult AccountEdit()
        {
            return View(_sql.Users.SingleOrDefault(x => x.UserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)));
        }
        [HttpPost]
        public IActionResult AccountEdit(User user, string NewPassword)
        {
            var istifadeci = _sql.Users.SingleOrDefault(x => x.UserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
            if (user.UserPassword == null)
            {
                istifadeci.UserName = user.UserName;
                istifadeci.UserSurname = user.UserSurname;
                istifadeci.UserNickname = user.UserNickname;
            }else
            {
                if(user.UserPassword == istifadeci.UserPassword)
                {
                    istifadeci.UserName = user.UserName;
                    istifadeci.UserSurname = user.UserSurname;
                    istifadeci.UserNickname = user.UserNickname;
                    istifadeci.UserPassword = NewPassword;
                }
                else
                {
                    return BadRequest();
                }
            }
            _sql.SaveChanges();
            return View();
        }
        [Authorize]
        public IActionResult AccountOrders()
        {
            return View(_sql.Cargos.Where(x => x.CargoUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)).Include(x=> x.CargoLevel).Include(x=> x.CargoProduct).ToList());
        }
        [Authorize]
        public IActionResult AccountWishlist()
        {
            return View();
        }
        [HttpGet()]
        public IActionResult AddToBasket(int id, Basket basket)
        {
            basket.BasketProductId = id;
            basket.BasketUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            basket.BasketProductQuantity = 1;
            _sql.Baskets.Add(basket);
            _sql.SaveChanges();
            return Ok();
        }
    }
}
