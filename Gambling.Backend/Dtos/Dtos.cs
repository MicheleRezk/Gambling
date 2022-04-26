using System.ComponentModel.DataAnnotations;

namespace Gambling.Backend.Dtos
{
    public record PlayerDto(string Name, int Account);
    public record CreatePlayerDto([Required] string Name);
    public record CreateBetDto([Required] Guid PlayerId, [Required] int Points,[Required] int Number);
    public record BetResultDto(int Account, string Status, string Points);
}