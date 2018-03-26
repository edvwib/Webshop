using System.Collections.Generic;
using Webshop.Models;

namespace Webshop.Core.Models
{
    public class OrderModel
    {
        public CheckoutModel Address { get; set; }
        public List<CartModel> Cart { get; set; }
    }
}

