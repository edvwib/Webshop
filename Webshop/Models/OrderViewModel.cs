using System.Collections.Generic;
using Webshop.Core.Models;

namespace Webshop.Models
{
    public class OrderViewModel
    {
        public CheckoutViewModel Address { get; set; }
        public List<UserCartModel> UserCart { get; set; }
    }
}
