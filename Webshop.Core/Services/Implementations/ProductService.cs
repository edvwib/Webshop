using System.Collections.Generic;
using Webshop.Core.Models;
using Webshop.Core.Repositories;
using Webshop.Core.Repositories.Implementations;

namespace Webshop.Core.Services.Implementations
{
    public class ProductService
    {
        private readonly IProductsRepository _productsRepository;

        public ProductService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public List<ProductModel> GetAll()
        {
            return _productsRepository.GetAll();
        }

        public ProductModel Get(int id)
        {
            if (id <= 0)
                return null;

            return _productsRepository.Get(id);
        }

        public bool Create(ProductModel product)
        {
            return _productsRepository.Create(product);
        }

        public bool Edit(ProductModel product)
        {
            return _productsRepository.Edit(product);
        }
    }
}
