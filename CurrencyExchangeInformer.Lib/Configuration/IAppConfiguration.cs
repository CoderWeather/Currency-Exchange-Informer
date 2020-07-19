using System;

namespace CurrencyExchangeInformer.Lib.Configuration
{
	public interface IAppConfiguration
	{
		public IServiceProvider ServiceProvider { get; }
	}
}