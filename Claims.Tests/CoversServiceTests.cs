using Application.Services;
using Domain.Entities;
using Moq;
using Persistance.Interfaces;
using Xunit;

namespace Claims.Tests
{
    public class CoversServiceTests
    {
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
            var coversRepositoryMock = new Mock<ICoversRepository>();
            var auditsRepositoryMock = new Mock<IAuditsRepository>();

            coversRepositoryMock.Setup(repo => repo.AddItemAsync(It.IsAny<Cover>()))
                .Returns(Task.CompletedTask);

            auditsRepositoryMock.Setup(repo => repo.AuditCoverAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var coversService = new CoversService(coversRepositoryMock.Object, auditsRepositoryMock.Object);
            DateOnly start = DateOnly.Parse(startDate);
            DateOnly end = DateOnly.Parse(endDate);

            // Act
            decimal actualPremium = coversService.ComputePremium(start, end, coverType);

            // Assert
            Assert.Equal(expectedPremium, actualPremium);
        }
    }
}
