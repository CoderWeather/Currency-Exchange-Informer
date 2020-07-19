using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyExchangeInformer.Lib.Xml
{
	public class HttpXmlLoader
	{
		private readonly string _url;
		private readonly HttpClient _httpClient;

		public HttpXmlLoader(string url)
		{
			_url = url;
			_httpClient = new HttpClient();
		}

		public async Task<string> LoadStringAsync() => await _httpClient.GetStringAsync(_url);

		public async Task<XDocument> LoadXmlDocumentAsync() => XDocument.Parse(await LoadStringAsync());
	}
}