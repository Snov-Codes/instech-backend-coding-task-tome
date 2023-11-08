using Application.Services;
using Domain.Entities;
using Moq;
using Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Claims.Tests
{
    public class ClaimsServiceTests : IClassFixture<ClaimsServiceTestFixture>
    {
        private readonly ClaimsService _claimsService;
        private readonly Mock<ICoversRepository> _coversRepositoryMock;
        private readonly Mock<IAuditsRepository> _auditsRepositoryMock;
        private readonly Mock<IClaimsRepository> _claimsRepositoryMock;

        public ClaimsServiceTests(ClaimsServiceTestFixture fixture)
        {
            _claimsRepositoryMock = fixture.ClaimsRepositoryMock;
            _coversRepositoryMock = fixture.CoversRepositoryMock;
            _auditsRepositoryMock = fixture.AuditsRepositoryMock;
            _claimsService = new ClaimsService(_claimsRepositoryMock.Object, _auditsRepositoryMock.Object, _coversRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateClaimAsync_ValidClaim_ReturnsSuccessResult()
        {
            var validClaim = new Claim
            {
                CoverId = "valid-cover-id",
                Created = DateTime.Now.AddDays(2),
                DamageCost = 50000
            };

            var cover = new Cover
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5))
            };

            _coversRepositoryMock.Setup(repo => repo.GetItemAsync(It.IsAny<string>())).ReturnsAsync(cover);

            // Act
            var result = await _claimsService.CreateClaimAsync(validClaim);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateClaimAsync_CoverNotFound_ReturnsFailureResult()
        {
            // Arrange
            _coversRepositoryMock.Setup(repo => repo.GetItemAsync(It.IsAny<string>())).ReturnsAsync((Cover)null);

            // Act
            var result = await _claimsService.CreateClaimAsync(new Claim());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Cover is required when creating a Claim!", result.Error);
        }

        [Fact]
        public async Task CreateClaimAsync_InvalidClaim_ReturnsFailureResult()
        {
            // Arrange
            var invalidClaim = new Claim
            {
                CoverId = "valid-cover-id",
                Created = DateTime.Now.AddDays(2),
                DamageCost = 150000
            };

            // Act
            var result = await _claimsService.CreateClaimAsync(invalidClaim);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("The damage cost of the Claim cannot exceed 100.000!", result.Error);
        }

        [Fact]
        public async Task CreateClaimAsync_ClaimOutsideCoverDates_ReturnsFailureResult()
        {
            // Arrange
            var invalidClaim = new Claim
            {
                CoverId = "valid-cover-id",
                Created = DateTime.Now.AddDays(10),
                DamageCost = 50000
            };

            var cover = new Cover
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5))
            };

            _coversRepositoryMock.Setup(repo => repo.GetItemAsync(It.IsAny<string>())).ReturnsAsync(cover);

            // Act
            var result = await _claimsService.CreateClaimAsync(invalidClaim);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Claim creation date must be within the Cover start and end dates!", result.Error);
        }

    }

    public class ClaimsServiceTestFixture : IDisposable
    {
        public Mock<ICoversRepository> CoversRepositoryMock { get; }
        public Mock<IAuditsRepository> AuditsRepositoryMock { get; }
        public Mock<IClaimsRepository> ClaimsRepositoryMock { get; }

        public ClaimsServiceTestFixture()
        {
            CoversRepositoryMock = new Mock<ICoversRepository>();
            AuditsRepositoryMock = new Mock<IAuditsRepository>();
            ClaimsRepositoryMock = new Mock<IClaimsRepository>();
        }

        public void Dispose() { }
    }
}
