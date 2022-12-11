using TwitterApp.Core.Models;

namespace TwitterApp.Core.Interfaces;

public interface ISerializationService
{
    List<TweetModel> Deserialize(ref string json);
}