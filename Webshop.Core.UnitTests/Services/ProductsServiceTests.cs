using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using Webshop.Core.Models;
using Webshop.Core.Repositories;
using Webshop.Core.Services.Implementations;

namespace Webshop.Core.UnitTests.Services
{
    public class ProductsServiceTests
    {
        private ProductService _productService;
        private IProductsRepository _productsRepository;

        [SetUp]
        public void SetUp()
        {
            _productsRepository = A.Fake<IProductsRepository>();
            _productService = new ProductService(_productsRepository);
        }

        [Test]
        public void GetAll_ReturnsProductList()
        {
            //Arrange
            var products = new List<ProductModel>
            {
                new ProductModel { Id = 1 }
            };

            A.CallTo(() => _productsRepository.GetAll()).Returns(products);

            //Act
            var result = _productService.GetAll();

            //Assert
            Assert.That(result, Is.EqualTo(products));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Get_GivenIdLessThanOne_ReturnsNull(int id)
        {
            //Act
            var result = _productService.Get(id);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_GivenValidId_ReturnsExpectedProduct()
        {
            //Arrange
            const int id = 3;
            var expectedProductItem = new ProductModel { Id = id };

            A.CallTo(() => _productsRepository.Get(id)).Returns(expectedProductItem);

            //Act
            var result = _productService.Get(id);

            //Assert
            Assert.That(result, Is.EqualTo(expectedProductItem));
        }
    }
}
