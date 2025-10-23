using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace VehiclePortal.Models;

public partial class VehicleContext : DbContext
{
    public VehicleContext()
    {
    }

    public VehicleContext(DbContextOptions<VehicleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<Checkpost> Checkposts { get; set; }

    public virtual DbSet<Checkpostname> Checkpostnames { get; set; }

    public virtual DbSet<Clusternodalregistration> Clusternodalregistrations { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Nodalregistration> Nodalregistrations { get; set; }

    public virtual DbSet<Source> Sources { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    public virtual DbSet<Vehicleregistration> Vehicleregistrations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connection string is configured in Program.cs via dependency injection
        // This method should not be called in normal operation since we configure it in Program.cs
        // If it is called, it means the DbContext was created without proper DI configuration
        if (!optionsBuilder.IsConfigured)
        {
            // This fallback should not be used in production
            // The connection should always be configured via Program.cs
            throw new InvalidOperationException("DbContext must be configured via dependency injection. " +
                "Make sure VehicleContext is registered in Program.cs and injected properly.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(e => e.BlockId).HasName("PRIMARY");

            entity.ToTable("block");

            entity.HasIndex(e => e.DistrictId, "DistrictId");

            entity.Property(e => e.BlockId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Blockname).HasMaxLength(50);
            entity.Property(e => e.DistrictId).HasColumnType("int(11)");

            entity.HasOne(d => d.District).WithMany(p => p.Blocks)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("block_ibfk_1");
        });

        modelBuilder.Entity<Checkpost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("checkpost");

            entity.HasIndex(e => e.BlockId, "BlockId_idx");

            entity.HasIndex(e => e.CheckpostId, "CheckpostId_idx");

            entity.HasIndex(e => e.DistrictId, "DistrictId_idx");

            entity.HasIndex(e => e.VehicleNo, "VehicleNo_UNIQUE").IsUnique();

            entity.Property(e => e.BlockId).HasColumnType("int(11)");
            entity.Property(e => e.CheckpostId).HasColumnType("int(11)");
            entity.Property(e => e.CurrentDate).HasColumnType("datetime");
            entity.Property(e => e.DistrictId).HasColumnType("int(11)");
            entity.Property(e => e.TotalPeople).HasColumnType("int(11)");
            entity.Property(e => e.VehicleNo).HasMaxLength(20);

            entity.HasOne(d => d.Checkpostname)   // nav property in Checkpost
      .WithMany(p => p.Checkposts)   // collection in Checkpostname
      .HasForeignKey(d => d.CheckpostId) // FK column in Checkpost table
      .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.VehicleNoNavigation).WithOne(p => p.Checkpost)
                .HasForeignKey<Checkpost>(d => d.VehicleNo)
                .HasConstraintName("VehicleNo");
        });

        modelBuilder.Entity<Checkpostname>(entity =>
        {
            entity.HasKey(e => e.CheckpostId).HasName("PRIMARY");

            entity.ToTable("checkpostname");

            entity.Property(e => e.CheckpostId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.CheckpostName)
                .HasMaxLength(75)
                .HasColumnName("checkpostname");
        });

        modelBuilder.Entity<Clusternodalregistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clusternodalregistration");

            entity.Property(e => e.Block).HasMaxLength(45);
            entity.Property(e => e.Gpname)
                .HasMaxLength(45)
                .HasColumnName("GPName");
            entity.Property(e => e.MobileNo).HasMaxLength(15);
            entity.Property(e => e.NodalName).HasMaxLength(45);
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PRIMARY");

            entity.ToTable("district");

            entity.Property(e => e.DistrictId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.DistrictName).HasMaxLength(30);
        });

        modelBuilder.Entity<Nodalregistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("nodalregistration", tb => tb.HasComment("		"));

            entity.Property(e => e.AssitantNodalMobileNo).HasMaxLength(15);
            entity.Property(e => e.AssitantNodalName).HasMaxLength(45);
            entity.Property(e => e.District).HasMaxLength(30);
            entity.Property(e => e.NodalMobileNo).HasMaxLength(15);
            entity.Property(e => e.NodalName).HasMaxLength(45);
        });

        modelBuilder.Entity<Source>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sources");

            entity.HasIndex(e => e.BlockId, "BlockId");

            entity.HasIndex(e => e.DistrictId, "DistrictId");

            entity.HasIndex(e => e.VehicleNo, "VehicleNo").IsUnique();

            entity.Property(e => e.BlockId).HasColumnType("int(11)");
            entity.Property(e => e.CurrentDate).HasColumnType("datetime");
            entity.Property(e => e.DistrictId).HasColumnType("int(11)");
            entity.Property(e => e.TotalPeople).HasColumnType("int(11)");
            entity.Property(e => e.VehicleNo).HasMaxLength(20);

            entity.HasOne(d => d.VehicleNoNavigation).WithOne(p => p.Source)
                .HasForeignKey<Source>(d => d.VehicleNo)
                .HasConstraintName("sources_ibfk_1");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("userlogin");

            entity.HasIndex(e => e.DistrictId, "DistrictId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.DistrictId).HasColumnType("int(11)");
            entity.Property(e => e.LoginRole).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(45);

            entity.HasOne(d => d.District).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("userlogin_ibfk_1");
        });

        modelBuilder.Entity<Vehicleregistration>(entity =>
        {
            entity.HasKey(e => e.VehicleNo).HasName("PRIMARY");

            entity.ToTable("vehicleregistration");

            entity.HasIndex(e => e.BlockId, "BlockId_idx");

            entity.HasIndex(e => e.DistrictId, "DistrictId_idx");

            entity.HasIndex(e => e.VehicleNo, "VehicleNo_UNIQUE").IsUnique();

            entity.Property(e => e.VehicleNo).HasMaxLength(20);
            entity.Property(e => e.BlockId).HasColumnType("int(11)");
            entity.Property(e => e.DistrictId).HasColumnType("int(11)");
            entity.Property(e => e.DriverMobileNo).HasMaxLength(15);
            entity.Property(e => e.DriverName).HasMaxLength(45);
            entity.Property(e => e.Gpname)
                .HasMaxLength(45)
                .HasColumnName("GPName");
            entity.Property(e => e.NodalMobileNo).HasMaxLength(15);
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.SeatCapacity).HasColumnType("int(11)");
            entity.Property(e => e.VehicleNodalName).HasMaxLength(45);
            entity.Property(e => e.VehicleType).HasMaxLength(30);

            entity.HasOne(d => d.Block).WithMany(p => p.Vehicleregistrations)
                .HasForeignKey(d => d.BlockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BlockId");

            entity.HasOne(d => d.District).WithMany(p => p.Vehicleregistrations)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DistrictId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
