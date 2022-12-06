using System.Text.Json.Serialization;

namespace TwitterApp.Models;

public abstract class TweetModel
{
    [JsonPropertyName("edit_history_tweet_ids")]
    public List<string> EditHistoryTweetIds { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}