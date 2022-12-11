﻿using System.Text.Json;
using TwitterAppWeb.Interfaces;
using TwitterAppWeb.Models;

namespace TwitterAppWeb.Services;

public class SerializationService : ISerializationService
{
    /// <summary>
    /// Deserialize twitter json data to object
    /// </summary>
    /// <param name="json">twitter json data</param>
    /// <returns>List of TweetModel</returns>
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
                if (data != null) tweets.Add(data.Data);
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