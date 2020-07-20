using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExchangeInformer.Lib.DbModels;
using Microsoft.EntityFrameworkCore;

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

		public async Task UpdateDbDataFromSource() => await UpdateDbDataFromSource(DateTime.Now.Date);

		public async Task UpdateDbDataFromSource(DateTime date)
		{
			var currencies = await Loader.GetCurrenciesAsync();
			var currenciesConversions = await Loader.GetCurrencyConversionsAsync();

			await UpdateCurrencies(currencies);
			await UpdateCurrencyConversions(currenciesConversions);
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

		private async Task UpdateCurrencyConversions(IEnumerable<CurrencyConversions> currencyConversions) =>
			await UpdateCurrencyConversions(currencyConversions, DateTime.Now.Date);

		private async Task UpdateCurrencyConversions(IEnumerable<CurrencyConversions> currencyConversions, DateTime date)
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

		public async Task<decimal?> GetExchangeRateByNameForDate(string valuteName, DateTime date)
		{
			if (await DbContext.CurrencyConversions.AnyAsync(
				cc => cc.Date.Equals(date.Date) &&
				(cc.Item.OriginalName.Contains(valuteName) || cc.Item.EngName.Contains(valuteName))
				) is false)
			{
				var currenciesConversions = await Loader.GetCurrencyConversionsForDateAsync(date.Date);
			}

			return (await DbContext.CurrencyConversions.FirstOrDefaultAsync(cc =>
			cc.Date.Date.Equals(date.Date) &&
			(cc.Item.OriginalName.Contains(valuteName) || cc.Item.EngName.Contains(valuteName))))?.Value;
		}

		public void Dispose()
		{
			DbContext.Dispose();
		}
	}
}