using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyExchangeInformer.Lib.DbModels;
using CurrencyExchangeInformer.Lib.Xml;

namespace CurrencyExchangeInformer.Lib.DataLoaders
{
	public class CentroBankExchangeRatesLoader : IExchangeRatesLoader
	{
		public CentroBankExchangeRatesLoader()
		{
			CurrenciesListUrl = @"http://www.cbr.ru/scripts/XML_valFull.asp";
			ExchangesRateForDate = @"http://www.cbr.ru/scripts/XML_daily.asp?date_req=";
		}

		private string CurrenciesListUrl { get; }
		private string ExchangesRateForDate { get; }

		public async Task<IEnumerable<Currencies>> GetCurrenciesAsync()
		{
			var httpClient = new HttpXmlLoader(CurrenciesListUrl);
			var parser = new CurrenciesXmlParser(await httpClient.LoadXmlDocumentAsync());
			return CurrenciesDbModelsFabric.CreateCurrencies(parser.GetCurrencies());
		}

		public async Task<IEnumerable<CurrencyRates>> GetCurrencyRatesForDateAsync(DateTime date)
		{
			var httpClient = new HttpXmlLoader(GetRateUrlForDate(date));
			var parser = new CurrencyRatesXmlParser(await httpClient.LoadStringAsync());
			return CurrenciesDbModelsFabric.CreateCurrenciesRates(parser.GetCurrenciesRates(), date);
		}

		private string GetRateUrlForDate(DateTime date)
		{
			return ExchangesRateForDate + date.ToString("dd.MM.yyyy");
		}
	}
}