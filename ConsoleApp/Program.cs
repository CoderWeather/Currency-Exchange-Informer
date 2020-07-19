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
            
            Console.ReadLine();
        }
    }
}