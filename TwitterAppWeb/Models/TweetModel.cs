using System.Text.Json.Serialization;

namespace TwitterAppWeb.Models;

public class TweetModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    public DateTime CreatedTime { get; set; }
}