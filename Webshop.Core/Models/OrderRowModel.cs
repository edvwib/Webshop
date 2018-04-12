namespace Webshop.Core.Models
{
    public class OrderRowModel
    {
        public string Guid { get; set; }
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
