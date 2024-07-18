using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModaECommerce.Models;
using System.Security.Claims;

namespace ModaECommerce.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ModaCommerceContext _sql;

        public AdminController(ILogger<HomeController> logger, ModaCommerceContext sql)
        {
            _sql = sql;
            _logger = logger;
        }
        public IActionResult AdminPanel()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            return View(_sql.Users.SingleOrDefault(x => x.UserId == userId));
        }
        public IActionResult UsersList()
        {
            return View(_sql.Users.Where(x => x.UserRole != "Admin").ToList());
        }
        [HttpGet("/Admin/ChangeUserStat/{id}")]
        public IActionResult ChangeUserStat(int id)
        {
            var user = _sql.Users.SingleOrDefault(x => x.UserId == id);
            if (user.UserStatus == "unblock")
            {
                user.UserStatus = "block";
            }
            else
            {
                user.UserStatus = "unblock";
            }
            _sql.SaveChanges();
            return Ok();
        }
        [HttpGet("/Admin/DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var istifadeci = _sql.Users.SingleOrDefault(x => x.UserId == id);
            var address =  _sql.Addresses.Where(x => x.AddressUserId == id);
            _sql.Addresses.RemoveRange(address);
            _sql.Users.Remove(istifadeci);
            _sql.SaveChanges();
            return Ok();
        }
        public IActionResult ConfirmProduct()
        {
            return View(_sql.Products.Include(x => x.ProductCategory).Include(x=>x.Photos).Where(x => x.ProductStatus == "False").ToList());
        }
        [HttpPost]
        public IActionResult ConfirmProduct(int id)
        {
            var a = _sql.Products.SingleOrDefault(x => x.ProductId == id);
            a.ProductStatus = "True";
            _sql.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        public IActionResult RejectProduct(int id)
        {
            var a = _sql.Products.SingleOrDefault(x => x.ProductId == id);
            _sql.Products.Remove(a);
            _sql.SaveChanges();
            return Ok();
        }
        public IActionResult AddProduct()
        {
            ViewBag.Colors = _sql.Colors.ToList();
            ViewBag.Sizes = _sql.Sizes.ToList();
            ViewBag.Brands = _sql.Brands.ToList();
            ViewBag.Categories = _sql.Categories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(Product product, IFormFile[] productPhoto)
        {
            ViewBag.Colors = _sql.Colors.ToList();
            ViewBag.Sizes = _sql.Sizes.ToList();
            ViewBag.Brands = _sql.Brands.ToList();
            ViewBag.Categories = _sql.Categories.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            product.ProductRatings = 0;
            product.ProductAverageRating = 0;
            product.ProductStatus = "False";
            _sql.Products.Add(product);
            _sql.SaveChanges();
            foreach (var item in productPhoto)
            {
                string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(item.FileName);
                using (Stream stream = new FileStream("wwwroot/img/productsPhoto/" + filename, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
                Photo p = new Photo();
                p.PhotoName = filename;
                p.PhotoProductId = product.ProductId;
                _sql.Photos.Add(p);
                _sql.SaveChanges();
            }
            return View();
        }
        public IActionResult EditProduct(int id)
        {
            ViewBag.Colors = _sql.Colors.ToList();
            ViewBag.Sizes = _sql.Sizes.ToList();
            ViewBag.Brands = _sql.Brands.ToList();
            ViewBag.Categories = _sql.Categories.ToList();
            return View(_sql.Products);
        }
        [HttpPost]
        public IActionResult EditProduct(int id, Product product)
        {
            var a = _sql.Products.SingleOrDefault(x => x.ProductId == product.ProductId);
            a.ProductName = product.ProductName;
            a.ProductSize = product.ProductSize;
            a.ProductPrice = product.ProductPrice;
            a.ProductColor = product.ProductColor;
            a.ProductAbout = product.ProductAbout;
            a.ProductCountry = product.ProductCountry;
            a.ProductWeight = product.ProductWeight;
            a.ProductYear = product.ProductYear;
            _sql.SaveChanges();
            return RedirectToAction("ConfirmProduct", "Admin");
        }
        public IActionResult DeleteProduct(int id)
        {
            var a = _sql.Products.SingleOrDefault(x => x.ProductId == id);
            _sql.Products.Remove(a);
            _sql.SaveChanges();
            return Ok();
        }
        public IActionResult ConfirmOrders(int levelId, int cargoId, int filtrId = 0)
        {
            IQueryable<Cargo> cargoList = _sql.Cargos.Include(x => x.CargoLevel);
            if (levelId != 0)
            {
                var cargo = _sql.Cargos.SingleOrDefault(x => x.CargoId == cargoId);
                cargo.CargoLevelId = levelId;
                _sql.SaveChanges();
            }
            if (filtrId == 1)
            {
                cargoList = cargoList.Where(x => x.CargoLevelId == 1);
            }
            else if (filtrId == 2)
            {
                cargoList = cargoList.Where(x => x.CargoLevelId >= 2 && x.CargoLevelId < 6);
            }
            else if (filtrId == 3)
            {
                cargoList = cargoList.Where(x => x.CargoLevelId == 6);
            }
            ViewBag.Levels = new SelectList(_sql.Levels.ToList(), "LevelId", "LevelName");
            return View(cargoList.ToList());
        }
    }
}
