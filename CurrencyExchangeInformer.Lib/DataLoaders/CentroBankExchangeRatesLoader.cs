using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyExchangeInformer.Lib.DbModels;
using CurrencyExchangeInformer.Lib.Xml;

namespace CurrencyExchangeInformer.Lib.DataLoaders
{
	public class CentroBankExchangeRatesLoader : IExchangeRatesLoader
	{
		private string CurrenciesListUrl { get; }
		private string ExchangesRateForDate { get; }

		public CentroBankExchangeRatesLoader()
		{
			CurrenciesListUrl = @"http://www.cbr.ru/scripts/XML_valFull.asp";
			ExchangesRateForDate = @"http://www.cbr.ru/scripts/XML_daily.asp?date_req=";
		}

		private string GetRateUrlForDate(DateTime date) => ExchangesRateForDate + date.ToString("dd.MM.yyyy");

		public async Task<IEnumerable<Currencies>> GetCurrenciesAsync()
		{
			var httpClient = new HttpXmlLoader(CurrenciesListUrl);
			var parser = new CurrenciesXmlParser(await httpClient.LoadXmlDocumentAsync());
			IEnumerable<Currencies> currencies = CurrenciesDbModelsFabric.CreateCurrencies(parser.GetCurrencies());

			return currencies;
		}

		public async Task<IEnumerable<CurrencyConversions>> GetCurrencyConversionsForDateAsync(DateTime date)
		{
			var httpClient = new HttpXmlLoader(GetRateUrlForDate(date));
			var parser = new CurrencyConversionsXmlParser(await httpClient.LoadStringAsync());
			IEnumerable<CurrencyConversions> currencyConversions =
				CurrenciesDbModelsFabric.CreateCurrenciesConversions(parser.GetCurrenciesConversions(), date);

			return currencyConversions;
		}
	}
}