using System;
using System.Threading.Tasks;
using CurrencyExchangeInformer.Lib.DataLoaders;

namespace CurrencyExchangeInformer.ConsoleApp
{
	internal static class Program
	{
		private static async Task Main(string[] args)
		{
			// await using var dataAccessor = new CurrenciesDataAccessor(new CentroBankExchangeRatesLoader());
			// Console.WriteLine("02.01.2020");
			// foreach (var cc in 
			// 	await dataAccessor.GetExchangeRatesForDate(new DateTime(2020, 1, 2).Date))
			// {
			// 	Console.WriteLine($"{cc.Item.OriginalName}: {cc.Value}");
			// }
			// await dataAccessor.UpdateDbDataFromSource();
			// var rate = await dataAccessor.GetExchangeRateByNameForDate("рубль", DateTime.Today);
			// Console.WriteLine(rate);

			for (var iter = DateTime.Today; iter.DayOfYear > 1; iter = iter.AddDays(-1d))
			{
				await using var dataAccessor = new CurrenciesDataAccessor(new CentroBankExchangeRatesLoader());
				await dataAccessor.UpdateCurrencyRates(iter);
				Console.WriteLine($"{iter:dd.MM.yyyy} parsed");
			}

			Console.WriteLine("Done");
		}
	}
}