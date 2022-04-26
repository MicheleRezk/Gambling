namespace Gambling.Backend.Common
{
    public interface ISerializerService
    {
        T Deserialize<T> (string json);
    }
}
