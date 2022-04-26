using AutoMapper;
using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;
using Gambling.Backend.Profiles;
using Gambling.Backend.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Gambling.Backend.Tests.MockData
{
    internal class GamblingMockData
    {
        private static IMapper _mapper;
        public static Player GetPlayer()
        {
            var player = new Player
            {
                Name = "Player 1",
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Account = 10000,
                Bets = new List<Bet>()
            };
            return player;
        }
        public static CreatePlayerDto GetCreatePlayerDto()
        {
            return new CreatePlayerDto("Player 1");
        }
        public static IOptions<ServiceSettings> GetServiceSettingsOptions()
        {
            return Options.Create<ServiceSettings>(new ServiceSettings
            {
                StartingAccount = 1000,
                ServiceName = "Gambling"
            });
        }
        public static IMapper GetMapper()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new PlayerProfile());
                    mc.AddProfile(new BetProfile());
                });
                _mapper = mappingConfig.CreateMapper();
            }
            return _mapper;
        }
    }
}
