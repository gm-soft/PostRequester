using System.Threading.Tasks;
using PostProcessor.Entitites;

namespace PostProcessor.Interfaces
{
    public interface IRequestSender
    {
        Task<Response> SendGetRequestAsync(Request request);

        Task<Response> SendPostRequestAsync(Request request);
    }
}