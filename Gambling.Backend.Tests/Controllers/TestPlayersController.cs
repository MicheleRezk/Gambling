using FluentAssertions;
using Gambling.Backend.Common;
using Gambling.Backend.Controllers;
using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;
using Gambling.Backend.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Gambling.Backend.Tests.Controllers
{
    public class TestPlayersController
    {
        [Fact]
        public async Task GetByIdAsync_ShouldReturn200Status()
        {
            // Arrange
            var playerServices = new Mock<IPlayerServices>();
            var mapper = GamblingMockData.GetMapper();
            var mockPlayer = GamblingMockData.GetPlayer();
            var serviceSettings = GamblingMockData.GetServiceSettingsOptions();
            playerServices.Setup(_ => _.GetPlayerAsync(It.IsAny<Guid>())).ReturnsAsync(mockPlayer);
            var sut = new PlayersController(mapper,playerServices.Object, serviceSettings);

            // Act
            var actionResult = await sut.GetByIdAsync(mockPlayer.Id);


            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            var playerResponse = result.Value as PlayerDto;
            playerResponse.Should().NotBeNull();
            Assert.Equal(new PlayerDto(mockPlayer.Id,mockPlayer.Name, mockPlayer.Account), playerResponse);

        }
        [Fact]
        public async Task GetAsync_WithPlayerNotExists_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var serviceSettings = GamblingMockData.GetServiceSettingsOptions();
            var mapper = GamblingMockData.GetMapper();
            var playerServices = new Mock<IPlayerServices>();
            playerServices.Setup(_ => _.GetPlayerAsync(It.IsAny<Guid>())).ReturnsAsync(() => { return null; });
            var sut = new PlayersController(mapper, playerServices.Object, serviceSettings);
            // Act
            var actionResult = await sut.GetByIdAsync(Guid.NewGuid());


            // Assert
            var result = actionResult.Result as NotFoundResult;
            result.Should().NotBeNull();

        }
        [Fact]
        public async Task PostAsync_ShouldReturn201Status()
        {
            // Arrange
            var playerServices = new Mock<IPlayerServices>();
            var mapper = GamblingMockData.GetMapper();
            var serviceSettings = GamblingMockData.GetServiceSettingsOptions();
            playerServices.Setup(_ => _.CreatePlayerAsync(It.IsAny<Player>())).Returns((Task.CompletedTask));
            var sut = new PlayersController(mapper, playerServices.Object, serviceSettings);

            // Act
            var actionResult = await sut.PostAsync(GamblingMockData.GetCreatePlayerDto());


            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
            var playerResponse = result.Value as PlayerDto;
            playerResponse.Should().NotBeNull();

        }
    }
}
