using System.Text.Json.Serialization;

namespace TwitterApp.Models;

public class DataModel
{
    [JsonPropertyName("data")]
    public TweetModel Data { get; set; }
}