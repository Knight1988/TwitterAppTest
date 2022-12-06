using System.Text.Json.Serialization;

namespace TwitterApp.Models;

public class TweetModel
{
    [JsonPropertyName("edit_history_tweet_ids")]
    public List<string> EditHistoryTweetIds { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class DataModel
{
    [JsonPropertyName("data")]
    public TweetModel Data { get; set; }
}