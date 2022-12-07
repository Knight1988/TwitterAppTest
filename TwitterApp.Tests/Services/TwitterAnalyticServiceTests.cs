using FluentAssertions;
using Moq;
using Moq.AutoMock;
using TwitterApp.Interfaces;
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
    public async Task GetAverageTweetsPerMinute_1000Per5Min_Return200()
    {
        var mocker = new AutoMocker();
        var twitterFake = new Mock<ITwitterAnalyticRepository>();
        twitterFake.Setup(x => x.GetTweetCountFromMinuteAsync(It.IsAny<DateTime>())).ReturnsAsync(1000);
        mocker.Use(twitterFake);

        var service = mocker.CreateInstance<TwitterAnalyticService>();

        // Get avg count
        var tweetCount = await service.GetAverageTweetsPerMinuteAsync();
        
        // Assert result
        tweetCount.Should().Be(200);
    }

    [Test]
    public async Task GetAverageTweetsPerMinute_0Tweet_Return0()
    {
        var mocker = new AutoMocker();
        var twitterFake = new Mock<ITwitterAnalyticRepository>();
        twitterFake.Setup(x => x.GetTweetCountFromMinuteAsync(It.IsAny<DateTime>())).ReturnsAsync(0);
        mocker.Use(twitterFake);

        var service = mocker.CreateInstance<TwitterAnalyticService>();

        // Get avg count
        var tweetCount = await service.GetAverageTweetsPerMinuteAsync();
        
        // Assert result
        tweetCount.Should().Be(0);
    }
}