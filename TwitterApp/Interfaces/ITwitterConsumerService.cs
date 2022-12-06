using System.IO;
using System.Threading.Tasks;

namespace TwitterApp.Interfaces;

public interface ITwitterConsumerService
{
    Task<Stream> GetSampleStreamAsync();
}