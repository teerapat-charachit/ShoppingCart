using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _context;

        public double GrandTotal { get; private set; }

        public CartController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };

            return View(cartVM);
        }

        public async Task<IActionResult> Add(long id)
        {
            Product product = await _context.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            TempData["Success"] = "The product has been added!";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Decrease(long id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "The product has been removed!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(long id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(p => p.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "The product has been removed!";

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");
        }

       
        public async Task<IActionResult> Create(long id) 
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            

            Orders orders = new Orders()
            {

                Orderdate = DateTime.Now,
                OrderTotal = (double)cart.Sum(x => x.Quantity * x.Price) 



            };
            _context.Add(orders);
            await _context.SaveChangesAsync();
           

            foreach (CartItem cartItem in cart) 
            {
                OrderDetails details = new OrderDetails()
                {
                    OrdersId = orders.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.ProductName, 
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price

                    
                };

                _context.Add(details);
                await _context.SaveChangesAsync();
               


            }
            HttpContext.Session.SetJson("Cart", cart);

            TempData["Success"] = "Check out complete";
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");
        }

    }


    
}
