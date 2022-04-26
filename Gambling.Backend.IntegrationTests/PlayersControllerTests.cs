using FluentAssertions;
using Gambling.Backend.Dtos;
using System;
using System.Net.Http.Json;
using System.Net;
using Xunit;

namespace Gambling.Backend.IntegrationTests
{
    public class PlayersControllerTests: IntegrationTest
    {
        
        [Fact]
        public async void PostAsync_ShouldReturnCreatedStatus()
        {
            //Arrange
            var rand = _random.Next(1, int.MaxValue);
            var createPlayerDto = new CreatePlayerDto($"Player {rand}");

            //Act
            var response = await _testClient.PostAsJsonAsync(ApiRoutes.Players.Post, createPlayerDto);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var playerDto = _serializerService.Deserialize<PlayerDto>(content);
            Assert.NotNull(playerDto);
            playerDto.Name.Should().Be(createPlayerDto.Name);

            //Cleanup
            await DeletePlayer(playerDto.Id);
        }

        [Fact]
        public async void GetByIdAsync_WithIdNotExists_ShouldReturnNotFoundStatus()
        {
            //Arrange
            var url = ApiRoutes.Players.GetById.Replace("{playerId}", Guid.NewGuid().ToString());

            //Act
            var response = await _testClient.GetAsync(url);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void GetByIdAsync_WithIdExists_ShouldReturnOkStatus()
        {
            //Arrange
            var player = await CreatePlayerForTesting();
            var url = ApiRoutes.Players.GetById.Replace("{playerId}", player.Id.ToString());

            //Act
            var response = await _testClient.GetAsync(url);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var playerDto = _serializerService.Deserialize<PlayerDto>(content);
            Assert.NotNull(playerDto);
            playerDto.Name.Should().Be(player.Name);

            //Cleanup
            await DeletePlayer(player.Id);
        }


    }
}