using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExchangeInformer.Lib.DbModels;

namespace CurrencyExchangeInformer.Lib.DataLoaders
{
	public class CurrenciesDataAccessor : IDisposable
	{
		private ExchangeCurrencyDbContext DbContext { get; }

		private IExchangeRatesLoader Loader { get; }

		public CurrenciesDataAccessor(IExchangeRatesLoader loader)
		{
			DbContext = new ExchangeCurrencyDbContext();
			Loader = loader;
		}

		public async Task UpdateDbDataFromSource()
		{
			var currencies = await Loader.GetCurrenciesAsync();
			var currenciesConversions = await Loader.GetCurrencyConversionsAsync();

			await UpdateCurrencies(currencies);
			await DbContext.CurrencyConversions.AddRangeAsync(currenciesConversions);
			await DbContext.SaveChangesAsync();
		}

		private async Task UpdateCurrencies(IEnumerable<Currencies> currencies)
		{
			foreach (var cur in currencies)
			{
				var curForAdd = DbContext.Currencies
				   .FirstOrDefault(c =>
						c.ItemId.Equals(cur.ItemId) &&
						c.IsoNumCode.Equals(cur.IsoNumCode) &&
						c.IsoCharCode.Equals(cur.IsoCharCode));
				if (curForAdd is null)
				{
					await DbContext.AddAsync(cur);
				}
			}

			await DbContext.SaveChangesAsync();
		}

		private async Task UpdateCurrencyConversions(IEnumerable<CurrencyConversions> currencyConversions)
		{
			foreach (var curConv in currencyConversions)
			{
				var curConvForAdd = DbContext.CurrencyConversions
				   .FirstOrDefault(cc =>
						cc.ItemId.Equals(curConv.ItemId));
				if (curConvForAdd is null)
				{
					await DbContext.AddAsync(curConv);
				}
				else
				{
					curConvForAdd.Value = curConv.Value;
				}
			}

			await DbContext.SaveChangesAsync();
		}

		public decimal GetExchangeRateForDate(string isoCharCode, DateTime date) =>
			DbContext.CurrencyConversions.First(cc =>
				cc.Date.Date.Equals(date.Date) && cc.Item.IsoCharCode.Equals(isoCharCode)).Value;

		public decimal GetExchangeRateForDate(int isoNumCode, DateTime date) =>
			DbContext.CurrencyConversions.First(cc =>
				cc.Date.Date.Equals(date.Date) && cc.Item.IsoNumCode.Equals(isoNumCode)).Value;

		public void Dispose()
		{
			DbContext.Dispose();
		}
	}
}