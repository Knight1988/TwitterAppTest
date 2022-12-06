using TwitterApp.Models;

namespace TwitterApp.Interfaces;

public interface ISerializationService
{
    List<TweetModel> Deserialize(ref string json);
}