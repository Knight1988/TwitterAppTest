using System.Text.Json.Serialization;

namespace TwitterAppWeb.Models;

public class DataModel
{
    [JsonPropertyName("data")]
    public TweetModel Data { get; set; }
}