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
    private readonly IRepository<Player> _playersRepo;
    private readonly ServiceSettings _serviceSettings;

    public PlayersController(
        IMapper mapper,
        IRepository<Player> playersRepo,
        IOptions<ServiceSettings> serviceSettings)
    {
        this._mapper = mapper;
        this._playersRepo = playersRepo;
        this._serviceSettings = serviceSettings.Value;
    }

    // GET /players
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
    {
        Console.WriteLine("--> Getting Players....");
        var players = await _playersRepo.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<PlayerDto>>(players));
    }

    // GET /players/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetByIdAsync(Guid id)
    {
        Console.WriteLine("--> Getting Player By Id....");
        var player = await _playersRepo.GetAsync(id);

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

        await _playersRepo.CreateAsync(player);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = player.Id }, player);
    }
}