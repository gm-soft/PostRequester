using System;
using System.Collections.Generic;
using System.Threading;
using PostProcessor.Entitites;
using PostProcessor.Interfaces;
using PostProcessor.Web;

namespace PostProcessor
{
    public class WebController : IController
    {
        private readonly IRequestSender _requestSender;

        private readonly IEventListener _listenter;

        private Dictionary<string, string> _parameters;

        private Thread _requestThread;

        public WebController(string baseAddress, Dictionary<string, string> parameters, IEventListener listenter)
        {
            _listenter = listenter;
            _parameters = parameters ?? throw new ArgumentNullException();
            _requestSender = new WebRequestSender(baseAddress);
        }

        public void StartCycle(string url, int count)
        {
            if (_requestThread != null)
            {
                return;
            }
            _requestThread = new Thread(() => StartCycleInner(url, count, _parameters));
            _requestThread.Start();
        }

        private async void StartCycleInner(string url, int count, Dictionary<string, string> parameters)
        {
            var delay = TimeSpan.FromMilliseconds(10);
            var request = new Request
            {
                Parameters = parameters,
                Url = url
            };
            for (int i = 0; i < count; i++)
            {
                Response response = await _requestSender.SendPostRequestAsync(request);
                _listenter.OnEvent($"{i}) {response.StatusCode} - {response.ResponseContent}");
                Thread.Sleep(delay);
            }
        }

        public void StopCycle()
        {
            _requestThread?.Abort();
            _requestThread = null;
        }
    }
}