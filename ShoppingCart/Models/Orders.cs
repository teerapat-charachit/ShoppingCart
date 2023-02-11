using Microsoft.CodeAnalysis;
using NSubstitute.ReceivedExtensions;

namespace ShoppingCart.Models;
    public class Orders
    {
        public long Id { get; set; }
        public DateTime Orderdate { get; set; } = DateTime.Now;

        public double OrderTotal { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }

       

}

