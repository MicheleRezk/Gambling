using Gambling.Backend.Common;
using Gambling.Backend.Entities;

namespace Gambling.Backend.Services
{
    public class PlayerServices : IPlayerServices
    {
        private readonly IRepository<Player> _playersRepo;

        public PlayerServices(IRepository<Player> playersRepo)
        {
            this._playersRepo = playersRepo;
        }
        public async Task<Player> GetPlayerAsync(Guid playerId)
        {
            return await _playersRepo.GetAsync(playerId);
        }
        public async Task CreatePlayerAsync(Player player)
        {
            await _playersRepo.CreateAsync(player);
        }
        public async Task DeletePlayerAsync(Guid playerId)
        {
            await _playersRepo.RemoveAsync(playerId);
        }
    }
}
