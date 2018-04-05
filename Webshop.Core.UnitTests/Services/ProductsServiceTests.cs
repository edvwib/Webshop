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
        private IProductRepository _productRepository;

        [SetUp]
        public void SetUp()
        {
            _productRepository = A.Fake<IProductRepository>();
            _productService = new ProductService(_productRepository);
        }

        [Test]
        public void GetAll_ReturnsProductList()
        {
            //Arrange
            var expectedProducts = new List<ProductModel>
            {
                new ProductModel { Id = 1 }
            };

            A.CallTo(() => _productRepository.GetAll()).Returns(expectedProducts);

            //Act
            var result = _productService.GetAll();

            //Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Get_GivenInvalidId_ReturnsNull(int id)
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

            A.CallTo(() => _productRepository.Get(id)).Returns(expectedProductItem);

            //Act
            var result = _productService.Get(id);

            //Assert
            Assert.That(result, Is.EqualTo(expectedProductItem));
        }
    }
}
