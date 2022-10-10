﻿//// <auto-generated />
//using System;
//using BookAuthor.Api.DataAccess;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Migrations;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

//#nullable disable

//namespace BookAuthor.Api.Migrations
//{
//    [DbContext(typeof(AppDbContext))]
//    [Migration("20220726010637_NullableDateTimes")]
//    partial class NullableDateTimes
//    {
//        protected override void BuildTargetModel(ModelBuilder modelBuilder)
//        {
//#pragma warning disable 612, 618
//            modelBuilder
//                .HasAnnotation("ProductVersion", "6.0.7")
//                .HasAnnotation("Relational:MaxIdentifierLength", 128);

//            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

//            modelBuilder.Entity("BookAuthor.Api.Model.ApiUser", b =>
//                {
//                    b.Property<string>("Id")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<int>("AccessFailedCount")
//                        .HasColumnType("int");

//                    b.Property<string>("ConcurrencyStamp")
//                        .IsConcurrencyToken()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Email")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<bool>("EmailConfirmed")
//                        .HasColumnType("bit");

//                    b.Property<bool>("LockoutEnabled")
//                        .HasColumnType("bit");

//                    b.Property<DateTimeOffset?>("LockoutEnd")
//                        .HasColumnType("datetimeoffset");

//                    b.Property<string>("NormalizedEmail")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<string>("NormalizedUserName")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<string>("PasswordHash")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("PhoneNumber")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<bool>("PhoneNumberConfirmed")
//                        .HasColumnType("bit");

//                    b.Property<string>("SecurityStamp")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<bool>("TwoFactorEnabled")
//                        .HasColumnType("bit");

//                    b.Property<string>("UserName")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.HasKey("Id");

//                    b.HasIndex("NormalizedEmail")
//                        .HasDatabaseName("EmailIndex");

//                    b.HasIndex("NormalizedUserName")
//                        .IsUnique()
//                        .HasDatabaseName("UserNameIndex")
//                        .HasFilter("[NormalizedUserName] IS NOT NULL");

//                    b.ToTable("AspNetUsers", (string)null);
//                });

//            modelBuilder.Entity("BookAuthor.Api.Model.Author", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

//                    b.Property<string>("About")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<DateTime?>("BirthDate")
//                        .HasColumnType("datetime2");

//                    b.Property<string>("Country")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<DateTime?>("DeathDate")
//                        .HasColumnType("datetime2");

//                    b.Property<string>("HomePlace")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("ImageUrl")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Name")
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.ToTable("Authors");
//                });

//            modelBuilder.Entity("BookAuthor.Api.Model.AuthorBook", b =>
//                {
//                    b.Property<int>("BookId")
//                        .HasColumnType("int");

//                    b.Property<int>("AuthorId")
//                        .HasColumnType("int");

//                    b.HasKey("BookId", "AuthorId");

//                    b.HasIndex("AuthorId");

//                    b.ToTable("AuthorBooks");
//                });

//            modelBuilder.Entity("BookAuthor.Api.Model.Book", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

//                    b.Property<string>("Isbn10")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Isbn13")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<int?>("PageCount")
//                        .HasColumnType("int");

//                    b.Property<DateTime?>("PublicationDate")
//                        .HasColumnType("datetime2");

//                    b.Property<string>("Publisher")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Title")
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.ToTable("Books");
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
//                {
//                    b.Property<string>("Id")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("ConcurrencyStamp")
//                        .IsConcurrencyToken()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Name")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<string>("NormalizedName")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.HasKey("Id");

//                    b.HasIndex("NormalizedName")
//                        .IsUnique()
//                        .HasDatabaseName("RoleNameIndex")
//                        .HasFilter("[NormalizedName] IS NOT NULL");

//                    b.ToTable("AspNetRoles", (string)null);

//                    b.HasData(
//                        new
//                        {
//                            Id = "1ef479ae-b577-4d86-a4de-0f387f402af4",
//                            ConcurrencyStamp = "a1ff26da-568a-44fb-ac68-c1e2325e393d",
//                            Name = "User",
//                            NormalizedName = "USER"
//                        },
//                        new
//                        {
//                            Id = "2c5be23f-f47c-48fe-ae3a-18acef7588e5",
//                            ConcurrencyStamp = "9c2ae9cf-74d8-4b7f-bc3a-2ba279536e5b",
//                            Name = "Admin",
//                            NormalizedName = "ADMIN"
//                        });
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

//                    b.Property<string>("ClaimType")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("ClaimValue")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("RoleId")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("Id");

//                    b.HasIndex("RoleId");

//                    b.ToTable("AspNetRoleClaims", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

//                    b.Property<string>("ClaimType")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("ClaimValue")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("UserId")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("Id");

//                    b.HasIndex("UserId");

//                    b.ToTable("AspNetUserClaims", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
//                {
//                    b.Property<string>("LoginProvider")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("ProviderKey")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("ProviderDisplayName")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("UserId")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("LoginProvider", "ProviderKey");

//                    b.HasIndex("UserId");

//                    b.ToTable("AspNetUserLogins", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
//                {
//                    b.Property<string>("UserId")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("RoleId")
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("UserId", "RoleId");

//                    b.HasIndex("RoleId");

//                    b.ToTable("AspNetUserRoles", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
//                {
//                    b.Property<string>("UserId")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("LoginProvider")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("Name")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("Value")
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("UserId", "LoginProvider", "Name");

//                    b.ToTable("AspNetUserTokens", (string)null);
//                });

//            modelBuilder.Entity("BookAuthor.Api.Model.AuthorBook", b =>
//                {
//                    b.HasOne("BookAuthor.Api.Model.Author", "Author")
//                        .WithMany("AuthorBooks")
//                        .HasForeignKey("AuthorId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.HasOne("BookAuthor.Api.Model.Book", "Book")
//                        .WithMany("AuthorBooks")
//                        .HasForeignKey("BookId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.Navigation("Author");

//                    b.Navigation("Book");
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
//                {
//                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
//                        .WithMany()
//                        .HasForeignKey("RoleId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
//                {
//                    b.HasOne("BookAuthor.Api.Model.ApiUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
//                {
//                    b.HasOne("BookAuthor.Api.Model.ApiUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
//                {
//                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
//                        .WithMany()
//                        .HasForeignKey("RoleId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.HasOne("BookAuthor.Api.Model.ApiUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
//                {
//                    b.HasOne("BookAuthor.Api.Model.ApiUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("BookAuthor.Api.Model.Author", b =>
//                {
//                    b.Navigation("AuthorBooks");
//                });

//            modelBuilder.Entity("BookAuthor.Api.Model.Book", b =>
//                {
//                    b.Navigation("AuthorBooks");
//                });
//#pragma warning restore 612, 618
//        }
//    }
//}
