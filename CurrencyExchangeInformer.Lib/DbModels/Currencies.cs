using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace CurrencyExchangeInformer.Lib.DbModels
{
	public partial class Currencies
	{
		[Required]
		[Column("original_name")]
		[StringLength(32)]
		public string OriginalName { get; set; }

		[Required]
		[Column("engName")]
		[StringLength(32)]
		public string EngName { get; set; }

		[Column("nominal")] public int Nominal { get; set; }

		[Required]
		[Column("parent_code")]
		[StringLength(7)]
		public string ParentCode { get; set; }

		[Column("iso_num_code")] public int IsoNumCode { get; set; }

		[Required]
		[Column("iso_char_code")]
		[StringLength(3)]
		public string IsoCharCode { get; set; }

		[Key]
		[Column("item_id")]
		[StringLength(7)]
		public string ItemId { get; set; }

		[InverseProperty("Item")] public virtual CurrencyConversions CurrencyConversions { get; set; }
	}
}