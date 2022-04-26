using AutoMapper;
using Gambling.Backend.Common;
using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;
using Gambling.Backend.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gambling.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPlayerServices _playerServices;
    private readonly ServiceSettings _serviceSettings;

    public PlayersController(
        IMapper mapper,
        IPlayerServices playerServices,
        IOptions<ServiceSettings> serviceSettings)
    {
        this._mapper = mapper;
        this._playerServices = playerServices;
        this._serviceSettings = serviceSettings.Value;
    }

    // GET /players/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetByIdAsync(Guid id)
    {
        Console.WriteLine("--> Getting Player By Id....");
        var player = await _playerServices.GetPlayerAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PlayerDto>(player));
    }

    // POST /players
    [HttpPost]
    public async Task<ActionResult<PlayerDto>> PostAsync(CreatePlayerDto createPlayerDto)
    {
        Console.WriteLine("--> Creating a new player....");
        var player = new Player
        {
            Name = createPlayerDto.Name,
            Account = _serviceSettings.StartingAccount,
            Id = Guid.NewGuid(),
            CreatedDate = DateTimeOffset.Now,
            Bets = new List<Bet>()
        };

        await _playerServices.CreatePlayerAsync(player);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = player.Id }, _mapper.Map<PlayerDto>(player));
    }
}