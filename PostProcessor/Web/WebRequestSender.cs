using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PostProcessor.Entitites;
using PostProcessor.Interfaces;
using System.Net.Http;

namespace PostProcessor.Web
{
    public class WebRequestSender : IRequestSender, IDisposable
    {
        private readonly HttpClient _client;
        private readonly CookieContainer _cookieContainer;
        private readonly string _baseAddress;

        public WebRequestSender(string baseAddress)
        {
            _baseAddress = baseAddress;
            _cookieContainer = new CookieContainer();
            AddCookies();
            _client = new HttpClient(new HttpClientHandler
            {
                CookieContainer = _cookieContainer
            });
        }

        public async Task<Response> SendGetRequestAsync(Request request)
        {
            string parameters = string.Empty;
            string lastKey = request.Parameters.Keys.LastOrDefault();
            foreach (KeyValuePair<string, string> pair in request.Parameters)
            {
                parameters += $"{pair.Key}={pair.Value}";
                if (lastKey != pair.Key)
                {
                    parameters += "&";
                }
            }
            HttpResponseMessage getResponse = await _client.GetAsync(new Uri($"{request.Url}?{parameters}"));
            return new Response
            {
                ResponseContent = await getResponse.Content.ReadAsStringAsync(),
                StatusCode = (int)getResponse.StatusCode
            };
            
        }

        public async Task<Response> SendPostRequestAsync(Request request)
        {
            var content = new FormUrlEncodedContent(request.Parameters);
            HttpResponseMessage postResponse = await _client.PostAsync(request.Url, content);

            return new Response
            {
                ResponseContent = await postResponse.Content.ReadAsStringAsync(),
                StatusCode = (int)postResponse.StatusCode
            };
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        private bool AddCookies()
        {
            var cookie = new Cookie("KaspiHelp_ASP.NET_SessionId", "p1rxv5fqqreamtjp5dh3raf1", "/", _baseAddress);
            _cookieContainer.Add(cookie);
            return true;
        }
    }
}