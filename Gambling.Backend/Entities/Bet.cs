using Gambling.Backend.Common;

namespace Gambling.Backend.Entities;

public class Bet
{
    public int Points { get; set; }
    public int Number { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public Status Status { get; set; }
}