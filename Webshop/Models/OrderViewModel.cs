using System.Collections.Generic;

namespace Webshop.Models
{
    public class OrderViewModel
    {
        public CheckoutViewModel Address { get; set; }
        public List<UserCartViewModel> UserCart { get; set; }
    }
}
