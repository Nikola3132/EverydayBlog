﻿// <auto-generated />
using System;
using EveryDayBlog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EveryDayBlog.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190711094647_ImageContentType")]
    partial class ImageContentType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EveryDayBlog.Data.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("ImageId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentType")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<byte[]>("ImageByte")
                        .IsRequired();

                    b.Property<string>("ImagePath");

                    b.Property<string>("ImageTitle");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.PageHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("ImageId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("PageIndicator")
                        .IsRequired();

                    b.Property<string>("SubTitle")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("IsDeleted");

                    b.ToTable("PagesHeaders");
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.Paragraph", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("ImageId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("MainText")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int?>("SectionId");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("SectionId");

                    b.ToTable("Paragraphs");
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("PageHeaderId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("PageHeaderId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int?>("PostId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("PostId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.ApplicationUser", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.PageHeader", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.Paragraph", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EveryDayBlog.Data.Models.Section")
                        .WithMany("Paragraphs")
                        .HasForeignKey("SectionId");
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.Post", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.PageHeader", "PageHeader")
                        .WithMany()
                        .HasForeignKey("PageHeaderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EveryDayBlog.Data.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EveryDayBlog.Data.Models.Section", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.Post")
                        .WithMany("Sections")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EveryDayBlog.Data.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EveryDayBlog.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
