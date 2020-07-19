using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyExchangeInformer.Lib.DbModels;

namespace CurrencyExchangeInformer.Lib.DataLoaders
{
	public interface IExchangeRatesLoader
	{
		public Task<IEnumerable<Currencies>> GetCurrenciesAsync();

		public async Task<IEnumerable<CurrencyConversions>> GetCurrencyConversionsAsync() =>
			await GetCurrencyConversionsForDateAsync(DateTime.Now);

		public Task<IEnumerable<CurrencyConversions>> GetCurrencyConversionsForDateAsync(DateTime date);
	}
}