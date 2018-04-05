using Webshop.Core.Models;

namespace Webshop.Core.Repositories
{
    public interface IOrderRepository
    {
        OrderModel GetAddress(string guid);

        bool AddAddress(OrderModel address);

        OrderModel GetOrder(string guid);

        bool AddOrder(string guid);
    }
}
