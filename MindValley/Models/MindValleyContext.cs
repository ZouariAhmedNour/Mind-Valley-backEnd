using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MindValley.Models;

public partial class MindValleyContext : DbContext
{
    public MindValleyContext()
    {
    }

    public MindValleyContext(DbContextOptions<MindValleyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<MoodLog> MoodLogs { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Therapist> Therapists { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=MindValleyContextConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC21419E77A");

            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Therapist).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.TherapistId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Appointme__Thera__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Appointme__UserI__4316F928");
        });

        modelBuilder.Entity<MoodLog>(entity =>
        {
            entity.HasKey(e => e.MoodLogId).HasName("PK__MoodLogs__DADB3626974A74AC");

            entity.Property(e => e.LogDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mood).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.MoodLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MoodLogs__UserId__3B75D760");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK__Resource__4ED1816F49CD7D78");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(255);
        });

        modelBuilder.Entity<Therapist>(entity =>
        {
            entity.HasKey(e => e.TherapistId).HasName("PK__Therapis__4D6219320E5A195E");

            entity.Property(e => e.ContactInfo).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Specialization).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C565E41F0");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A9050E76").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
