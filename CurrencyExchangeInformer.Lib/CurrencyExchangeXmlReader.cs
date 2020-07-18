using System.Xml.Linq;

namespace CurrencyExchangeInformer.Lib
{
    public class CurrencyExchangeXmlReader
    {
        private XDocument _document;

        public CurrencyExchangeXmlReader(string data)
        {
            _document = XDocument.Parse(data);
        }
        
        
    }
}