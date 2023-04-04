using System;
using System.Collections.Generic;
using System.Text;
using AppliFact.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppliFact.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, String,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Prestation> Prestations { get; set; }

        public DbSet<Paiement> Paiements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });
            modelBuilder.Entity<ApplicationUser>()
    .Property(e => e.Id)
    .ValueGeneratedOnAdd();
            //modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Clients)
            //    .WithOne(e => e.User).HasForeignKey(uc => uc.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Facture>().HasKey(c => c.Id);
            modelBuilder.Entity<Paiement>().HasKey(c => c.Id);
            modelBuilder.Entity<Prestation>(builder => {

                builder.HasKey(c => c.Id);
                builder.Property(c => c.MontantHorsTax).HasColumnType("Decimal(18,2)");
                builder.Property(c => c.TauxTva).HasColumnType("Decimal(18,2)");

            });
            //modelBuilder.Entity<ApplicationUser>().HasKey(c => c.Id);
            
            modelBuilder.Entity<Client>(builder => {
                builder.HasMany(c => c.Factures)
                    .WithOne(c => c.Client).HasForeignKey(c => c.IdClient)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasMany(c => c.Paiements).WithOne(c => c.Client).HasForeignKey(c => c.IdClient)
                   .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Facture>().HasMany(c => c.Prestations)
                .WithOne(c => c.Facture).HasForeignKey(c => c.IdFacture)
                .OnDelete(DeleteBehavior.Cascade);
        }
       // public DbSet<AppliFact.Models.Client> Client { get; set; }
    }
}
