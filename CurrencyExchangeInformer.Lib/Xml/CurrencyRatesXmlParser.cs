using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CurrencyExchangeInformer.Lib.Xml
{
	public class CurrencyRatesXmlParser
	{
		private readonly XDocument _document;

		public CurrencyRatesXmlParser(string data)
		{
			_document = XDocument.Parse(data);
		}

		public CurrencyRatesXmlParser(XDocument document)
		{
			_document = document;
		}

		public IEnumerable<XElement> GetCurrenciesRates()
		{
			var rootEl = _document.Element("ValCurs");
			return rootEl is null
				? Enumerable.Empty<XElement>()
				: rootEl.Elements("Valute");
		}
	}
}