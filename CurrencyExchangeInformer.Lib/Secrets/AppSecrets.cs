using System;
using Microsoft.Extensions.Options;

namespace CurrencyExchangeInformer.Lib.Secrets
{
	public class AppSecrets : IAppSecrets
	{
		private readonly ExchangeCurrencyDbSecrets _secrets;

		public AppSecrets(IOptions<ExchangeCurrencyDbSecrets> options)
		{
			_secrets = options.Value ?? throw new ArgumentNullException(nameof(options));
		}

		public string ConnectionString => _secrets.ConnectionString;
	}
}