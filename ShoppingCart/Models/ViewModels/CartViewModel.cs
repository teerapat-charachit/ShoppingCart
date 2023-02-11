using Microsoft.CodeAnalysis;

namespace ShoppingCart.Models.ViewModels
{
        public class CartViewModel
        {
                public List<CartItem> CartItems { get; set; }

                public decimal GrandTotal { get; set; }

                 public List<Orders> OrderTotal { get; set; }

                public List<OrderDetails> ProductId { get; set; }

                public List<OrderDetails> ProductName { get; set; }

               public List<OrderDetails> Quantity { get; set; }

                public List<OrderDetails> Price { get; set; }
    }
}
