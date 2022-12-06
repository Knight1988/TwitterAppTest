using TwitterApp.Models;

namespace TwitterApp.Services;

public interface ISerializationService
{
    List<TweetModel> Deserialize(ref string json);
}