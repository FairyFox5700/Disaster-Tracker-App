﻿// <auto-generated />
using System;
using DisasterTrackerApp.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DisasterTrackerApp.Dal.Migrations
{
    [DbContext(typeof(DisasterTrackerContext))]
    partial class DisasterTrackerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DisasterTrackerApp.Entities.CalendarEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CalendarId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CalendarId1")
                        .HasColumnType("uuid");

                    b.Property<Point>("Coordiantes")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndTs")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GoogleEventId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartedTs")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId1");

                    b.HasIndex("GoogleEventId")
                        .IsUnique();

                    b.ToTable("CalendarEvents");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.CategoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DisasterPropertyEntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("ExternalApiId")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DisasterPropertyEntityId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.DisasterEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Geometry>("Geometry")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.Property<Guid>("PropertiesId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PropertiesId");

                    b.ToTable("DisasterEvent");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.DisasterPropertyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Closed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DisasterPropertyEntity");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.GoogleCalendar", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("GoogleCalendarId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("Primary")
                        .HasColumnType("boolean");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GoogleCalendarId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.GoogleUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("ExpiresIn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("GoogleUsers");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.SourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DisasterPropertyEntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("ExternalApiId")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DisasterPropertyEntityId");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.CalendarEvent", b =>
                {
                    b.HasOne("DisasterTrackerApp.Entities.GoogleCalendar", "Calendar")
                        .WithMany("CalendarEvents")
                        .HasForeignKey("CalendarId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Calendar");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.CategoryEntity", b =>
                {
                    b.HasOne("DisasterTrackerApp.Entities.DisasterPropertyEntity", null)
                        .WithMany("Categories")
                        .HasForeignKey("DisasterPropertyEntityId");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.DisasterEvent", b =>
                {
                    b.HasOne("DisasterTrackerApp.Entities.DisasterPropertyEntity", "Properties")
                        .WithMany()
                        .HasForeignKey("PropertiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Properties");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.GoogleCalendar", b =>
                {
                    b.HasOne("DisasterTrackerApp.Entities.GoogleUser", "User")
                        .WithMany("Calendars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.SourceEntity", b =>
                {
                    b.HasOne("DisasterTrackerApp.Entities.DisasterPropertyEntity", null)
                        .WithMany("Sources")
                        .HasForeignKey("DisasterPropertyEntityId");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.DisasterPropertyEntity", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Sources");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.GoogleCalendar", b =>
                {
                    b.Navigation("CalendarEvents");
                });

            modelBuilder.Entity("DisasterTrackerApp.Entities.GoogleUser", b =>
                {
                    b.Navigation("Calendars");
                });
#pragma warning restore 612, 618
        }
    }
}
