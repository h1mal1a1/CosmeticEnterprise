using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class OrderConfig : IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.IdUser)
            .HasColumnName("id_user")
            .IsRequired();

        builder.Property(x => x.IdUserAddress)
            .HasColumnName("id_user_address")
            .IsRequired();

        builder.Property(x => x.IdSalesChannel)
            .HasColumnName("id_sales_channel")
            .IsRequired();

        builder.Property(x => x.OrderStatus)
            .HasColumnName("order_status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.DeliveryStatus)
            .HasColumnName("delivery_status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.PaymentType)
            .HasColumnName("payment_type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .HasColumnName("payment_method")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.PaymentStatus)
            .HasColumnName("payment_status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasColumnName("total_amount")
            .HasPrecision(18, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(x => x.DeliveryPrice)
            .HasColumnName("delivery_price")
            .HasPrecision(18, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasColumnName("comment");

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.HasIndex(x => x.IdUser)
            .HasDatabaseName("IX_orders_id_user");

        builder.HasIndex(x => x.IdUserAddress)
            .HasDatabaseName("IX_orders_id_user_address");

        builder.HasIndex(x => x.IdSalesChannel)
            .HasDatabaseName("IX_orders_id_sales_channel");

        builder.HasOne(x => x.User)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.IdUser)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UserAddress)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.IdUserAddress)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SalesChannel)
            .WithMany(x => x.OrdersList)
            .HasForeignKey(x => x.IdSalesChannel)
            .OnDelete(DeleteBehavior.Restrict);
    }
}