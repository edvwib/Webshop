using System.Collections.Generic;

namespace Webshop.Core.Models
{
    public class AddressModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
