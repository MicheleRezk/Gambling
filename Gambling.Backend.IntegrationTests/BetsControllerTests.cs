using FluentAssertions;
using Gambling.Backend.Dtos;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using System;
using Gambling.Backend.Common;

namespace Gambling.Backend.IntegrationTests
{
    public class BetsControllerTests: IntegrationTest
    {
        
        [Fact]
        public async void PostAsync_WithPlayerNotExists_ShouldReturnBadRequest()
        {
            //Arrange
            var createBetDto = new CreateBetDto(Guid.NewGuid(), 100, 3);

            //Act
            var response = await _testClient.PostAsJsonAsync(ApiRoutes.Bets.Post, createBetDto);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void PostAsync_WithNumberOutsideRange_ShouldReturnBadRequest()
        {
            //Arrange
            var createBetDto = new CreateBetDto(Guid.NewGuid(), 100, 50);

            //Act
            var response = await _testClient.PostAsJsonAsync(ApiRoutes.Bets.Post, createBetDto);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void PostAsync_WithPointsOutsideRange_ShouldReturnBadRequest()
        {
            //Arrange
            var createBetDto = new CreateBetDto(Guid.NewGuid(), -1, 3);

            //Act
            var response = await _testClient.PostAsJsonAsync(ApiRoutes.Bets.Post, createBetDto);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void PostAsync_ShouldReturnOkResponse()
        {
            //Arrange
            var points = 100; var expectedEarnedPoints = points * 9; var expectedLostPoints = -points;
            var player = await CreatePlayerForTesting();
            var createBetDto = new CreateBetDto(player.Id, points, 3);

            //Act
            var response = await _testClient.PostAsJsonAsync(ApiRoutes.Bets.Post, createBetDto);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var resultDto = _serializerService.Deserialize<BetResultDto>(content);
            Assert.NotNull(resultDto);
            if(resultDto.Status == Status.Won.ToString())
            {
                resultDto.Account.Should().Be(player.Account + expectedEarnedPoints);
                resultDto.Points.Should().Be($"+{expectedEarnedPoints}");
            }
            else
            {
                resultDto.Account.Should().Be(player.Account + expectedLostPoints);
                resultDto.Points.Should().Be($"{expectedLostPoints}");
            }

            //Cleanup
            await DeletePlayer(player.Id);
        }



    }
}