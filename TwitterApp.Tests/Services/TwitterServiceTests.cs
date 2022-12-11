using FluentAssertions;
using Moq;
using Moq.AutoMock;
using TwitterAppWeb.Interfaces;
using TwitterAppWeb.Models;
using TwitterAppWeb.Services;

namespace TwitterApp.Tests.Services;

[TestFixture]
public class TwitterServiceTests
{
    [Test]
    public async Task SaveData_ShouldAddCreatedTime()
    {
        var mocker = new AutoMocker();
        var twitterFake = new Mock<ITwitterRepository>();
        twitterFake.Setup(x => x.UpsertAsync(It.IsAny<IEnumerable<TweetModel>>())).Callback<IEnumerable<TweetModel>>(
            tweetModels =>
            {
                // Assert created time is today
                tweetModels.ToList()[0].CreatedTime.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-MM-dd"));
            });
        mocker.Use(twitterFake);
        var sampleData = new List<TweetModel>() { new() { Id = "1", Text = "Test" } };

        var service = mocker.CreateInstance<TwitterService>();

        await service.SaveDataAsync(sampleData);
    }
}