using System;
using System.Collections.Generic;
using System.Text;

namespace FavIconlib.AbstractClasses
{
    internal abstract class Web
    {
        private readonly HttpClient _client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = true
        })
        {
            Timeout = TimeSpan.FromSeconds(3)
        };

        public Web()
            => UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:145.0) Gecko/20100101 Firefox/145.0";

        public string UserAgent
        {
            get => _client.DefaultRequestHeaders.UserAgent.ToString();
            set {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                _client.DefaultRequestHeaders.Remove("User-Agent");
                _client.DefaultRequestHeaders.Add("User-Agent", value);
            }
        }

        public TimeSpan Timeout { get => _client.Timeout; set => _client.Timeout = value; }

        public async Task<HttpResponseMessage> GetAsync(string url)
            => await _client.GetAsync(url);

    }
}
