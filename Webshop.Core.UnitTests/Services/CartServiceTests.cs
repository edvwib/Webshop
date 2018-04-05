using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using Webshop.Core.Models;
using Webshop.Core.Repositories;
using Webshop.Core.Services;
using Webshop.Core.Services.Implementations;

namespace Webshop.Core.UnitTests.Services
{
    public class CartServiceTests
    {
        private ICartService _cartService;
        private ICartRepository _cartRepository;
        private IProductRepository _productsRepository;

        [SetUp]
        public void SetUp()
        {
            _cartRepository = A.Fake<ICartRepository>();
            _productsRepository = A.Fake<IProductRepository>();

            _cartService = new CartService(_cartRepository, new ProductService(_productsRepository));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("967d56bd-3899")]
        public void GetAll_GivenInvalidGuid_ReturnsNull(string testGuid)
        {
            //Arrange
            var result = _cartService.GetAll(testGuid);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAll_GivenValidGuid_ReturnsExpectedCart()
        {
            //Arrange
            const string guid = "967d56bd-3899-4097-8548-e42a6968dea0";
            var expectedCart = new List<CartModel>
            {
                new CartModel{Guid = guid}
            };

            A.CallTo(() => _cartRepository.GetAll(guid)).Returns(expectedCart);

            //Act

            //Assert
        }
    }
}
