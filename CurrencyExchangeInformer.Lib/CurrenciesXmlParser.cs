using System.Collections.Generic;
using System.Xml.Linq;

namespace CurrencyExchangeInformer.Lib
{
    public class CurrenciesXmlParser
    {
        private readonly XDocument _document;

        public CurrenciesXmlParser(string data)
        {
            _document = XDocument.Parse(data);
        }

        public IEnumerable<XElement> GetCurrencies()
        {
            return _document.Elements(XName.Get("Item"));
        }
    }
}