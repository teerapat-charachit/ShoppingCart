
using Microsoft.CodeAnalysis;

namespace ShoppingCart.Models
{
    public partial class OrderDetails
    {
        
        public long Id { get; set; }

        
        public long  OrdersId { get; set; }
        
        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public virtual Orders Orders { get; set; }
        public virtual Product Product { get; set; }

    }
}
