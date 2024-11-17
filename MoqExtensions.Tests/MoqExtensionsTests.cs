using Moq;

namespace MoqExtensions.Tests
{
    public class MoqExtensionsTests
    {
        private readonly Mock<IFooService> _fooServiceMock = new();

        [Fact]
        public async Task MoqExtensions_WhenSetupSimpleAsync_ThenMoq()
        {
            _fooServiceMock.SetupSimpleAsync<IFooService, string>(nameof(IFooService.DoFooAsync))
                .ReturnsAsync("Moq");

            string? result = await _fooServiceMock.Object.DoFooAsync(
                default,
                default,
                default,
                default);
            Assert.Equal(
                "Moq",
                result);
        }


        [Fact]
        public void MoqExtensions_WhenSetupSimple_ThenMoq()
        {
            _fooServiceMock.SetupSimple<IFooService, string>(nameof(IFooService.DoFoo))
                .Returns("Moq");

            string? result = _fooServiceMock.Object.DoFoo(
                default,
                default,
                default);
            Assert.Equal(
                "Moq",
                result);
        }

        [Fact]
        public void MoqExtensions_WhenVerifySimpleAsync_ThenMoq()
        {
            _fooServiceMock.Object.DoFooAsync(
                default,
                default,
                default,
                default);
            _fooServiceMock.VerifySimpleAsync(
                nameof(IFooService.DoFooAsync),
                Times.Once);
        }

        [Fact]
        public void MoqExtensions_WhenVerifySimple_ThenMoq()
        {
            _fooServiceMock.Object.DoFoo(
                default,
                default,
                default);
            _fooServiceMock.VerifySimple(
                nameof(IFooService.DoFoo),
                Times.Once);
        }
    }
}