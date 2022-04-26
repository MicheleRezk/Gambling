using Gambling.Backend.Entities;

namespace Gambling.Backend.Common
{
    public interface IPlayerServices
    {
        Task<Player> GetPlayerAsync(Guid playerId);
        Task CreatePlayerAsync(Player player);
        Task DeletePlayerAsync(Guid playerId);
    }
}
