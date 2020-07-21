using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace CurrencyExchangeInformer.Lib.DbModels
{
    public partial class CurrencyRates
    {
        [Key]
        [Column("item_id")]
        [StringLength(7)]
        public string ItemId { get; set; }
        [Column("nominal")]
        public int Nominal { get; set; }
        [Column("value", TypeName = "decimal(18, 0)")]
        public decimal Value { get; set; }
        [Key]
        [Column("date", TypeName = "date")]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(ItemId))]
        [InverseProperty(nameof(Currencies.CurrencyRates))]
        public virtual Currencies Item { get; set; }
    }
}
