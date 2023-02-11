using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;

namespace ShoppingCart.Areas.Admin.Controllers
{
        
        public class ProductsController : Controller
        {
                private readonly DataContext _context;
                private readonly IWebHostEnvironment _webHostEnvironment;


                public ProductsController(DataContext context, IWebHostEnvironment webHostEnvironment)
                {
                        _context = context;
                        _webHostEnvironment = webHostEnvironment;
                }
        public async Task<IActionResult> Index(string categorySlug = "", int p = 1)
        {
            int pageSize = 6;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;

            if (categorySlug == "")
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

                return View(await _context.Products.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
            }

            Category category = await _context.Categories.Where(c => c.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null) return RedirectToAction("Index");

            var productsByCategory = _context.Products.Where(p => p.CategoryId == category.Id);
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsByCategory.Count() / pageSize);

            return View(await productsByCategory.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        public async Task<IActionResult> Index5()
                {
                    var mvcMovieContext = _context.Products.Include(p => p.Category);
                    return View(await mvcMovieContext.ToListAsync());
                }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
                {
                        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

                        return View();
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create(Product product)
                {
                        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

                        if (ModelState.IsValid)
                        {
                                product.Slug = product.Name.ToLower().Replace(" ", "-");

                                var slug = await _context.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                                if (slug != null)
                                {
                                        ModelState.AddModelError("", "The product already exists.");
                                        return View(product);
                                }

                                if (product.ImageUpload != null)
                                {
                                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                                        string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;

                                        string filePath = Path.Combine(uploadsDir, imageName);

                                        FileStream fs = new FileStream(filePath, FileMode.Create);
                                        await product.ImageUpload.CopyToAsync(fs);
                                        fs.Close();

                                        product.Image = imageName;
                                }

                                _context.Add(product);
                                await _context.SaveChangesAsync();

                                TempData["Success"] = "The product has been created!";

                                return RedirectToAction("Index");
                        }

                        return View(product);
                }

                public async Task<IActionResult> Edit(long id)
                {
                        Product product = await _context.Products.FindAsync(id);

                        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

                        return View(product);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, Product product)
                {
                        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

                        if (ModelState.IsValid)
                        {
                                product.Slug = product.Name.ToLower().Replace(" ", "-");

                                var slug = await _context.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                                if (slug != null)
                                {
                                        ModelState.AddModelError("", "The product already exists.");
                                        return View(product);
                                }

                                if (product.ImageUpload != null)
                                {
                                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                                        string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;

                                        string filePath = Path.Combine(uploadsDir, imageName);

                                        FileStream fs = new FileStream(filePath, FileMode.Create);
                                        await product.ImageUpload.CopyToAsync(fs);
                                        fs.Close();

                                        product.Image = imageName;
                                }

                                _context.Update(product);
                                await _context.SaveChangesAsync();

                                TempData["Success"] = "The product has been edited!";
                        }

                        return View(product);
                }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'MvcMovieContext.Product'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}