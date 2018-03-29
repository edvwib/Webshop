using System.Collections.Generic;

namespace Webshop.Core.Models
{
    public class OrderModel
    {
        public string Guid { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<CartModel> Cart { get; set; }
    }
}
