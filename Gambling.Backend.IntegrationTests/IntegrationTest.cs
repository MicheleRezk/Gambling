using Gambling.Backend.Common;
using Gambling.Backend.Entities;
using Gambling.Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gambling.Backend.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient _testClient;
        protected readonly Random _random = new Random();
        protected readonly IPlayerServices _playerServices;
        protected readonly IBetServices _betServices;
        protected readonly ISerializerService _serializerService;
        protected IntegrationTest()
        {
            var appFactory = new TestingWebAppFactory<Program>();
            _testClient = appFactory.CreateDefaultClient();
            this._serializerService = appFactory.Server.Services.GetService<ISerializerService>();
            this._playerServices = appFactory.Server.Services.GetService<IPlayerServices>();
        }
        protected async Task DeletePlayer(Guid playerId)
        {
            _playerServices.DeletePlayerAsync(playerId);
        }
        protected async Task<Player> CreatePlayerForTesting()
        {
            var player = new Player
            {
                Id = Guid.NewGuid(),
                Account = 10000,
                CreatedDate = DateTime.UtcNow,
                Name = $"Player_{_random.Next(1, int.MaxValue)}",
                Bets = new List<Bet>(),
            };
           await _playerServices.CreatePlayerAsync(player);
           return player;
        }
    }
}
