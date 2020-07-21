using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExchangeInformer.Lib.DbModels;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeInformer.Lib.DataLoaders
{
	public class CurrenciesDataAccessor : IDisposable, IAsyncDisposable
	{
		private ExchangeCurrencyDbContext DbContext { get; }

		private IExchangeRatesLoader Loader { get; }

		public CurrenciesDataAccessor(IExchangeRatesLoader loader)
		{
			DbContext = new ExchangeCurrencyDbContext();
			Loader = loader;
		}

		public async Task UpdateDbDataFromSource() => await UpdateDbDataFromSource(DateTime.Today.Date);

		public async Task UpdateDbDataFromSource(DateTime date)
		{
			await UpdateCurrencies();
			await UpdateCurrencyRates(date);
		}

		public async Task UpdateCurrencies() => await UpdateCurrencies(await Loader.GetCurrenciesAsync());

		public async Task UpdateCurrencies(IEnumerable<Currencies> currencies)
		{
			foreach (var currency in currencies)
			{
				var currencyForAdd = DbContext.Currencies
				   .FirstOrDefault(c => c.ItemId.Equals(currency.ItemId));
				if (currencyForAdd is null)
				{
					await DbContext.AddAsync(currency);
				}
				else
				{
					currencyForAdd.Nominal = currency.Nominal;
					currencyForAdd.OriginalName ??= currency.OriginalName;
					currencyForAdd.EngName ??= currency.EngName;
					currencyForAdd.ParentCode ??= currency.ParentCode;
					currencyForAdd.IsoCharCode ??= currency.IsoCharCode;
					currencyForAdd.IsoNumCode ??= currency.IsoNumCode;
				}
			}

			await DbContext.SaveChangesAsync();
		}

		public async Task UpdateCurrencyRates() =>
			await UpdateCurrencyRates(DateTime.Today);

		public async Task UpdateCurrencyRates(DateTime date) =>
			await UpdateCurrencyRates(await Loader.GetCurrencyRatesForDateAsync(date), date);

		public async Task UpdateCurrencyRates(IEnumerable<CurrencyRates> currencyRates,
			DateTime date)
		{
			date = date.Date;
			foreach (var currencyRate in currencyRates)
			{
				var findCurrencyRate = DbContext.CurrencyRates
				   .FirstOrDefault(cc =>
						cc.ItemId.Equals(currencyRate.ItemId) && cc.Date.Equals(date));
				if (findCurrencyRate is null)
					await DbContext.AddAsync(currencyRate);
				else
				{
					findCurrencyRate.Value = currencyRate.Value;
				}
				await DbContext.SaveChangesAsync();
			}

			await DbContext.SaveChangesAsync();
		}

		public async Task<List<CurrencyRates>> GetExchangeRatesForDate(DateTime date)
		{
			date = date.Date;
			return await DbContext.CurrencyRates
			   .Where(cc => cc.Date.Equals(date)).ToListAsync();
		}

		public async Task<decimal?> GetExchangeRateByNameForDate(string valuteName, DateTime date)
		{
			date = date.Date;
			if (await DbContext.CurrencyRates
			   .AnyAsync(cc =>
					cc.Date.Equals(date) &&
					(cc.Item.OriginalName.Contains(valuteName) || cc.Item.EngName.Contains(valuteName))) is false)
			{
				await UpdateCurrencyRates(date);
			}

			return (await DbContext.CurrencyRates
			   .FirstOrDefaultAsync(cc =>
					cc.Date.Equals(date) &&
					(cc.Item.OriginalName.Contains(valuteName) || cc.Item.EngName.Contains(valuteName)))).Value;
		}

		public void Dispose() => DbContext.Dispose();

		public ValueTask DisposeAsync() => DbContext.DisposeAsync();
	}
}