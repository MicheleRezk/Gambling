using AutoMapper;
using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;

namespace Gambling.Backend.Profiles;

public class PlayerProfile : Profile
{
    public PlayerProfile()
    {
        // Source -> Target
        CreateMap<Player, PlayerDto>();
    }
}