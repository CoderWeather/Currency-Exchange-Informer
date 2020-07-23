using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CurrencyExchangeInformer.Lib.DbModels
{
	public static class CurrenciesDbModelsFabric
	{
		public static Currencies CreateCurrency(XElement el)
		{
			var obj = new Currencies();

			var itemId = el.Attribute("ID")?.Value;
			var originName = el.Element("Name")?.Value;
			var engName = el.Element("EngName")?.Value;
			var nominal = el.Element("Nominal")?.Value;
			var parentCode = el.Element("ParentCode")?.Value;
			var isoNumCode = el.Element("ISO_Num_Code")?.Value;
			var isoCharCode = el.Element("ISO_Char_Code")?.Value;

			if (itemId != null)
				obj.ItemId = itemId;

			if (originName != null)
				obj.OriginalName = originName;

			if (engName != null)
				obj.EngName = engName;

			if (int.TryParse(nominal, out var nominalParsed))
				obj.Nominal = nominalParsed;

			if (parentCode != null)
				obj.ParentCode = parentCode;

			if (int.TryParse(isoNumCode, out var numCodeParsed))
				obj.IsoNumCode = numCodeParsed;


			if (isoCharCode != null)
				obj.IsoCharCode = isoCharCode;


			return obj;
		}

		public static IEnumerable<Currencies> CreateCurrencies(IEnumerable<XElement> elements)
		{
			return elements.Select(CreateCurrency);
		}

		public static CurrencyRates CreateCurrenciesConversion(XElement el, DateTime date)
		{
			var obj = new CurrencyRates {Date = date};

			var itemId = el.Attribute("ID")?.Value;
			var nominal = el.Element("Nominal")?.Value;
			var value = el.Element("Value")?.Value;

			if (itemId != null)
				obj.ItemId = itemId;

			if (int.TryParse(nominal, out var nominalParsed))
				obj.Nominal = nominalParsed;

			if (decimal.TryParse(value, out var valueParsed))
				obj.Value = valueParsed;

			return obj;
		}

		public static IEnumerable<CurrencyRates> CreateCurrenciesRates(IEnumerable<XElement> elements,
			DateTime date)
		{
			return elements.Select(el => CreateCurrenciesConversion(el, date));
		}
	}
}