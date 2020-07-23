using System;
using CurrencyExchangeInformer.Lib.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchangeInformer.Lib.Configuration
{
	public class CurrencyExchangeConfiguration : IAppConfiguration
	{
		public CurrencyExchangeConfiguration()
		{
			var devEnvVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
			var isDevelopment = string.IsNullOrEmpty(devEnvVariable) || devEnvVariable.ToLower() == "development";

			var builder = new ConfigurationBuilder()
			   .AddJsonFile("appsettings.json", false, true);

			if (isDevelopment)
				builder.AddUserSecrets<ExchangeCurrencyDbSecrets>();

			var configurationRoot = builder.Build();

			var services = new ServiceCollection()
			   .Configure<ExchangeCurrencyDbSecrets>(configurationRoot.GetSection(nameof(ExchangeCurrencyDbSecrets)))
			   .AddOptions()
			   .AddLogging()
			   .AddSingleton<IAppSecrets, AppSecrets>();

			ServiceProvider = services.BuildServiceProvider();
		}

		#region Public Properties

		public IServiceProvider ServiceProvider { get; }

		#endregion

		#region Singletone

		private static IAppConfiguration? _instance;
		public static IAppConfiguration Instance => _instance ??= new CurrencyExchangeConfiguration();

		#endregion
	}
}