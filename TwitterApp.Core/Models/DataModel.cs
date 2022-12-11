using System.Text.Json.Serialization;

namespace TwitterApp.Core.Models;

public class DataModel
{
    [JsonPropertyName("data")]
    public TweetModel Data { get; set; }
}