﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RatJiggler.Data;

#nullable disable

namespace RatJiggler.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("RatJiggler.Data.Entities.ApplicationSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SelectedTabIndex")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ApplicationSettings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            SelectedTabIndex = 0
                        });
                });

            modelBuilder.Entity("RatJiggler.Data.Entities.NormalMovementSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("BackAndForth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MoveX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MoveY")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("NormalMovementSettings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BackAndForth = true,
                            Duration = 60,
                            MoveX = 50,
                            MoveY = 0
                        });
                });

            modelBuilder.Entity("RatJiggler.Data.Entities.RealisticMovementSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableRandomPauses")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableStepPauses")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableUserInterventionDetection")
                        .HasColumnType("INTEGER");

                    b.Property<float>("HorizontalBias")
                        .HasColumnType("REAL");

                    b.Property<int>("MaxSpeed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinSpeed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MovementThresholdInPixels")
                        .HasColumnType("INTEGER");

                    b.Property<float>("PaddingPercentage")
                        .HasColumnType("REAL");

                    b.Property<int>("RandomPauseMax")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RandomPauseMin")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RandomPauseProbability")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RandomSeed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StepPauseMax")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StepPauseMin")
                        .HasColumnType("INTEGER");

                    b.Property<float>("VerticalBias")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("RealisticMovementSettings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EnableRandomPauses = true,
                            EnableStepPauses = true,
                            EnableUserInterventionDetection = true,
                            HorizontalBias = 0f,
                            MaxSpeed = 7,
                            MinSpeed = 3,
                            MovementThresholdInPixels = 10,
                            PaddingPercentage = 0.1f,
                            RandomPauseMax = 500,
                            RandomPauseMin = 100,
                            RandomPauseProbability = 10,
                            StepPauseMax = 50,
                            StepPauseMin = 20,
                            VerticalBias = 0f
                        });
                });

            modelBuilder.Entity("RatJiggler.Data.Entities.UserSettingsEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("BackForth")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<int>("Duration")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(60);

                    b.Property<bool>("EnableRandomPauses")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<bool>("EnableStepPauses")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<bool>("EnableUserInterventionDetection")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<float>("HorizontalBias")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("REAL")
                        .HasDefaultValue(0f);

                    b.Property<int>("MaxSpeed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(7);

                    b.Property<int>("MinSpeed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(3);

                    b.Property<int>("MoveX")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(50);

                    b.Property<int>("MoveY")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("MovementThresholdInPixels")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(10);

                    b.Property<float>("PaddingPercentage")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("REAL")
                        .HasDefaultValue(0.1f);

                    b.Property<int>("RandomPauseMax")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(500);

                    b.Property<int>("RandomPauseMin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(100);

                    b.Property<int>("RandomPauseProbability")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(10);

                    b.Property<int?>("RandomSeed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SelectedMouseMovementModeIndex")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("StepPauseMax")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(50);

                    b.Property<int>("StepPauseMin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(20);

                    b.Property<float>("VerticalBias")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("REAL")
                        .HasDefaultValue(0f);

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });
#pragma warning restore 612, 618
        }
    }
}
