﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using xUnitMockApi.Models;

namespace xUnitMockApi.Migrations
{
    [DbContext(typeof(MockContext))]
    partial class MockContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("xUnitMockApi.Models.Engine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacity");

                    b.Property<string>("Configuration");

                    b.Property<string>("FuelType");

                    b.HasKey("Id");

                    b.ToTable("Engines");
                });

            modelBuilder.Entity("xUnitMockApi.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("xUnitMockApi.Models.VehicleEngine", b =>
                {
                    b.Property<int>("VehicleId");

                    b.Property<int>("EngineId");

                    b.Property<int>("Id");

                    b.HasKey("VehicleId", "EngineId");

                    b.HasIndex("EngineId");

                    b.HasIndex("VehicleId")
                        .IsUnique();

                    b.ToTable("VehicleEngines");
                });

            modelBuilder.Entity("xUnitMockApi.Models.VehicleWheel", b =>
                {
                    b.Property<int>("VehicleId");

                    b.Property<int>("WheelId");

                    b.Property<int>("Id");

                    b.HasKey("VehicleId", "WheelId");

                    b.HasIndex("WheelId");

                    b.ToTable("VehicleWheels");
                });

            modelBuilder.Entity("xUnitMockApi.Models.Wheel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Size");

                    b.Property<int>("Width");

                    b.HasKey("Id");

                    b.ToTable("Wheels");
                });

            modelBuilder.Entity("xUnitMockApi.Models.VehicleEngine", b =>
                {
                    b.HasOne("xUnitMockApi.Models.Engine", "Engine")
                        .WithMany()
                        .HasForeignKey("EngineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("xUnitMockApi.Models.Vehicle", "Vehicle")
                        .WithOne("Engine")
                        .HasForeignKey("xUnitMockApi.Models.VehicleEngine", "VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("xUnitMockApi.Models.VehicleWheel", b =>
                {
                    b.HasOne("xUnitMockApi.Models.Vehicle", "Vehicle")
                        .WithMany("Wheels")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("xUnitMockApi.Models.Wheel", "Wheel")
                        .WithMany()
                        .HasForeignKey("WheelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
