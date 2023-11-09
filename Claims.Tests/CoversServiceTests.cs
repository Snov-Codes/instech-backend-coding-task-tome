using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Claims.Tests
{
    public class CoversServiceTests : IClassFixture<CoversServiceTestFixture>
    {
        private readonly CoversService _coversService;
        private readonly Mock<ICoversRepository> _coversRepositoryMock;
        private readonly Mock<IAuditsRepository> _auditsRepositoryMock;

        public CoversServiceTests(CoversServiceTestFixture fixture)
        {
            _coversRepositoryMock = fixture.CoversRepositoryMock;
            _auditsRepositoryMock = fixture.AuditsRepositoryMock;
            _coversService = new CoversService(_coversRepositoryMock.Object, _auditsRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateCoverAsync_WithinAYear_Success()
        {
            // Arrange

            var cover = new Cover
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                Type = CoverType.Yacht
            };

            // Act
            var result = await _coversService.CreateCoverAsync(cover);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateCoverAsync_PastStartDate_Failure()
        {
            // Arrange

            var cover = new Cover
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                Type = CoverType.Yacht
            };

            // Act
            var result = await _coversService.CreateCoverAsync(cover);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Cover start date cannot be in the past!", result.Error);
        }

        [Fact]
        public async Task CreateCoverAsync_MoreThanAYear_Failure()
        {
            // Arrange
            var cover = new Cover
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddYears(6)),
                Type = CoverType.Yacht
            };

            // Act
            var result = await _coversService.CreateCoverAsync(cover);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Start and End dates of the Cover must be within a one year time span", result.Error);
            _coversRepositoryMock.Verify(mock => mock.AddItemAsync(cover), Times.Never);
            _auditsRepositoryMock.Verify(mock => mock.AuditCoverAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("2023-11-01", "2023-11-30", CoverType.Yacht, 41250)]
        [InlineData("2023-11-01", "2023-11-30", CoverType.PassengerShip, 45000)]
        [InlineData("2023-11-01", "2023-11-30", CoverType.Tanker, 56250)]
        [InlineData("2023-11-01", "2023-11-30", CoverType.BulkCarrier, 48750)]
        [InlineData("2023-11-01", "2024-03-30", CoverType.Yacht, 199306.25)]
        [InlineData("2023-11-01", "2024-03-30", CoverType.PassengerShip, 222870)]
        [InlineData("2023-11-01", "2024-03-30", CoverType.Tanker, 278587.5)]
        [InlineData("2023-11-01", "2024-03-30", CoverType.BulkCarrier, 241442.5)]
        [InlineData("2023-11-01", "2024-10-31", CoverType.Yacht, 472477.5)]
        [InlineData("2023-11-01", "2024-10-31", CoverType.PassengerShip, 536130)]
        [InlineData("2023-11-01", "2024-10-31", CoverType.Tanker, 670162.5)]
        [InlineData("2023-11-01", "2024-10-31", CoverType.BulkCarrier, 580807.5)]
        public void ComputePremium_CalculatesCorrectly(string startDate, string endDate, CoverType coverType, decimal expectedPremium)
        {
            // Arrange
            DateOnly start = DateOnly.Parse(startDate);
            DateOnly end = DateOnly.Parse(endDate);

            // Act
            decimal actualPremium = _coversService.ComputePremium(start, end, coverType);

            // Assert
            Assert.Equal(expectedPremium, actualPremium);
        }
    }

    public class CoversServiceTestFixture : IDisposable
    {
        public Mock<ICoversRepository> CoversRepositoryMock { get; }
        public Mock<IAuditsRepository> AuditsRepositoryMock { get; }

        public CoversServiceTestFixture()
        {
            CoversRepositoryMock = new Mock<ICoversRepository>();
            AuditsRepositoryMock = new Mock<IAuditsRepository>();
        }

        public void Dispose(){}
    }
}
