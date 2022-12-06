using System.Text.Json;
using TwitterApp.Models;

namespace TwitterApp.Services;

public class SerializationService : ISerializationService
{
    public List<TweetModel> Deserialize(ref string json)
    {
        var jsonArray = json.Split("\r\n");
        var tweets = new List<TweetModel>();
        var newJson = string.Empty;
        for (int i = 0; i < jsonArray.Length; i++)
        {
            try
            {
                var data = JsonSerializer.Deserialize<DataModel>(jsonArray[i]);
                tweets.Add(data.Data);
            }
            catch (Exception)
            {
                // store unfinished 
                if (i == jsonArray.Length - 1) newJson += jsonArray[i];
            }
        }

        json = newJson;

        return tweets;
    }
}