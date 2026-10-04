using System.Net.Http;
using System.Text.RegularExpressions;

namespace PlainApp.Services
{
    public class ExternalSourceUtils
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        public static async Task<string> GetURLTitleAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            try
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) ||
                   (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                {
                    return string.Empty;
                }

                // Set a user agent to avoid being blocked by some websites, get source code, extract title, and return it
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                string htmlContent = await _httpClient.GetStringAsync(uriResult);
                Match match = Regex.Match(htmlContent, @"<title[^>]*>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (match.Success)
                {
                    string title = System.Net.WebUtility.HtmlDecode(match.Groups[1].Value);
                    return title.Trim();
                }

                // Fallback: return the host if title is not found
                return uriResult.Host;
            }

            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
