using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using SQLitePCL;
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
        [TestCase("967d56bd-3899-")]
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
            var result = _cartService.GetAll(guid);

            //Assert
            Assert.That(result, Is.EqualTo(expectedCart));
        }

        [TestCase(0, "")]
        [TestCase(0, " ")]
        [TestCase(0, null)]
        [TestCase(0, "967d56bd-3899-")]
        [TestCase(0, "967d56bd-3899-4097-8548-e42a6968dea0")]
        [TestCase(1, "")]
        [TestCase(1, " ")]
        [TestCase(1, null)]
        [TestCase(1, "967d56bd-3899-")]
        public void Get_GivenInvalidData_ReturnsNull(int productId, string guid)
        {
            //Act
            var result = _cartService.Get(guid, productId);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_GivenValidData_ReturnsExpectedProduct()
        {
            //Arrange
            const string guid = "967d56bd-3899-4097-8548-e42a6968dea0";
            const int productId = 1;
            var expectedProduct = new CartModel{
                Guid = guid,
                Id = productId
            };

            A.CallTo(() => _cartRepository.Get(guid, productId)).Returns(expectedProduct);

            //Act
            var result = _cartService.Get(guid, productId);

            //Assert
            Assert.That(result, Is.EqualTo(expectedProduct));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("967d56bd-3899-")]
        public void GetTotal_GivenInvalidGuid_ReturnsZero(string testGuid)
        {
            //Arrange
            var result = _cartService.GetTotal(testGuid);

            //Assert
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void GetTotal_GivenValidGuid_ReturnsExpectedTotal()
        {
            //Arrange
            const string guid = "967d56bd-3899-4097-8548-e42a6968dea0";
            var expectedPrice = 10;
            var testCart = new List<CartModel>
            {
                new CartModel
                {
                    Guid = guid,
                    Id = 1,
                    Count = 1,
                    //Price = expectedPrice,
                }
            };

            A.CallTo(() => _cartRepository.GetTotal(guid)).Returns(expectedPrice);

            //Act
            var result = _cartService.GetTotal(guid);

            //Assert
            Assert.That(result, Is.EqualTo(expectedPrice));
        }

        [TestCase(0, "")]
        [TestCase(0, " ")]
        [TestCase(0, null)]
        [TestCase(0, "967d56bd-3899-")]
        [TestCase(0, "967d56bd-3899-4097-8548-e42a6968dea0")]
        [TestCase(1, "")]
        [TestCase(1, " ")]
        [TestCase(1, null)]
        [TestCase(1, "967d56bd-3899-")]
        public void Add_GivenInvalidData_ReturnsFalse(int productId, string guid)
        {
            //Act
            var result = _cartService.Add(guid, productId);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Add_GivenValidData_ReturnsTrue()
        {
            //Arrange
            const string guid = "967d56bd-3899-4097-8548-e42a6968dea0";
            const int productId = 1;

            A.CallTo(() => _productsRepository.Get(productId)).Returns(new ProductModel());
            A.CallTo(() => _cartRepository.Get(guid, productId)).Returns(null);
            A.CallTo(() => _cartRepository.Add(guid, productId)).Returns(true);

            //Act
            var result = _cartService.Add(guid, productId);

            //Assert
            Assert.That(result, Is.True);
        }

        [TestCase(0, 10, "967d56bd-3899-4097-8548-e42a6968dea0")]
        [TestCase(1, -10, "967d56bd-3899-4097-8548-e42a6968dea0")]
        [TestCase(1, 10, "967d56bd-3899")]
        public void UpdateCount_GivenInvalidData_ReturnsFalse(int productId, int count, string guid)
        {
            //Act
            var result = _cartService.UpdateCount(guid, productId, count);

            //Assert
            Assert.That(result, Is.False);
        }
    }
}
