// <auto-generated />
using System;
using CloudCash.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CloudCash.DAL.Migrations
{
    [DbContext(typeof(CloudCashDbContext))]
    [Migration("20210423141654_AttributeUpdate")]
    partial class AttributeUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CS_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "6.0.0-preview.3.21201.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CloudCash.DAL.Entities.Card", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Customer", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BonusPoints")
                        .HasColumnType("bigint");

                    b.Property<long?>("CardID")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CardID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.ExpenseIncome", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("EIType")
                        .HasColumnType("int");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("ExpenseIncomes");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Payment", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CustomerID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Discount")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsPartial")
                        .HasColumnType("bit");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<long?>("TableID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("TableID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Product", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoryID")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("VatLevel")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.ProductCategory", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("PrintSeparately")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Reservation", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("PeopleCount")
                        .HasColumnType("tinyint");

                    b.Property<long?>("SelectedTableID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("SelectedTableID");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Sell", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Discount")
                        .HasColumnType("tinyint");

                    b.Property<long?>("PaymentID")
                        .HasColumnType("bigint");

                    b.Property<long?>("ProductID")
                        .HasColumnType("bigint");

                    b.Property<long>("TableID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("PaymentID");

                    b.HasIndex("ProductID");

                    b.HasIndex("TableID");

                    b.ToTable("Sells");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Shift", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CashValue")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ShiftRecordType")
                        .HasColumnType("int");

                    b.Property<long?>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Table", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<long?>("TableInfoID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("TableInfoID");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.TableCategory", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("TableCategories");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.TableInfo", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoryID")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("PositionX")
                        .HasColumnType("smallint");

                    b.Property<short>("PositionY")
                        .HasColumnType("smallint");

                    b.Property<byte>("Size")
                        .HasColumnType("tinyint");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("TableInfos");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CardID")
                        .HasColumnType("bigint");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Hash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .UseCollation("SQL_Latin1_General_CP1_CS_AS");

                    b.Property<int>("Rights")
                        .HasColumnType("int");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("ID");

                    b.HasIndex("CardID");

                    b.HasIndex("NickName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.UserLog", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("LogType")
                        .HasColumnType("int");

                    b.Property<long?>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("UserLogs");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Customer", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardID");

                    b.Navigation("Card");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.ExpenseIncome", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Payment", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID");

                    b.HasOne("CloudCash.DAL.Entities.Table", "Table")
                        .WithMany("Payments")
                        .HasForeignKey("TableID");

                    b.Navigation("Customer");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Product", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.ProductCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Reservation", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.TableInfo", "SelectedTable")
                        .WithMany()
                        .HasForeignKey("SelectedTableID");

                    b.Navigation("SelectedTable");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Sell", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.Payment", null)
                        .WithMany("Sells")
                        .HasForeignKey("PaymentID");

                    b.HasOne("CloudCash.DAL.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.HasOne("CloudCash.DAL.Entities.Table", "Table")
                        .WithMany("Sells")
                        .HasForeignKey("TableID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Shift", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Table", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.TableInfo", "TableInfo")
                        .WithMany()
                        .HasForeignKey("TableInfoID");

                    b.Navigation("TableInfo");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.TableInfo", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.TableCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.User", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardID");

                    b.Navigation("Card");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.UserLog", b =>
                {
                    b.HasOne("CloudCash.DAL.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Payment", b =>
                {
                    b.Navigation("Sells");
                });

            modelBuilder.Entity("CloudCash.DAL.Entities.Table", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Sells");
                });
#pragma warning restore 612, 618
        }
    }
}
