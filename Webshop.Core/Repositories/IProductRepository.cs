﻿using System.Collections.Generic;
using Webshop.Core.Models;

namespace Webshop.Core.Repositories
{
    public interface IProductRepository
    {
        List<ProductModel> GetAll();

        ProductModel Get(int id);

        bool Create(ProductModel product);

        bool Edit(ProductModel product);
    }
}
