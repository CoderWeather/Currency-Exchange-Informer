using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CurrencyExchangeInformer.Lib
{
    public class CurrencyExchangeXmlParser
    {
        private readonly XDocument _document;

        public CurrencyExchangeXmlParser(string data)
        {
            _document = XDocument.Parse(data);
        }

        public IEnumerable<XElement> GetCurrenciesConversions()
        {
            return _document.Elements(XName.Get("Valute"));
        }
    }
}