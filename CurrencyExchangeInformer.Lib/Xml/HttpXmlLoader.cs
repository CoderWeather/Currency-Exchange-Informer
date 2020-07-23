using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyExchangeInformer.Lib.Xml
{
	public class HttpXmlLoader
	{
		private readonly Encoding _encoding;
		private readonly HttpClient _httpClient;
		private readonly string _url;

		public HttpXmlLoader(string url)
		{
			_encoding = CodePagesEncodingProvider.Instance.GetEncoding(1251) ?? Encoding.Default;
			_url = url;
			_httpClient = new HttpClient();
		}

		public async Task<string> LoadStringAsync()
		{
			var response = await _httpClient.GetAsync(_url);
			var buffer = await response.Content.ReadAsByteArrayAsync();
			var bytes = buffer.ToArray();
			return _encoding.GetString(bytes, 0, bytes.Length);
		}

		public async Task<XDocument> LoadXmlDocumentAsync()
		{
			return XDocument.Parse(await LoadStringAsync());
		}
	}
}