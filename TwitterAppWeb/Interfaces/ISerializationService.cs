using TwitterAppWeb.Models;

namespace TwitterAppWeb.Interfaces;

public interface ISerializationService
{
    List<TweetModel> Deserialize(ref string json);
}