using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using TwitterApp.Interfaces;
using TwitterApp.Models;
using TwitterApp.Services;

namespace TwitterApp.Tests.Services;

[TestFixture]
public class TwitterAnalyticServiceTests
{
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(21)]
    public async Task GetTotalTweetCount_ReturnTweetCount(int count)
    {
        var mocker = new AutoMocker();
        var twitterFake = new Mock<ITwitterAnalyticRepository>();
        twitterFake.Setup(x => x.GetTotalTweetCountAsync()).ReturnsAsync(count);
        mocker.Use(twitterFake);

        var service = mocker.CreateInstance<TwitterAnalyticService>();

        var tweetCount = await service.GetTotalTweetCountAsync();
        tweetCount.Should().Be(count);
    }

    [Test]
    public async Task GetAverageTweetsPerMinute_RecentFiveMin_ReturnAverageCount()
    {
        //Set the randomizer seed to generate repeatable data sets.
        Randomizer.Seed = new Random(2000);
        
        // generate fake tweets
        var testTweets = new Faker<TweetModel>()
            .RuleFor(p => p.CreatedTime, f => f.Date.Between(DateTime.Now.AddMinutes(-5), DateTime.Now));
        
        var mocker = new AutoMocker();
        var twitterFake = new Mock<ITwitterAnalyticRepository>();
        twitterFake.Setup(x => x.GetLatestTweetsAsync(It.IsAny<int>())).ReturnsAsync(testTweets.Generate(1000));
        mocker.Use(twitterFake);

        var service = mocker.CreateInstance<TwitterAnalyticService>();

        // Get avg count
        var tweetCount = await service.GetAverageTweetsPerMinuteAsync();
        
        // Assert result
        tweetCount.Should().Be(205);
    }

    [Test]
    public async Task GetAverageTweetsPerMinute_RecentTenSecond_ReturnAverageCount()
    {
        //Set the randomizer seed to generate repeatable data sets.
        Randomizer.Seed = new Random(8675309);
        
        // generate fake tweets
        var testTweets = new Faker<TweetModel>()
            .RuleFor(p => p.CreatedTime, f => DateTime.Now.AddSeconds(-10));
        
        var mocker = new AutoMocker();
        var twitterFake = new Mock<ITwitterAnalyticRepository>();
        twitterFake.Setup(x => x.GetLatestTweetsAsync(It.IsAny<int>())).ReturnsAsync(testTweets.Generate(1000));
        mocker.Use(twitterFake);

        var service = mocker.CreateInstance<TwitterAnalyticService>();

        // Get avg count
        var tweetCount = await service.GetAverageTweetsPerMinuteAsync();
        
        // Assert result
        tweetCount.Should().Be(1000);
    }
}