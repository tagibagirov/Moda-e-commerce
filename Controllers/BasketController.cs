using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ModaECommerce.Models;
using ModaECommerce.Models.ViewModel;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
using System.Security.Claims;

namespace ModaECommerce.Controllers
{
    [Authorize]

    public class BasketController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ModaCommerceContext _sql;
        private BasketPrice basketPrice;
        public BasketController(ILogger<HomeController> logger, ModaCommerceContext sql)
        {
            _sql = sql;
            _logger = logger;
            basketPrice = new BasketPrice();
        }

        [HttpGet]
        public IActionResult ShowBasket()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            var basketProducts = _sql.Baskets.Where(x => x.BasketUserId == userId).Include(x => x.BasketProduct).ThenInclude(x => x.Photos).Include(x => x.BasketProduct).ThenInclude(x => x.ProductColor).ToList();
            ViewBag.Sizes = _sql.Sizes.ToList();
            ViewBag.BasketProducts = basketProducts;
            return View();
        }

        [HttpPost]
        public IActionResult ShowBasket(BaglamaDetails baglamaDetails)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            var basketProducts = _sql.Baskets.Where(x => x.BasketUserId == userId).Include(x => x.BasketProduct).ThenInclude(x => x.Photos).Include(x => x.BasketProduct).ThenInclude(x => x.ProductColor).ToList();
            ViewBag.Sizes = _sql.Sizes.ToList();
            ViewBag.BasketProducts = basketProducts;
            if (!basketProducts.Any())
            {
                ModelState.AddModelError("basketError", "Basket empty! You must select a product to confirm the basket.");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            HttpContext.Session.SetString("BaglamaDetails", JsonConvert.SerializeObject(baglamaDetails));
            return RedirectToAction("Checkout", "basket");
        }

        public IActionResult Checkout()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            ViewBag.BasketProducts = _sql.Baskets.Where(x => x.BasketUserId == userId).Include(x => x.BasketProduct).ToList();
            ViewBag.User = _sql.Users.SingleOrDefault(x => x.UserId == userId);
            var baglamaDetailsString = HttpContext.Session.GetString("BaglamaDetails");
            var baglamaDetails = JsonConvert.DeserializeObject<BaglamaDetails>(baglamaDetailsString);
            ViewBag.BasketDetails = baglamaDetails;
            ViewBag.AddressesList = _sql.Addresses.Where(x => x.AddressUserId == userId).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Cargo cargo, int Address)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            var BasketProduct = _sql.Baskets.Where(x => x.BasketUserId == userId).Include(x => x.BasketProduct).ToList();
            var baglamaDetailsString = HttpContext.Session.GetString("BaglamaDetails");
            var baglamaDetails = JsonConvert.DeserializeObject<BaglamaDetails>(baglamaDetailsString);
            var TotalPrice = BasketProduct.Sum(x => x.BasketProductQuantity * x.BasketProduct?.ProductPrice) + 19;
            float Weight = (float)BasketProduct.Sum(x => x.BasketProductQuantity * x.BasketProduct.ProductWeight);
            var baglamas = new List<Baglama>();

            ViewBag.BasketProducts = BasketProduct;
            ViewBag.User = _sql.Users.SingleOrDefault(x => x.UserId == userId);
            ViewBag.BasketDetails = baglamaDetails;
            ViewBag.AddressesList = _sql.Addresses.Where(x => x.AddressUserId == userId).ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }

            switch (baglamaDetails?.ShippingType)
            {
                case "Flat rate: $49":
                    TotalPrice += 49;
                    break;
                case "Local pickup: $8":
                    TotalPrice += 8;
                    break;
                default:
                    break;
            }

            if (Address != 0)
            {
                var addresses = _sql.Addresses.Where(x => x.AddressUserId == userId).ToList();

                if (Address < addresses.Count)
                {
                    var _address = addresses[(int)Address - 1];
                    cargo.CargoAddressCountry = _address.AddressCountry;
                    cargo.CargoAddressCity = _address.AddressCity;
                    cargo.CargoAddressDistrict = _address.AddressDistrict;
                    cargo.CargoAddressStreet = _address.AddressStreet;
                    cargo.CargoAddressHouse = _address.AddressHouse;
                    cargo.CargoAddressApartment = _address.AddressApartment;
                }
            }

            cargo.CargoPrice = TotalPrice;
            cargo.CargoTime = DateTime.Now;
            cargo.CargoLevelId = 1;
            cargo.CargoUserId = userId;
            var id = cargo.CargoId;
            _sql.Cargos.Add(cargo);
            _sql.SaveChanges();
            foreach (var item in BasketProduct)
            {
                var baglama = new Baglama();
                baglama.BaglamaProductId = item.BasketProductId;
                baglama.BaglamaCargoId = cargo.CargoId;
                baglama.BaglamaProductQuantity = item.BasketProductQuantity;
                baglama.BaglamaProductSizeId = baglamaDetails?.SizeId;
                _sql.Baglamas.Add(baglama);
                //baglamas.Add(baglama);
            }
            cargo.CargoWeight = Weight;
            //_sql.Baglamas.AddRange(baglamas);
            _sql.SaveChanges();
            return RedirectToAction("OrderComplete", "basket", new { id });
        }

        public IActionResult OrderComplete(int id)
        {
            var baglamaDetailsString = HttpContext.Session.GetString("BaglamaDetails");
            var baglamaDetails = JsonConvert.DeserializeObject<BaglamaDetails>(baglamaDetailsString);
            ViewBag.BaglamaDetails = baglamaDetails;
            var cargo = _sql.Cargos
                .Include(x => x.CargoLevel)
                .Include(x => x.Baglamas).ThenInclude(x => x.BaglamaProduct).ThenInclude(x => x.Photos)
                .Include(x => x.Baglamas).ThenInclude(x => x.BaglamaProduct).ThenInclude(x => x.ProductBrend)
                .Include(x => x.Baglamas).ThenInclude(x => x.BaglamaProduct).ThenInclude(x => x.ProductCategory)
                .Include(x => x.Baglamas).ThenInclude(x => x.BaglamaProduct).ThenInclude(x => x.ProductColor)
                .SingleOrDefault(x => x.CargoId == id);
            return View(cargo);
        }

        public IActionResult QuantityControl(int basketId, string type)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            var product = _sql.Baskets.Include(x => x.BasketProduct).SingleOrDefault(x => x.BasketId == basketId);
            IQueryable<Basket> basketProducts = _sql.Baskets
            .Where(x => x.BasketUserId == userId)
            .Include(x => x.BasketProduct);
            if (type == "increase" && product.BasketProductQuantity < 10)
            {
                product.BasketProductQuantity++;
            }
            else if (type == "reduce" && product.BasketProductQuantity > 1)
            {
                product.BasketProductQuantity--;
            }
            _sql.SaveChanges();
            basketPrice.ProductQuantity = (int)product.BasketProductQuantity;
            basketPrice.Price = (float)(product.BasketProductQuantity * product.BasketProduct.ProductPrice);
            basketPrice.TotalPrice = (float)basketProducts.Sum(item => item.BasketProductQuantity * item.BasketProduct.ProductPrice);
            return Ok(basketPrice);
        }

        public IActionResult RemoveCartItem(int basketId)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            var product = _sql.Baskets.Include(x => x.BasketProduct).SingleOrDefault(x => x.BasketId == basketId);
            IQueryable<Basket> basketProducts = _sql.Baskets
            .Where(x => x.BasketUserId == userId)
            .Include(x => x.BasketProduct);
            _sql.Baskets.Remove(product);
            _sql.SaveChanges();
            basketPrice.TotalPrice = (float)basketProducts.Sum(item => item.BasketProductQuantity * item.BasketProduct.ProductPrice);
            return Ok(basketPrice);
        }
        public IActionResult EmptyBasket()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            var basketProducts = _sql.Baskets.Where(x => x.BasketUserId == userId);
            _sql.Baskets.RemoveRange(basketProducts);
            _sql.SaveChanges();
            basketPrice.TotalPrice = 0;
            return Ok(basketPrice);
        }
    }
}
