using AutoMapper;
using Gambling.Backend.Common;
using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gambling.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BetsController : ControllerBase
{
    private readonly IRepository<Player> _playersRepo;
    private readonly IBetServices _betServices;

    public BetsController(IRepository<Player> playersRepo, IBetServices betServices)
    {
        this._playersRepo = playersRepo;
        this._betServices = betServices;
    }

    // POST /bets
    [HttpPost]
    public async Task<ActionResult<BetResultDto>> PostAsync(CreateBetDto createBetDto)
    {
        //TODO: random min & max can be added to configuration 
        var rand = _betServices.GetRandomNumber(0, 9);
        Console.WriteLine($"--> Creating a new bet for player, random number:{rand}, bet info: {createBetDto}");

        //Validate player request
        var player = await _playersRepo.GetAsync(createBetDto.PlayerId);
        if (player == null)
        {
            return BadRequest($"Player with Id {createBetDto.PlayerId} not exists");
        }

        if (createBetDto.Number < 0 || createBetDto.Number > 9)
        {
            return BadRequest($"Gambling number must be between zero and 9");
        }

        if (createBetDto.Points <= 0)
        {
            return BadRequest("Gambling Points must be greater than zero");
        }

        if (player.Account < createBetDto.Points)
        {
            return BadRequest($"Player's Account:{player.Account} is less than Gambling Points:{createBetDto.Points}");
        }

        //check the bet and get the result
        var result = _betServices.CheckPlayerBet(rand, player.Account, createBetDto);

        //Save to DB
        _betServices.SavePlayerBet(player, createBetDto, result.Account, Enum.Parse<Status>(result.Status));

        return Ok(result);
    }
}