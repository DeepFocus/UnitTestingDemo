using System;
using MvcDemoSite.Controllers;
using MvcDemoSite.Models;
using NSubstitute;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class RateControllerTests
    {
        [Test]
        // Testing state of the model.
        public void IndexMethod_RateInCache_Returnes_Model()
        {
            // Arrange
            var cacheStub = Substitute.For<ICacheManager>();
            cacheStub.Get(Arg.Any<string>()).Returns(new CurrencyRate { Rate = 1.7m });

            var rateStub = Substitute.For<ICurrencyRateManager>();
            var fakeLogger = Substitute.For<ILog>();
            var controller = new HomeController(cacheStub, rateStub, fakeLogger);

            // Act
            var view = controller.Index();
            var model = view.Model as CurrencyRate;

            // Assert
            Assert.NotNull(model);
            Assert.That(model.Rate != Decimal.Zero);
        }

        [Test]
        public void IndexMethod_RateNotInCache_ReturnsViewAndModel()
        {
            // Arrange
            var cacheStub = Substitute.For<ICacheManager>();
            cacheStub.Get(Arg.Any<string>()).Returns(null);

            var rateManagerStub = Substitute.For<ICurrencyRateManager>();
            rateManagerStub.GetRate(Arg.Any<string>()).Returns(new CurrencyRate { Rate = 1.7m });

            var fakeLogger = Substitute.For<ILog>();

            var controller = new HomeController(cacheStub, rateManagerStub, fakeLogger);

            // Act
            var view = controller.Index();
            var model = view.Model as CurrencyRate;

            // Assert
            Assert.NotNull(model);
            Assert.That(model.Rate != Decimal.Zero);
        }

        [Test]
        public void IndexMethod_RateNotInCacheThrowsException_LogsErrorReturnsNullModel()
        {
            // Arrange  
            var cacheStub = Substitute.For<ICacheManager>();
            cacheStub.Get(Arg.Any<string>()).Returns(null);

            var rateManagerStub = Substitute.For<ICurrencyRateManager>();
            rateManagerStub.When(x => x.GetRate(Arg.Any<string>()))
                       .Do(x =>
                       {
                           throw new Exception("fake excption");
                       });

            var loggerMock = Substitute.For<ILog>();
            var controller = new HomeController(cacheStub, rateManagerStub, loggerMock);
                      

            // Act
            var view = controller.Index();
            var model = view.Model as CurrencyRate;

            // Assert
            loggerMock.Received().Error(Arg.Is<string>(x => x.Contains("Currency update failed")), Arg.Any<Exception>());
            Assert.Null(model);
        }
    }
}
