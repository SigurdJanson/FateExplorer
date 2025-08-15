using FateExplorer.Shared.ClientSideStorage;
using Microsoft.JSInterop;
using jsVoidResult = Microsoft.JSInterop.Infrastructure.IJSVoidResult;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace UnitTests.Shared.ClientSideStorage
{
    [TestFixture]
    public class CookieStorageTests
    {
        private MockRepository mockRepository;

        private Mock<IJSRuntime> mockJSRuntime;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockJSRuntime = this.mockRepository.Create<IJSRuntime>();
        }


        private CookieStorage CreateCookieStorage()
        {
            return new CookieStorage(mockJSRuntime.Object);
        }



        [Test]
        [TestCase("Any", "Setthis")]
        public async Task SetValue_ValidContent(string key, string value)
        {
            int? days = null;

            // Arrange
            var cookieStorage = this.CreateCookieStorage();
            mockJSRuntime.Setup(o 
                => o.InvokeAsync<jsVoidResult>(
                    It.Is<string>(s => s == "eval"), 
                    It.IsAny<object[]>()))
                .Returns(new ValueTask<jsVoidResult>());

            // Act
            await cookieStorage.Store(key, value, days);

            // Assert
            mockRepository.VerifyAll();
            mockJSRuntime.Verify(o 
                => o.InvokeAsync<jsVoidResult>(It.Is<string>(s => s == "eval"), It.IsAny<object[]>()), 
                Times.Once);
        }



        [Test]
        [TestCase("Any/Ω/this", "Setthis", Description = "Test with non-ascii character in key throws exception.")]
        public void SetValue_NonAsciiKey_Exception(string key, string value)
        {
            int? days = null;

            // Arrange
            var cookieStorage = this.CreateCookieStorage();
            mockJSRuntime.Setup(o
                => o.InvokeAsync<jsVoidResult>(
                    It.Is<string>(s => s == "eval"), It.IsAny<object[]>()))
                .Returns(new ValueTask<jsVoidResult>());

            // Act
            Assert.ThrowsAsync<ArgumentException>(async () => await cookieStorage.Store(key, value, days));

            // Assert
            mockJSRuntime.Verify(o
                => o.InvokeAsync<jsVoidResult>(It.Is<string>(s => s == "eval"), It.IsAny<object[]>()),
                Times.Never);
        }


        [Test]
        [TestCase("a", "object1")]
        [TestCase("b", " object2")]
        [TestCase("c", "object3")]
        [TestCase("d", " obj@ct4")]
        [TestCase("e", " obj {with} space")]
        [TestCase("f", "f+object")]
        public async Task GetValue_ParseListCorrectly_ReturnsValue(string key, string expected)
        {
            const string TestCookie = "a=object1; b= object2; c =object3; d =%20obj%40ct4; e =%20obj%20%7Bwith%7D%20space; f=f+object";
            // Arrange
            var cookieStorage = CreateCookieStorage();
            mockJSRuntime.Setup(o
                => o.InvokeAsync<string>(
                    It.Is<string>(s => s == "eval"), It.IsAny<object[]>()))
                .Returns(new ValueTask<string>(TestCookie));

            string myDefault = null;

            // Act
            var result = await cookieStorage.Retrieve(key, myDefault);

            // Assert
            Assert.That(expected, result);
            mockRepository.VerifyAll();
            mockJSRuntime.Verify(o
                => o.InvokeAsync<string>(It.Is<string>(s => s == "eval"), It.IsAny<object[]>()),
                Times.Once);
        }


        [Test]
        [TestCase("x", "Any string that may work as default 123")]
        public async Task GetValue_ValueNotFound_ReturnsDefault(string key, string expected)
        {
            const string TestCookie = "a=object1; b= object2; c =object3; d = object4; e = obj with space; f=f+object";
            // Arrange
            var cookieStorage = CreateCookieStorage();
            mockJSRuntime.Setup(o
                => o.InvokeAsync<string>(
                    It.Is<string>(s => s == "eval"), It.IsAny<object[]>()))
                .Returns(new ValueTask<string>(TestCookie));

            // Act
            var result = await cookieStorage.Retrieve(key, defaultVal: expected);

            // Assert
            Assert.AreEqual(expected, result);
            mockRepository.VerifyAll();
            mockJSRuntime.Verify(o
                => o.InvokeAsync<string>(It.Is<string>(s => s == "eval"), It.IsAny<object[]>()),
                Times.Once);
        }
    }
}
