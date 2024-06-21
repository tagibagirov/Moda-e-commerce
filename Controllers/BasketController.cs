using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ModaECommerce.Models;
using ModaECommerce.Models.ViewModel;
using System.Data;
using System.Linq;
using System.Security.Claims;

namespace ModaECommerce.Controllers
{
    public class BasketController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ModaCommerceContext _sql;

        public BasketController(ILogger<HomeController> logger, ModaCommerceContext sql)
        {
            _sql = sql;
            _logger = logger;
        }

        BasketPrice basketPrice = new BasketPrice();

        [Authorize]
        public IActionResult ShowBasket()
        {
            Response.Cookies.Delete("shippingType");
            var basketProducts = _sql.Baskets.Where(x => x.BasketUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)).Include(x => x.BasketProduct).ThenInclude(x => x.ProductColor);
            return View(basketProducts.ToList());
        }
        [Authorize]
        public IActionResult Checkout()
        {
            string shippingType = Request.Cookies["shippingType"];

            if (shippingType == null)
            {
                return RedirectToAction("ShowBasket", "Basket");
            }
            var basketProducts = _sql.Baskets.Where(x => x.BasketUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)).Include(x => x.BasketProduct);
            ViewBag.ShippingType = shippingType;
            return View(basketProducts.ToList());
        }
        [Authorize]
        public IActionResult OrderComplete(Cargo cargo)
        {
            var BasketProduct = _sql.Baskets.Where(x => x.BasketUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)).Include(x => x.BasketProduct).ToList();
            cargo.CargoUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            cargo.CargoTime = DateTime.Now;
            cargo.CargoLevelId = 1;
            _sql.Cargos.Add(cargo);
            _sql.SaveChanges();
            foreach (var item in BasketProduct)
            {
                var baglama = new Baglama();
                baglama.BaglamaProductId = item.BasketProductId;
                baglama.BaglamaCargoId = cargo.CargoId;
                _sql.Baglamas.Add(baglama);
                _sql.SaveChanges();
            }
            cargo.CargoPrice = (float?)Convert.ToDouble(Request.Cookies["TotalPrice"]);
            _sql.SaveChanges();
            string shippingType = Request.Cookies["shippingType"];
            ViewBag.Cargos = cargo;
            ViewBag.ShippingType = shippingType;
            return View(BasketProduct);
        }

        public IActionResult QuantityReduce(int basketId)
        {
            var product = _sql.Baskets.Include(x => x.BasketProduct).SingleOrDefault(x => x.BasketId == basketId);
            IQueryable<Basket> basketProducts = _sql.Baskets
            .Where(x => x.BasketUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value))
            .Include(x => x.BasketProduct);
            product.BasketProductQuantity--;
            _sql.SaveChanges();
            basketPrice.ProductQuantity = (int)product.BasketProductQuantity;
            basketPrice.Price = (float)(product.BasketProductQuantity * product.BasketProduct.ProductPrice);
            basketPrice.TotalPrice = (float)basketProducts.Sum(item => item.BasketProductQuantity * item.BasketProduct.ProductPrice);
            return Ok(basketPrice);
        }
        public IActionResult QuantityIncrease(int basketId)
        {
            var product = _sql.Baskets.Include(x => x.BasketProduct).SingleOrDefault(x => x.BasketId == basketId);
            IQueryable<Basket> basketProducts = _sql.Baskets
            .Where(x => x.BasketUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value))
            .Include(x => x.BasketProduct);
            product.BasketProductQuantity++;
            _sql.SaveChanges();
            basketPrice.ProductQuantity = (int)product.BasketProductQuantity;
            basketPrice.Price = (float)(product.BasketProductQuantity * product.BasketProduct.ProductPrice);
            basketPrice.TotalPrice = (float)basketProducts.Sum(item => item.BasketProductQuantity * item.BasketProduct.ProductPrice);
            return Ok(basketPrice);
        }

        public IActionResult RemoveCartItem(int basketId)
        {
            var product = _sql.Baskets.Include(x => x.BasketProduct).SingleOrDefault(x => x.BasketId == basketId);
            IQueryable<Basket> basketProducts = _sql.Baskets
            .Where(x => x.BasketUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value))
            .Include(x => x.BasketProduct);
            _sql.Baskets.Remove(product);
            _sql.SaveChanges();
            basketPrice.TotalPrice = (float)basketProducts.Sum(item => item.BasketProductQuantity * item.BasketProduct.ProductPrice);
            return Ok(basketPrice);
        }
        public IActionResult EmptyBasket()
        {
            var basketProducts = _sql.Baskets.Where(x => x.BasketUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
            _sql.Baskets.RemoveRange(basketProducts);
            _sql.SaveChanges();
            basketPrice.TotalPrice = 0;
            return Ok(basketPrice);
        }
    }
}
