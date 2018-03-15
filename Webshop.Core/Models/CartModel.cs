namespace Webshop.Core.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
