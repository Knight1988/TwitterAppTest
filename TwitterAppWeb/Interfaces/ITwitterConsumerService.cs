namespace TwitterAppWeb.Interfaces;

public interface ITwitterConsumerService
{
    Task<Stream> GetSampleStreamAsync();
}