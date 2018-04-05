using System.Collections.Generic;
using Webshop.Core.Models;

namespace Webshop.Core.Repositories
{
    public interface ICartRepository
    {
        List<CartModel> GetAll(string guid);

        CartModel Get(string guid, int productId);

        bool Add(string guid, int productId);

        bool UpdateCount(string guid, int productId, int count);

        bool Remove(string guid, int productId);

        bool Empty(string guid);
    }
}
