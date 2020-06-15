using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // for session
using Microsoft.AspNetCore.Identity; // for password hashing
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {

        private PACContext dbContext;

        public HomeController(PACContext context)
        {
            dbContext = context;
        }

        // Base route
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        ///////////// BEGINNING OF CRUD METHODS FOR PRODUCT MODEL /////////////

        // GET ALL Products
        [HttpGet("products")]
        public IActionResult Products()
        {
            //Product[] AllProducts = dbContext.Products.ToArray();
            ViewBag.AllProducts = dbContext.Products.ToArray();
            return View();
        }

        // GET Product/create Page
        [HttpGet("products/{productId}")]
        public IActionResult ViewProduct(int productId)
        {
            ViewBag.Product = dbContext.Products
                .Include(p => p.Categories)
                .ThenInclude(c => c.Category)
                .FirstOrDefault(p => p.ProductId == productId);

            ViewBag.Categories = dbContext.Categories.ToArray();

            return View();
        }

        [HttpPost("products/{productId}")]
        public IActionResult UpdateProduct(int productId)
        {
            Product product = dbContext.Products.FirstOrDefault(p => p.ProductId == productId);
            Category category = dbContext.Categories.FirstOrDefault(c => c.CategoryId == Convert.ToInt32(Request.Form["Category"]));

            Association newAssociation = new Association() { ProductId = product.ProductId, CategoryId = category.CategoryId, Product = product, Category = category };
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();

            return RedirectToAction("Products");
        }

        [HttpGet("categories/{categoryId}")]
        public IActionResult ViewCategory(int categoryId)
        {
            ViewBag.Category = dbContext.Categories
                .Include(c => c.Products)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(c => c.CategoryId == categoryId);

            ViewBag.Products = dbContext.Products.ToArray();

            return View();
        }

        [HttpPost("categories/{categoryId}")]
        public IActionResult UpdateCategory(int categoryId)
        {
            Product product = dbContext.Products.FirstOrDefault(p => p.ProductId == Convert.ToInt32(Request.Form["Product"]));
            Category category = dbContext.Categories.FirstOrDefault(c => c.CategoryId == categoryId);

            Association newAssociation = new Association() { ProductId = product.ProductId, CategoryId = category.CategoryId, Product = product, Category = category };
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();

            return RedirectToAction("Categories");
        }

        // POST Create One Single Product
        [HttpPost("product/create")]
        public IActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(product);
                dbContext.SaveChanges();
                return RedirectToAction("Products");
            }
            ViewBag.AllProducts = dbContext.Products.ToArray();
            return View("Products");
        }

        ///////////// END OF CRUD METHODS FOR PRODUCT MODEL /////////////

        ///////////// BEGINNING OF CRUD METHODS FOR CATEGORY MODEL /////////////

        // GET ALL Categories
        [HttpGet("categories")]
        public IActionResult Categories()
        {
            ViewBag.AllCategories = dbContext.Categories.ToArray();
            return View();
        }

        // POST Create One Single Category
        [HttpPost("category/create")]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(category);
                dbContext.SaveChanges();
                return RedirectToAction("Categories");
            }
            ViewBag.AllCategories = dbContext.Categories.ToArray();
            return View("Categories");
        }

        ///////////// END OF CRUD METHODS FOR CATEGORY MODEL /////////////

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

