using CurrencyExchangeInformer.Lib.Configuration;
using CurrencyExchangeInformer.Lib.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#nullable disable
namespace CurrencyExchangeInformer.Lib.DbModels
{
    public partial class ExchangeCurrencyDbContext : DbContext
    {
        public ExchangeCurrencyDbContext()
        {
        }

        public ExchangeCurrencyDbContext(DbContextOptions<ExchangeCurrencyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Currencies> Currencies { get; set; }
        public virtual DbSet<CurrencyRates> CurrencyRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                   .UseLazyLoadingProxies()
                   .EnableSensitiveDataLogging()
                   .UseSqlServer(
                        CurrencyExchangeConfiguration.Instance
                           .ServiceProvider.GetService<IAppSecrets>().ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currencies>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("Currencies_pk")
                    .IsClustered(false);

                entity.Property(e => e.ItemId).IsUnicode(false);

                entity.Property(e => e.EngName).IsUnicode(false);

                entity.Property(e => e.IsoCharCode).IsUnicode(false);

                entity.Property(e => e.ParentCode).IsUnicode(false);
            });

            modelBuilder.Entity<CurrencyRates>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.Date })
                    .HasName("CurrencyRates_pk")
                    .IsClustered(false);

                entity.Property(e => e.ItemId).IsUnicode(false);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.CurrencyRates)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CurrencyRates_Currencies_item_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
