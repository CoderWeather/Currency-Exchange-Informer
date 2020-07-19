using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CurrencyExchangeInformer.Lib.Xml
{
	public class CurrenciesXmlParser
	{
		private readonly XDocument _document;

		public CurrenciesXmlParser(string data)
		{
			_document = XDocument.Parse(data);
		}

		public CurrenciesXmlParser(XDocument document)
		{
			_document = document;
		}

		public IEnumerable<XElement> GetCurrencies()
		{
			var rootEl = _document.Element("Valuta");
			return rootEl is null
				? Enumerable.Empty<XElement>()
				: rootEl.Elements("Item");
		}
	}
}