﻿using System;
using System.Text.Json.Serialization;

namespace TwitterApp.Models;

public class TokenModel
{
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}