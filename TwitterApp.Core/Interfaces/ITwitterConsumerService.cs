namespace TwitterApp.Core.Interfaces;

public interface ITwitterConsumerService
{
    Task<Stream> GetSampleStreamAsync();
}