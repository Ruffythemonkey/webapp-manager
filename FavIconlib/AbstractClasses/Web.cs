using System.Net;

namespace FavIconlib.AbstractClasses
{
    internal abstract class Web
    {
        private readonly HttpClient _client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            AutomaticDecompression = DecompressionMethods.All
        })
        {
            Timeout = TimeSpan.FromSeconds(3)
        };

        public Web()
            => SetClient();

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

        private void SetClient()
        {
            _client.DefaultRequestVersion = HttpVersion.Version30;
            _client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            _client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:145.0) Gecko/20100101 Firefox/145.0";
        }

        private CancellationTokenSource _tokenSource = new();

        public TimeSpan Timeout { get => _client.Timeout; set => _client.Timeout = value; }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            await RegenerateTokenSource();
            return await _client.GetAsync(url, _tokenSource.Token);
        }

        private async Task RegenerateTokenSource()
        {
            await _tokenSource.CancelAsync();
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
        }
    }
}
