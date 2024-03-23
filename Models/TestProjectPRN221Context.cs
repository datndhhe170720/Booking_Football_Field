using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectPRN221_2.Models
{
    public partial class TestProjectPRN221Context : DbContext
    {
        public TestProjectPRN221Context()
        {
        }

        public TestProjectPRN221Context(DbContextOptions<TestProjectPRN221Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<FootballField> FootballFields { get; set; } = null!;
        public virtual DbSet<TimeSlot> TimeSlots { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = TestProjectPRN221; uid=sa;pwd=123;Trusted_Connection=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Account__1788CC4CCBD36A36");

                entity.ToTable("Account");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Account1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Account");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Field)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.FieldId)
                    .HasConstraintName("FK__Booking__FieldId__3F466844");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.TimeSlotId)
                    .HasConstraintName("FK__Booking__TimeSlo__403A8C7D");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FootballField>(entity =>
            {
                entity.HasKey(e => e.FieldId)
                    .HasName("PK__Football__C8B6FF07275A18AB");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.FieldName).HasMaxLength(100);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Size).HasMaxLength(50);
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.ToTable("TimeSlot");

                entity.Property(e => e.TimeSlotId).ValueGeneratedNever();

                entity.Property(e => e.Available)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Slot)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
