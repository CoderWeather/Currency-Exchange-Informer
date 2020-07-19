using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CurrencyExchangeInformer.Lib.Xml
{
    public class CurrencyConversionsXmlParser
    {
        private readonly XDocument _document;

        public CurrencyConversionsXmlParser(string data)
        {
            _document = XDocument.Parse(data);
        }

        public CurrencyConversionsXmlParser(XDocument document)
        {
            _document = document;
        }

        public IEnumerable<XElement> GetCurrenciesConversions()
        {
            var rootEl = _document.Element("ValCurs");
            return rootEl is null
                ? Enumerable.Empty<XElement>()
                : rootEl.Elements("Valute");
        }
    }
}