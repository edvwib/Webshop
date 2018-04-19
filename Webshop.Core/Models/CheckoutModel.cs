using System.Collections.Generic;

namespace Webshop.Core.Models
{
    public class CheckoutModel
    {
        public OrderModel Order { get; set; }
        public List<CartModel> Cart { get; set; }

        public AddressModel Address { get; set; }
    }
}
