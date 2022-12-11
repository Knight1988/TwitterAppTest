using FluentAssertions;
using TwitterAppWeb.Services;

namespace TwitterApp.Tests.Services;

public class SerializationServiceTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void DeserializeJson_ValidData_Success()
    {
        var sample = "{\"data\":{\"edit_history_tweet_ids\":[\"1599988115204345856\"],\"id\":\"1599988115204345856\",\"text\":\"RT @jobjab_ap: หนังน้องดรีมทำให้คนอยากไปคอนน้องดรีมเพิ่มขึ้น 99.9999999%  สั่นรอละ 🥲\"}}";
        var service = new SerializationService();
        var tweets = service.Deserialize(ref sample);

        tweets.Count.Should().Be(1);
        tweets[0].Id.Should().Be("1599988115204345856");
        sample.Should().BeEmpty("Should remove success parsed json");
    }

    [Test]
    public void DeserializeJson_HasInvalidData_SkipInvalidData()
    {
        var sample = "NotvalidJson\r\n{\"data\":{\"edit_history_tweet_ids\":[\"1599988115204345856\"],\"id\":\"1599988115204345856\",\"text\":\"RT @jobjab_ap: หนังน้องดรีมทำให้คนอยากไปคอนน้องดรีมเพิ่มขึ้น 99.9999999%  สั่นรอละ 🥲\"}}";
        var service = new SerializationService();
        var tweets = service.Deserialize(ref sample);

        tweets.Count.Should().Be(1);
        tweets[0].Id.Should().Be("1599988115204345856");
        sample.Should().BeEmpty("Should remove success parsed json");
    }

    [Test]
    public void DeserializeJson_HasIncompleteJson_SkipAndKeepIncompleteJson()
    {
        var sample = "{\"data\":{\"edit_history_tweet_ids\":[\"1599988115204345856\"],\"id\":\"1599988115204345856\",\"text\":\"RT @jobjab_ap: หนังน้องดรีมทำให้คนอยากไปคอนน้องดรีมเพิ่มขึ้น 99.9999999%  สั่นรอละ 🥲\"}}\r\n{\"data\":{\"edit";
        var service = new SerializationService();
        var tweets = service.Deserialize(ref sample);

        tweets.Count.Should().Be(1);
        tweets[0].Id.Should().Be("1599988115204345856");
        sample.Should().Be("{\"data\":{\"edit");
    }
}