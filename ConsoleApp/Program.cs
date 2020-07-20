using System;
using System.Threading.Tasks;
using CurrencyExchangeInformer.Lib.DataLoaders;

namespace CurrencyExchangeInformer.ConsoleApp
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            using var dataAccessor = new CurrenciesDataAccessor(new CentroBankExchangeRatesLoader());

            await dataAccessor.UpdateDbDataFromSource();

            await dataAccessor.GetExchangeRateByNameForDate("рубль", DateTime.Now);
            
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}