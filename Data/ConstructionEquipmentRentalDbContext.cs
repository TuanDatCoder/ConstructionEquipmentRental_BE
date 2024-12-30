using System;
using System.Collections.Generic;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public partial class ConstructionEquipmentRentalDbContext : DbContext
{
    public ConstructionEquipmentRentalDbContext()
    {
    }

    public ConstructionEquipmentRentalDbContext(DbContextOptions<ConstructionEquipmentRentalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderReport> OrderReports { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WalletLog> WalletLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=ConstructionEquipmentRentalDB;User ID=sa;Password=12345;Trusted_Connection=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07407B528F");

            entity.ToTable("Account");

            entity.HasIndex(e => e.StoreId, "UQ__Account__3B82F1003DAA3999").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.GoogleId).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Points).HasDefaultValue(0);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValue("CUSTOMER");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3214EC07BD90E6F8");

            entity.ToTable("Brand");

            entity.Property(e => e.Country).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC070ACFB81A");

            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC071D54CA94");

            entity.ToTable("Feedback");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("PENDING");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Accoun__4F7CD00D");

            entity.HasOne(d => d.Order).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__OrderI__5165187F");

            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Produc__5070F446");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC076182CF89");

            entity.ToTable("Order");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasDefaultValue("CASH");
            entity.Property(e => e.PurchaseMethod)
                .HasMaxLength(50)
                .HasDefaultValue("ONLINE");
            entity.Property(e => e.RecipientName).HasMaxLength(255);
            entity.Property(e => e.RecipientPhone).HasMaxLength(20);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("PENDING");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__440B1D61");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC076EDD49E0");

            entity.ToTable("OrderItem");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__4AB81AF0");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Produ__4BAC3F29");
        });

        modelBuilder.Entity<OrderReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderRep__3214EC0752475D5E");

            entity.ToTable("OrderReport");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ResolvedAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("PENDING");

            entity.HasOne(d => d.Handler).WithMany(p => p.OrderReportHandlers)
                .HasForeignKey(d => d.HandlerId)
                .HasConstraintName("FK__OrderRepo__Handl__5AEE82B9");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderReports)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRepo__Order__59063A47");

            entity.HasOne(d => d.Reporter).WithMany(p => p.OrderReportReporters)
                .HasForeignKey(d => d.ReporterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRepo__Repor__59FA5E80");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07ED216CAC");

            entity.ToTable("Product");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Dimensions).HasMaxLength(100);
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountEndDate).HasColumnType("datetime");
            entity.Property(e => e.DiscountStartDate).HasColumnType("datetime");
            entity.Property(e => e.FuelType).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceSale).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("AVAILABLE");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__BrandId__398D8EEE");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__38996AB5");

            entity.HasOne(d => d.Store).WithMany(p => p.Products)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__StoreId__3A81B327");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC07ED8B0615");

            entity.ToTable("ProductImage");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductIm__Produ__403A8C7D");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E591B1E703D");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.RefreshTokenId).HasColumnName("RefreshTokenID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__Accou__74AE54BC");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Store__3214EC070C6B5491");

            entity.ToTable("Store");

            entity.HasIndex(e => e.AccountId, "UQ__Store__349DA5A78D3EC318").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithOne(p => p.Store)
                .HasForeignKey<Store>(d => d.AccountId)
                .HasConstraintName("FK__Store__AccountId__300424B4");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC072C30DDD5");

            entity.ToTable("Transaction");

            entity.HasIndex(e => e.OrderId, "UQ__Transact__C3905BCE7B32A8E3").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("PENDING");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Accou__6383C8BA");

            entity.HasOne(d => d.Order).WithOne(p => p.Transaction)
                .HasForeignKey<Transaction>(d => d.OrderId)
                .HasConstraintName("FK__Transacti__Order__628FA481");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC077FFEB789");

            entity.ToTable("Wallet");

            entity.HasIndex(e => e.AccountId, "UQ__Wallet__349DA5A7C1399DC7").IsUnique();

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BankAccount).HasMaxLength(255);
            entity.Property(e => e.BankName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE");

            entity.HasOne(d => d.Account).WithOne(p => p.Wallet)
                .HasForeignKey<Wallet>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Wallet__AccountI__6A30C649");
        });

        modelBuilder.Entity<WalletLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WalletLo__3214EC0756D4C3D3");

            entity.ToTable("WalletLog");

            entity.HasIndex(e => e.TransactionId, "UQ__WalletLo__55433A6A22AB5D5E").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("COMPLETED");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Transaction).WithOne(p => p.WalletLog)
                .HasForeignKey<WalletLog>(d => d.TransactionId)
                .HasConstraintName("FK__WalletLog__Trans__71D1E811");

            entity.HasOne(d => d.Wallet).WithMany(p => p.WalletLogs)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WalletLog__Walle__70DDC3D8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
