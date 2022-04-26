using Gambling.Backend.Common;

namespace Gambling.Backend.Entities;

public class Player: IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public int Account { get; set; }
    public IEnumerable<Bet> Bets { get; set; }

    public void AddNewBet(Bet bet)
    {
        this.Bets = Bets.Append(bet).ToList();
    }
}