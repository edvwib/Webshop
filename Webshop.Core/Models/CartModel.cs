namespace Webshop.Core.Models
{
    public class CartModel
    {
//        public int Id { get; set; }
        public string Guid { get; set; }
        public int Id { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
