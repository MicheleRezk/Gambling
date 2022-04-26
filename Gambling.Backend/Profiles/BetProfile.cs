using AutoMapper;
using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;

namespace Gambling.Backend.Profiles;

public class BetProfile : Profile
{
    public BetProfile()
    {
        // Source -> Target
        CreateMap<CreateBetDto, Bet>();
    }
}