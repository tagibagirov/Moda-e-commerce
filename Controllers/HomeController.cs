using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModaECommerce.Models;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using static ModaECommerce.Controllers.HomeController;

namespace ModaECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ModaCommerceContext _sql;

        public HomeController(ILogger<HomeController> logger, ModaCommerceContext sql)
        {
            _sql = sql;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_sql.Products.Include(x => x.ProductCategory).Include(x => x.Photos).Where(p => p.ProductStatus == "True").ToList());
        }
        [HttpGet("/home/SearchProduct/{value}")]
        public IActionResult SearchProduct(string value)
        {
            var searchValue = value.ToLower();
            var product = _sql.Products.Include(x => x.Photos)
                .Include(x => x.ProductCategory)
                .Include(x => x.ProductBrend)
                .Where(x => x.ProductName.ToLower().Contains(searchValue) 
                || x.ProductBrend.BrandName.ToLower().Contains(searchValue) 
                || x.ProductCategory.CategoryName.ToLower().Contains(searchValue));
            return Ok(product);
        }

        public IActionResult Shop(string Gender, int sort)
        {
            ViewBag.Sizes = _sql.Sizes.ToList();
            ViewBag.Categories = _sql.Categories.ToList();
            ViewBag.Colors = _sql.Colors.ToList();
            ViewBag.Brands = _sql.Brands.ToList();
            IQueryable<Product> products = _sql.Products.Include(x => x.ProductCategory).Include(x => x.Photos).Where(p => p.ProductStatus == "True");
            if (!Gender.IsNullOrEmpty())
            {
                products = products.Where(x => x.ProductGender == Gender);
            }
            switch (sort)
            {
                case 2:
                    products = products.OrderBy(x => x.ProductName);
                    break;
                case 3:
                    products = products.OrderByDescending(x => x.ProductName);
                    break;
                case 4:
                    products = products.OrderBy(x => x.ProductPrice);
                    break;
                case 5:
                    products = products.OrderByDescending(x => x.ProductPrice);
                    break;
            }
            return View(products.ToList());
        }
        public IActionResult Filter(int cat, string colorList, string brandList, string sizeList, int maxPrice = 0, int minPrice = 0)
        {
            IQueryable<Product> products = _sql.Products
                .Include(x => x.ProductBrend)
                .Include(x => x.ProductCategory)
                .Include(x => x.ProductColor)
                .Include(x => x.Photos).Where(x => x.ProductStatus == "True").Where(x => x.ProductPrice > minPrice);
            if(maxPrice != 0)
            {
                products = products.Where(x => x.ProductPrice < maxPrice);
            }
            if (cat != 0)
            {
                products = products.Where(x => x.ProductCategoryId == cat);
            }
            if (colorList != "[]" && colorList != null)
            {
                List<int> colorList2 = JsonSerializer.Deserialize<List<int>>(json: colorList);
                products = products.Where(x => colorList2.Contains((int)x.ProductColorId));
            }
            if (brandList != "[]" && brandList != null)
            {
                List<int> brandList2 = JsonSerializer.Deserialize<List<int>>(json: brandList);
                products = products.Where(x => brandList2.Contains((int)x.ProductBrendId));
            }
            if (sizeList != "[]" && sizeList != null)
            {
                List<string> sizeList2 = JsonSerializer.Deserialize<List<string>>(json: sizeList);
                foreach (var size in sizeList2)
                {
                    products = products.Where(p => JsonSerializer.Deserialize<List<string>>(p.ProductSize, new JsonSerializerOptions()).Contains(size));

                };
            }
            return Json(products.ToList());
        }
        public IActionResult SearchBrand(string brand)
        {
            IQueryable<Brand> brands = _sql.Brands;
            if (brand != null)
            {
                brands = brands.Where(x => x.BrandName.ToLower().Contains(brand.ToLower()));
            }
            return Json(brands.ToList());
        }
        public IActionResult ProductDescription(int id)
        {
            ViewBag.Sizes = _sql.Sizes.ToList();
            return View(_sql.Products.Include(x => x.ProductBrend).Include(x => x.ProductCategory).Include(x => x.ProductColor).Include(x => x.Photos).FirstOrDefault(x => x.ProductId == id));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
