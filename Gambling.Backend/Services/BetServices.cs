using AutoMapper;
using Gambling.Backend.Common;
using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;

namespace Gambling.Backend.Services;

/// <summary>
/// Business logic of bets 
/// </summary>
public class BetServices : IBetServices
{
    private readonly Random _randNumber = new();
    private readonly IRepository<Player> _playersRepo;
    private readonly IMapper _mapper;

    public BetServices(IMapper mapper, IRepository<Player> playersRepo)
    {
        this._mapper = mapper;
        this._playersRepo = playersRepo;
    }

    /// <summary>
    /// Get Random number between min & max
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public int GetRandomNumber(int min, int max)
    {
        return _randNumber.Next(min, max);
    }

    /// <summary>
    /// Check player bet and return the result
    /// </summary>
    /// <param name="rand"></param>
    /// <param name="playerAccount"></param>
    /// <param name="createBetDto"></param>
    /// <returns></returns>
    public BetResultDto CheckPlayerBet(int rand, int playerAccount, CreateBetDto createBetDto)
    {
        var isPlayerWon = rand == createBetDto.Number;
        var earnedPoints = isPlayerWon ? (createBetDto.Points * 9) : -createBetDto.Points;
        var newAccount = playerAccount + earnedPoints;
        var status = isPlayerWon ? Status.Won : Status.Lose;
        var result = new BetResultDto(newAccount, status.ToString(), earnedPoints.ToString("+#;-#;0"));
        return result;
    }

    /// <summary>
    /// Save the player's bet to the DB
    /// </summary>
    /// <param name="player"></param>
    /// <param name="createBetDto"></param>
    /// <param name="newAccount"></param>
    /// <param name="status"></param>
    public async Task SavePlayerBet(Player player, CreateBetDto createBetDto, int newAccount, Status status)
    {
        var bet = _mapper.Map<Bet>(createBetDto);
        bet.CreatedDate = DateTimeOffset.Now;
        bet.Status = status;
        player.AddNewBet(bet);
        player.Account = newAccount;
        await _playersRepo.UpdateAsync(player);
    }
}