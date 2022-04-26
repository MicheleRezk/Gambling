using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;

namespace Gambling.Backend.Common;

public interface IBetServices
{
    int GetRandomNumber(int min, int max);
    BetResultDto CheckPlayerBet(int rand, int playerAccount, CreateBetDto createBetDto);
    Task SavePlayerBet(Player player, CreateBetDto createBetDto, int newAccount, Status status);
}