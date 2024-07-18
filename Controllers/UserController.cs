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
            else
            {
                ModelState.AddModelError("error", "Username or password is incorrect...");
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var istifadeci = _sql.Users.FirstOrDefault(x => x.UserEmail == user.UserEmail || x.UserNickname == user.UserNickname);
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
            else
            {
                ViewBag.ErrorMessage = "Sorry, but we already have a user with the same email address or nickname. Please try another one.";
                return View();
            }
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
        public IActionResult AccountAddresses(bool addressAddBool = false, bool addressEditBool = false)
        {
            ViewBag.AddressAdd = addressAddBool;
            ViewBag.AddressEdit = addressEditBool;
            ViewBag.AddressesList = _sql.Addresses.Where(x => x.AddressUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)).ToList();
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAddress(Address address, bool addressAddBool = false)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AccountAddresses", "user", new { addressAddBool });
            }
            address.AddressUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            _sql.Addresses.Add(address);
            _sql.SaveChanges();
            return RedirectToAction("AccountAddresses", "user");
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditAddress(int id, Address address, bool addressEditBool = false)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AccountAddresses", "user", new { addressEditBool });
            }
            var _address = _sql.Addresses.SingleOrDefault(x => x.AddressId == id);
            _address.AddressTitle = address.AddressTitle;
            _address.AddressCountry = address.AddressCountry;
            _address.AddressCity = address.AddressCity;
            _address.AddressDistrict = address.AddressDistrict;
            _address.AddressStreet = address.AddressStreet;
            _address.AddressHouse = address.AddressHouse;
            _address.AddressApartment = address.AddressApartment;
            _sql.SaveChanges();
            return RedirectToAction("AccountAddresses", "user");
        }
        [Authorize]
        public IActionResult RemaveAddress(int id)
        {
            var address = _sql.Addresses.SingleOrDefault(x => x.AddressId == id);
            _sql.Addresses.Remove(address);
            _sql.SaveChanges();
            return RedirectToAction("AccountAddresses", "user");
        }
        [Authorize]
        public IActionResult AccountEdit()
        {
            return View(_sql.Users.SingleOrDefault(x => x.UserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)));
        }
        [Authorize]
        [HttpPost]
        public IActionResult AccountEdit(User user, string NewPassword = " ", bool? passwordBool = false)
        {
            ViewBag.ChangePassword = passwordBool;
            var istifadeci = _sql.Users.SingleOrDefault(x => x.UserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
            if (!ModelState.IsValid)
            {
                return View(istifadeci);
            }
            if (!(bool)passwordBool)
            {
                istifadeci.UserName = user.UserName;
                istifadeci.UserSurname = user.UserSurname;
                istifadeci.UserNickname = user.UserNickname;
                istifadeci.UserPhone = user.UserPhone;
                istifadeci.UserEmail = user.UserEmail;
            }
            else
            {
                if (user.UserPassword == istifadeci.UserPassword)
                {
                    if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword.Length < 8)
                    {
                        ModelState.AddModelError("NewPassword", "The length of password must be 8 characters or more.");
                        return View(istifadeci);
                    }
                    istifadeci.UserName = user.UserName;
                    istifadeci.UserSurname = user.UserSurname;
                    istifadeci.UserNickname = user.UserNickname;
                    istifadeci.UserPhone = user.UserPhone;
                    istifadeci.UserEmail = user.UserEmail;
                    istifadeci.UserPassword = NewPassword;
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "Password is incorrect...");
                    return View(istifadeci);
                }
            }
            _sql.SaveChanges();
            return View();
        }
        [Authorize]
        public IActionResult AccountOrders()
        {
            return View(_sql.Cargos.Where(x => x.CargoUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)).Include(x => x.CargoLevel).ToList());
        }
        [Authorize]
        public IActionResult AccountWishlist()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult AddToBasket(int id)
        {
            Basket basket = new Basket();
            basket.BasketProductId = id;
            basket.BasketUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            basket.BasketProductQuantity = 1;
            _sql.Baskets.Add(basket);
            _sql.SaveChanges();
            return Ok();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddToBasket(int id, Basket basket)
        {
            basket.BasketProductId = id;
            basket.BasketUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            _sql.Baskets.Add(basket);
            _sql.SaveChanges();
            return RedirectToAction("ProductDescription", "home", new { id });
        }
        public IActionResult OrderDetails(int id)
        {
            var products = _sql.Cargos
                .Include(x => x.CargoLevel)
                .Include(x => x.Baglamas).ThenInclude(x => x.BaglamaProduct).ThenInclude(x => x.Photos)
                .Include(x => x.Baglamas).ThenInclude(x => x.BaglamaProduct).ThenInclude(x => x.ProductBrend)
                .Include(x => x.Baglamas).ThenInclude(x => x.BaglamaProduct).ThenInclude(x => x.ProductCategory)
                .Include(x => x.Baglamas).ThenInclude(x => x.BaglamaProduct).ThenInclude(x => x.ProductColor)
                .SingleOrDefault(x => x.CargoId == id);
            ViewBag.Sizes = _sql.Sizes.ToList();
            return View(products);
        }
    }
}
