using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasOne(u => u.Person)
                .WithOne()
                .HasForeignKey<Customer>(p => p.PersonId);
            modelBuilder
                .Entity<Customer>()
                .HasMany(x => x.Receipts)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerId);
            modelBuilder
                .Entity<Receipt>()
                .HasMany(x => x.ReceiptDetails)
                .WithOne(x => x.Receipt)
                .HasForeignKey(x => x.ReceiptId);
            modelBuilder
                .Entity<ReceiptDetail>()
                .HasOne(x => x.Product)
                .WithMany(x => x.ReceiptDetails)
                .HasForeignKey(x => x.ProductId);
            modelBuilder
                .Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductCategoryId);
        }
    }
}
