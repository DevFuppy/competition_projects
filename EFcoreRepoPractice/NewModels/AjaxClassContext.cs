using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFcoreRepoPractice.NewModels;

public partial class AjaxClassContext : DbContext
{
    public AjaxClassContext()
    {
    }

    public AjaxClassContext(DbContextOptions<AjaxClassContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PasswordToken> PasswordTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=JOJOYAU-PCW10B\\MSSQLSERVER01;Initial Catalog=ajaxClass;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PasswordToken>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsUsed).HasColumnName("isUsed");
            entity.Property(e => e.Token).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
