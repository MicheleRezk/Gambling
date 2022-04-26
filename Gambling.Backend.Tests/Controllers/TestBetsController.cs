using FluentAssertions;
using Gambling.Backend.Common;
using Gambling.Backend.Controllers;
using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;
using Gambling.Backend.Services;
using Gambling.Backend.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Gambling.Backend.Tests.Controllers
{
    public class TestBetsController
    {

        [Fact]
        public async Task PostAsync_WithPlayerNotExists_ShouldReturnBadRequest()
        {
            // Arrange
            var playerServices = new Mock<IPlayerServices>();
            playerServices.Setup(_ => _.GetPlayerAsync(It.IsAny<Guid>())).ReturnsAsync(() => { return null; });
            var betServices = new Mock<IBetServices>();
            var sut = new BetsController(playerServices.Object, betServices.Object);

            // Act
            var actionResult = await sut.PostAsync(new CreateBetDto(Guid.NewGuid(), 100, 3));


            // Assert
            var result = actionResult.Result as BadRequestObjectResult;
            result.Should().NotBeNull();
        }

        /// <summary>
        /// Number Range between 0 & 9
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(10)]
        [InlineData(-1)]
        public async Task PostAsync_WithNumberOutsideRange_ShouldReturnBadRequest(int number)
        {
            // Arrange
            var mockPlayer = GamblingMockData.GetPlayer();
            var playerServices = new Mock<IPlayerServices>();
            playerServices.Setup(_ => _.GetPlayerAsync(It.IsAny<Guid>())).ReturnsAsync(mockPlayer);
            var betServices = new Mock<IBetServices>();
            var sut = new BetsController(playerServices.Object, betServices.Object);

            // Act
            var actionResult = await sut.PostAsync(new CreateBetDto(Guid.NewGuid(), 100, number));


            // Assert
            var result = actionResult.Result as BadRequestObjectResult;
            result.Should().NotBeNull();
        }


        /// <summary>
        /// Points should be greater than zero and less or equal player's Account 
        /// (mock player with account: 10000)
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(10001)]
        [InlineData(15000)]
        public async Task PostAsync_WithPointsOutsideRange_ShouldReturnBadRequest(int points)
        {
            // Arrange
            var mockPlayer = GamblingMockData.GetPlayer();
            var playerServices = new Mock<IPlayerServices>();
            playerServices.Setup(_ => _.GetPlayerAsync(It.IsAny<Guid>())).ReturnsAsync(mockPlayer);
            var betServices = new Mock<IBetServices>();
            var sut = new BetsController(playerServices.Object, betServices.Object);

            // Act
            var actionResult = await sut.PostAsync(new CreateBetDto(Guid.NewGuid(), points, 3));


            // Assert
            var result = actionResult.Result as BadRequestObjectResult;
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_WithCorrectBet_ShouldReturnWonResult()
        {
            // Arrange
            var number = 3; var points = 100; var expectedEarnedPoints = points * 9;
            var mockPlayer = GamblingMockData.GetPlayer();
            var createBetDto = new CreateBetDto(mockPlayer.Id, points, number);
            var playerMockRepo = new Mock<IRepository<Player>>();
            var playerServices = new Mock<IPlayerServices>();
            playerServices.Setup(_ => _.GetPlayerAsync(It.IsAny<Guid>())).ReturnsAsync(mockPlayer);
            var betServices = new Mock<IBetServices>();
            var betServicesImplemented = new BetServices(GamblingMockData.GetMapper(),playerMockRepo.Object);
            betServices.Setup(_ => _.GetRandomNumber(It.IsAny<int>(), It.IsAny<int>())).Returns(number);
            betServices.Setup(_ => _.SavePlayerBet(It.IsAny<Player>(), It.IsAny<CreateBetDto>(), It.IsAny<int>(), It.IsAny<Status>()))
                .Returns(Task.CompletedTask);
            betServices.Setup(_ => _.CheckPlayerBet(number, mockPlayer.Account, createBetDto))
                .Returns<int, int, CreateBetDto>((x, y, z) => betServicesImplemented.CheckPlayerBet(x, y, z));

            var sut = new BetsController(playerServices.Object, betServices.Object);

            // Act
            var actionResult = await sut.PostAsync(createBetDto);


            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            var betResult = result.Value as BetResultDto;
            betResult.Should().NotBeNull();
            betResult.Status.Should().Be(Status.Won.ToString());
            betResult.Account.Should().Be(mockPlayer.Account+expectedEarnedPoints);
            betResult.Points.Should().Be($"+{expectedEarnedPoints}");
        }

        [Fact]
        public async Task PostAsync_WithWrongBet_ShouldReturnLoseResult()
        {
            // Arrange
            var number = 3; var points = 100; var expectedEarnedPoints = -points;var randNum = 4; 
            var mockPlayer = GamblingMockData.GetPlayer();
            var createBetDto = new CreateBetDto(mockPlayer.Id, points, number);
            var playerMockRepo = new Mock<IRepository<Player>>();
            var playerServices = new Mock<IPlayerServices>();
            playerServices.Setup(_ => _.GetPlayerAsync(It.IsAny<Guid>())).ReturnsAsync(mockPlayer);
            var betServices = new Mock<IBetServices>();
            var betServicesImplemented = new BetServices(GamblingMockData.GetMapper(), playerMockRepo.Object);
            betServices.Setup(_ => _.GetRandomNumber(It.IsAny<int>(), It.IsAny<int>())).Returns(randNum);
            betServices.Setup(_ => _.SavePlayerBet(It.IsAny<Player>(), It.IsAny<CreateBetDto>(), It.IsAny<int>(), It.IsAny<Status>()))
                .Returns(Task.CompletedTask);
            betServices.Setup(_ => _.CheckPlayerBet(randNum, mockPlayer.Account, createBetDto))
                .Returns<int, int, CreateBetDto>((x, y, z) => betServicesImplemented.CheckPlayerBet(x, y, z));

            var sut = new BetsController(playerServices.Object, betServices.Object);

            // Act
            var actionResult = await sut.PostAsync(createBetDto);


            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            var betResult = result.Value as BetResultDto;
            betResult.Should().NotBeNull();
            betResult.Status.Should().Be(Status.Lose.ToString());
            betResult.Account.Should().Be(mockPlayer.Account + expectedEarnedPoints);
            betResult.Points.Should().Be($"{expectedEarnedPoints}");
        }
    }
}
