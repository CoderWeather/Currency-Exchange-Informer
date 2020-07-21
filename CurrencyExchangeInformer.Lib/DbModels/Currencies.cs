using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace CurrencyExchangeInformer.Lib.DbModels
{
    public partial class Currencies
    {
        public Currencies()
        {
            CurrencyRates = new HashSet<CurrencyRates>();
        }

        [Required]
        [Column("original_name")]
        [StringLength(64)]
        public string OriginalName { get; set; }
        [Required]
        [Column("engName")]
        [StringLength(64)]
        public string EngName { get; set; }
        [Column("nominal")]
        public int Nominal { get; set; }
        [Required]
        [Column("parent_code")]
        [StringLength(7)]
        public string ParentCode { get; set; }
        [Column("iso_num_code")]
        public int? IsoNumCode { get; set; }
        [Column("iso_char_code")]
        [StringLength(3)]
        public string IsoCharCode { get; set; }
        [Key]
        [Column("item_id")]
        [StringLength(7)]
        public string ItemId { get; set; }

        [InverseProperty("Item")]
        public virtual ICollection<CurrencyRates> CurrencyRates { get; set; }
    }
}
