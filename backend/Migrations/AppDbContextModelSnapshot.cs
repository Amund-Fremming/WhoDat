﻿// <auto-generated />
using System;
using Backend.Features.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RaptorProject.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BoardCardEntity.BoardCard", b =>
                {
                    b.Property<int>("BoardCardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BoardCardID"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<int>("BoardID")
                        .HasColumnType("integer");

                    b.Property<int>("CardID")
                        .HasColumnType("integer");

                    b.HasKey("BoardCardID");

                    b.HasIndex("BoardID");

                    b.HasIndex("CardID");

                    b.ToTable("BoardCard");
                });

            modelBuilder.Entity("BoardEntity.Board", b =>
                {
                    b.Property<int>("BoardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BoardID"));

                    b.Property<int?>("ChosenCardID")
                        .HasColumnType("integer");

                    b.Property<int>("GameID")
                        .HasColumnType("integer");

                    b.Property<int>("PlayerID")
                        .HasColumnType("integer");

                    b.Property<int>("PlayersLeft")
                        .HasColumnType("integer");

                    b.HasKey("BoardID");

                    b.HasIndex("ChosenCardID");

                    b.HasIndex("GameID");

                    b.HasIndex("PlayerID");

                    b.ToTable("Board");
                });

            modelBuilder.Entity("CardEntity.Card", b =>
                {
                    b.Property<int>("CardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CardID"));

                    b.Property<string>("Name")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<int>("PlayerID")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.HasKey("CardID");

                    b.HasIndex("PlayerID");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("GameEntity.Game", b =>
                {
                    b.Property<int>("GameID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GameID"));

                    b.Property<int?>("PlayerOneID")
                        .HasColumnType("integer");

                    b.Property<int?>("PlayerTwoID")
                        .HasColumnType("integer");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("GameID");

                    b.HasIndex("PlayerOneID");

                    b.HasIndex("PlayerTwoID");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("MessageEntity.Message", b =>
                {
                    b.Property<int>("MessageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MessageID"));

                    b.Property<int?>("BoardID")
                        .HasColumnType("integer");

                    b.Property<int>("GameID")
                        .HasColumnType("integer");

                    b.Property<string>("MessageText")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<int>("PlayerID")
                        .HasColumnType("integer");

                    b.HasKey("MessageID");

                    b.HasIndex("BoardID");

                    b.HasIndex("GameID");

                    b.HasIndex("PlayerID");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("PlayerEntity.Player", b =>
                {
                    b.Property<int>("PlayerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlayerID"));

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PlayerID");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("BoardCardEntity.BoardCard", b =>
                {
                    b.HasOne("BoardEntity.Board", "Board")
                        .WithMany("BoardCards")
                        .HasForeignKey("BoardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CardEntity.Card", "Card")
                        .WithMany("BoardCards")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("Card");
                });

            modelBuilder.Entity("BoardEntity.Board", b =>
                {
                    b.HasOne("BoardCardEntity.BoardCard", "ChosenCard")
                        .WithMany()
                        .HasForeignKey("ChosenCardID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GameEntity.Game", "Game")
                        .WithMany("Boards")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlayerEntity.Player", "Player")
                        .WithMany("Boards")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChosenCard");

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("CardEntity.Card", b =>
                {
                    b.HasOne("PlayerEntity.Player", "Player")
                        .WithMany("Cards")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("GameEntity.Game", b =>
                {
                    b.HasOne("PlayerEntity.Player", "PlayerOne")
                        .WithMany("GamesAsPlayerOne")
                        .HasForeignKey("PlayerOneID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlayerEntity.Player", "PlayerTwo")
                        .WithMany("GamesAsPlayerTwo")
                        .HasForeignKey("PlayerTwoID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("PlayerOne");

                    b.Navigation("PlayerTwo");
                });

            modelBuilder.Entity("MessageEntity.Message", b =>
                {
                    b.HasOne("BoardEntity.Board", null)
                        .WithMany("Messages")
                        .HasForeignKey("BoardID");

                    b.HasOne("GameEntity.Game", "Game")
                        .WithMany("Messages")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlayerEntity.Player", "Player")
                        .WithMany("Messages")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("BoardEntity.Board", b =>
                {
                    b.Navigation("BoardCards");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("CardEntity.Card", b =>
                {
                    b.Navigation("BoardCards");
                });

            modelBuilder.Entity("GameEntity.Game", b =>
                {
                    b.Navigation("Boards");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("PlayerEntity.Player", b =>
                {
                    b.Navigation("Boards");

                    b.Navigation("Cards");

                    b.Navigation("GamesAsPlayerOne");

                    b.Navigation("GamesAsPlayerTwo");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
