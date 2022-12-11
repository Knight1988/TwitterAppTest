using System.Text;
using FluentAssertions;
using TwitterApp.Core.Services;

namespace TwitterApp.Tests.Services;

[TestFixture]
public class TwitterConsumerServiceTests
{
    [Test]
    public async Task GetSampleStream_ShouldReturnData()
    {
        var configuration = Helper.GetConfiguration();
        var service = new TwitterConsumerService(configuration);

        var stream = await service.GetSampleStreamAsync();
        
        // get json string
        var buffer = new byte[1024];
        var length = await stream.ReadAsync(buffer, 0, buffer.Length);
        var json = Encoding.UTF8.GetString(buffer, 0, length).Trim();

        json.Should().NotBeEmpty();
    }
}