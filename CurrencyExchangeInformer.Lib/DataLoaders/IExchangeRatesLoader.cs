using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyExchangeInformer.Lib.DbModels;

namespace CurrencyExchangeInformer.Lib.DataLoaders
{
	public interface IExchangeRatesLoader
	{
		public Task<IEnumerable<Currencies>> GetCurrenciesAsync();

		public async Task<IEnumerable<CurrencyRates>> GetCurrencyRatesAsync() =>
			await GetCurrencyRatesForDateAsync(DateTime.Today);

		public Task<IEnumerable<CurrencyRates>> GetCurrencyRatesForDateAsync(DateTime date);
	}
}