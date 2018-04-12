using System.Collections.Generic;

namespace Webshop.Core.Models
{
    public class OrderModel
    {
        public string Guid { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
    }
}
