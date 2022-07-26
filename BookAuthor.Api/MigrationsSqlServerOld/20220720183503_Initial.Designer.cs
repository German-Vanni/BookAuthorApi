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
//    [Migration("20220720183503_Initial")]
//    partial class Initial
//    {
//        protected override void BuildTargetModel(ModelBuilder modelBuilder)
//        {
//#pragma warning disable 612, 618
//            modelBuilder
//                .HasAnnotation("ProductVersion", "6.0.7")
//                .HasAnnotation("Relational:MaxIdentifierLength", 128);

//            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

//            modelBuilder.Entity("BookAuthor.Api.Model.Author", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

//                    b.Property<DateTime>("BirthDay")
//                        .HasColumnType("datetime2");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.ToTable("Authors");
//                });

//            modelBuilder.Entity("BookAuthor.Api.Model.Book", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

//                    b.Property<int>("AuthorId")
//                        .HasColumnType("int");

//                    b.Property<string>("ISBN10")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("ISBN13")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Title")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.HasIndex("AuthorId");

//                    b.ToTable("Books");
//                });

//            modelBuilder.Entity("BookAuthor.Api.Model.Book", b =>
//                {
//                    b.HasOne("BookAuthor.Api.Model.Author", "Author")
//                        .WithMany("Books")
//                        .HasForeignKey("AuthorId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.Navigation("Author");
//                });

//            modelBuilder.Entity("BookAuthor.Api.Model.Author", b =>
//                {
//                    b.Navigation("Books");
//                });
//#pragma warning restore 612, 618
//        }
//    }
//}
