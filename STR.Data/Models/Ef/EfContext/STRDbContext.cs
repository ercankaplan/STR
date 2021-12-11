using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Data.Models.Ef.EfContext
{
    public class STRDbContext : DbContext
    {
        public readonly string Schema = "public";
        public STRDbContext(DbContextOptions<STRDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ReportRequest> ReportRequest { get; set; }
        public virtual DbSet<ReportResult> ReportResult { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Person>(entity =>
            {

                entity.ToTable(nameof(Person), Schema);
                entity.Property(e => e.Id).ValueGeneratedNever().HasColumnType("uuid").IsRequired();
                entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
                entity.Property(p => p.Surname).HasMaxLength(100);
                entity.Property(p => p.CompanyName).HasMaxLength(300);
             

            });

            modelBuilder.Entity<Contact>(entity =>
            {

                entity.ToTable(nameof(Contact), Schema);
                entity.Property(e => e.Id).ValueGeneratedNever().HasColumnType("uuid").IsRequired();
                entity.Property(p => p.ContactInfo).HasMaxLength(50);
                entity.Property(p => p.ContactType).IsRequired();
             

                entity.HasOne(d => d.Person).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK_Contact_Person")
                .OnDelete(DeleteBehavior.Cascade);



            });

            modelBuilder.Entity<Report>(entity =>
            {

                entity.ToTable(nameof(Report), Schema);
                entity.Property(e => e.Id).ValueGeneratedNever().HasColumnType("uuid").IsRequired();
                entity.Property(p => p.Name).HasMaxLength(200).IsRequired();
                entity.Property(p => p.Description).IsRequired();
              

            });

            modelBuilder.Entity<ReportRequest>(entity =>
            {

                entity.ToTable(nameof(ReportRequest), Schema);
                entity.Property(e => e.Id).ValueGeneratedNever().HasColumnType("uuid").IsRequired();
                entity.Property(p => p.Status).HasMaxLength(200).IsRequired();
            

                entity.HasOne(d => d.Report)
                .WithMany(p => p.ReportRequests)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("FK_ReportRequest_Report")
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<ReportResult>(entity =>
            {
                entity.ToTable(nameof(ReportResult), Schema);
                entity.Property(e => e.Id).ValueGeneratedNever().HasColumnType("uuid").IsRequired();
                entity.Property(e => e.ReportRequestId).ValueGeneratedNever();

                entity.HasOne(d => d.ReportRequest)
                    .WithOne(p => p.ReportResult)
                    .HasForeignKey<ReportResult>(d => d.ReportRequestId)
                    .HasConstraintName("FK_ReportResult_ReportRequest")
                    .OnDelete(DeleteBehavior.Cascade);


            });



            base.OnModelCreating(modelBuilder);
        }
    }
}
