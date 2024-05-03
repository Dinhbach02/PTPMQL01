﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvcBach.Data;

#nullable disable

namespace MvcBach.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240503090841_Create_table_DaiLy")]
    partial class Create_table_DaiLy
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("MvcBach.Models.DaiLy", b =>
                {
                    b.Property<string>("MaDaiLy")
                        .HasColumnType("TEXT");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DienThoai")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MaHTPP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NguoiDaiDien")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TenDaiLy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("MaDaiLy");

                    b.HasIndex("MaHTPP");

                    b.ToTable("DaiLy");
                });

            modelBuilder.Entity("MvcBach.Models.HeThongPhanPhoi", b =>
                {
                    b.Property<string>("MaHTPP")
                        .HasColumnType("TEXT");

                    b.Property<string>("TenHTPP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("MaHTPP");

                    b.ToTable("HeThongPhanPhoi");
                });

            modelBuilder.Entity("MvcBach.Models.Student", b =>
                {
                    b.Property<string>("StudentID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Age")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("StudentID");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("MvcBach.Models.DaiLy", b =>
                {
                    b.HasOne("MvcBach.Models.HeThongPhanPhoi", "HeThongPhanPhoi")
                        .WithMany()
                        .HasForeignKey("MaHTPP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HeThongPhanPhoi");
                });
#pragma warning restore 612, 618
        }
    }
}
