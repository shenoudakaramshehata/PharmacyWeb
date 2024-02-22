using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pharmacy.Models;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace Pharmacy.Data
{
    public partial class PharmacyContext : DbContext
    {
        public PharmacyContext()
        {
        }

        public PharmacyContext(DbContextOptions<PharmacyContext> options)
            : base(options)
        {
        }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Area> Area { get; set; }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<CollectionItem> CollectionItems { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerFav> CustomerFav { get; set; }
        public virtual DbSet<HomeSlider> HomeSliders { get; set; }
        public virtual DbSet<HomeSliderType> HomeSliderTypes { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemImage> ItemImages { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Newsletter> Newsletters { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }

        public virtual DbSet<PageContent> PageContent { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.BrandPic);

                entity.Property(e => e.BrandTlAr);

                entity.Property(e => e.BrandTlEn);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryPic);

                entity.Property(e => e.CategoryTlAr);

                entity.Property(e => e.CategoryTlEn);

               
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.Property(e => e.CollectionTlAr);

                entity.Property(e => e.CollectionTlEn);
            });

            modelBuilder.Entity<CollectionItem>(entity =>
            {
                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.CollectionItem)
                    .HasForeignKey(d => d.CollectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectionItem_Collection");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.CollectionItem)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectionItem_Item");
            });

          

            modelBuilder.Entity<ContactUs>(entity =>
            {
                entity.Property(e => e.Email);

                entity.Property(e => e.FullName);

                entity.Property(e => e.Mobile);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerAddress);

                entity.Property(e => e.CustomerEmail);

                entity.Property(e => e.CustomerImage);

                entity.Property(e => e.CustomerNameAr);

                entity.Property(e => e.CustomerNameEn);

                entity.Property(e => e.CustomerPhone);

                entity.Property(e => e.CustomerRemarks);
            });

            modelBuilder.Entity<CustomerFav>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerFav)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerFav_Customer");
            });

            modelBuilder.Entity<HomeSlider>(entity =>
            {
                entity.Property(e => e.HomeSliderEntityId);

                entity.Property(e => e.HomeSliderPic);

                entity.HasOne(d => d.HomeSliderType)
                    .WithMany(p => p.HomeSlider)
                    .HasForeignKey(d => d.HomeSliderTypeId)
                    .HasConstraintName("FK_HomeSlider_HomeSliderType");
            });

            modelBuilder.Entity<HomeSliderType>(entity =>
            {
                entity.Property(e => e.HomeSliderTypeTlAr);

                entity.Property(e => e.HomeSliderTypeTlEn);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ItemTlAr);

                entity.Property(e => e.ItemTlEn);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Item_Brand");
            });

            modelBuilder.Entity<ItemImage>(entity =>
            {
                entity.Property(e => e.ImageUrl);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemImage)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemImage_Item");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Addrerss);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderSerial);

                entity.Property(e => e.Remarks);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Order_Customer");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.Qty).HasColumnName("QTY");

                entity.Property(e => e.Remakrs);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItem_Item");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderItem_Order");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.PaymentMethodTlAr);

                entity.Property(e => e.PaymentMethodTlEn);
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.Property(e => e.SectionPic);

                entity.Property(e => e.SectionTlAr);

                entity.Property(e => e.SectionTlEn);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
