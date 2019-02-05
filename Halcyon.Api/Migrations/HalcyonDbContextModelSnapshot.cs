﻿// <auto-generated />
using System;
using Halcyon.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Halcyon.Api.Migrations
{
    [DbContext(typeof(HalcyonDbContext))]
    partial class HalcyonDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Halcyon.Api.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(254);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("IsLockedOut");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .HasMaxLength(128);

                    b.Property<string>("PasswordResetToken")
                        .HasMaxLength(36);

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("TwoFactorSecret")
                        .HasMaxLength(50);

                    b.Property<string>("TwoFactorTempSecret")
                        .HasMaxLength(50);

                    b.Property<string>("VerifyEmailToken")
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Halcyon.Api.Entities.UserLogin", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Provider");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("Provider", "ExternalId")
                        .IsUnique();

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Halcyon.Api.Entities.UserRefreshToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<DateTime>("Issued");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserRefreshToken");
                });

            modelBuilder.Entity("Halcyon.Api.Entities.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Halcyon.Api.Entities.UserLogin", b =>
                {
                    b.HasOne("Halcyon.Api.Entities.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halcyon.Api.Entities.UserRefreshToken", b =>
                {
                    b.HasOne("Halcyon.Api.Entities.User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Halcyon.Api.Entities.UserRole", b =>
                {
                    b.HasOne("Halcyon.Api.Entities.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
