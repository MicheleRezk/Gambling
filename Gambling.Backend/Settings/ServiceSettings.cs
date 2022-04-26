namespace Gambling.Backend.Settings;

public class ServiceSettings
{
    public string ServiceName { get; init; }
    /// <summary>
    /// The Starting Account for each player (Default 10000)
    /// </summary>
    public int StartingAccount { get; init; }
}