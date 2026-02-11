using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DBModels.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatLocking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Genre = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Cast = table.Column<string>(type: "text", nullable: false),
                    Director = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    movieid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    duration = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    releasedate = table.Column<DateOnly>(type: "date", nullable: true),
                    genre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("movies_pkey", x => x.movieid);
                });

            migrationBuilder.CreateTable(
                name: "theaters",
                columns: table => new
                {
                    theaterid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    location = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("theaters_pkey", x => x.theaterid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    passwordhash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    passwordsalt = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    VenueId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    TotalCapacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.VenueId);
                });

            migrationBuilder.CreateTable(
                name: "MovieTheater",
                columns: table => new
                {
                    MoviesMovieid = table.Column<int>(type: "integer", nullable: false),
                    TheatersTheaterid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTheater", x => new { x.MoviesMovieid, x.TheatersTheaterid });
                    table.ForeignKey(
                        name: "FK_MovieTheater_movies_MoviesMovieid",
                        column: x => x.MoviesMovieid,
                        principalTable: "movies",
                        principalColumn: "movieid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieTheater_theaters_TheatersTheaterid",
                        column: x => x.TheatersTheaterid,
                        principalTable: "theaters",
                        principalColumn: "theaterid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movietheaters",
                columns: table => new
                {
                    movieid = table.Column<int>(type: "integer", nullable: false),
                    theaterid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("movietheater_pkey", x => new { x.movieid, x.theaterid });
                    table.ForeignKey(
                        name: "movietheaters_movieid_fkey",
                        column: x => x.movieid,
                        principalTable: "movies",
                        principalColumn: "movieid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "movietheaters_theaterid_fkey",
                        column: x => x.theaterid,
                        principalTable: "theaters",
                        principalColumn: "theaterid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seats",
                columns: table => new
                {
                    seatid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    theaterid = table.Column<int>(type: "integer", nullable: false),
                    row = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    baseprice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("seats_pkey", x => x.seatid);
                    table.ForeignKey(
                        name: "seats_theaterid_fkey",
                        column: x => x.theaterid,
                        principalTable: "theaters",
                        principalColumn: "theaterid");
                });

            migrationBuilder.CreateTable(
                name: "showtemplates",
                columns: table => new
                {
                    showtemplateid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<int>(type: "integer", nullable: true),
                    VenueId = table.Column<int>(type: "integer", nullable: true),
                    movieid = table.Column<int>(type: "integer", nullable: true),
                    theaterid = table.Column<int>(type: "integer", nullable: true),
                    language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    format = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    baseprice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("showtemplates_pkey", x => x.showtemplateid);
                    table.ForeignKey(
                        name: "FK_showtemplates_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_showtemplates_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId");
                    table.ForeignKey(
                        name: "showtemplates_movieid_fkey",
                        column: x => x.movieid,
                        principalTable: "movies",
                        principalColumn: "movieid");
                    table.ForeignKey(
                        name: "showtemplates_theaterid_fkey",
                        column: x => x.theaterid,
                        principalTable: "theaters",
                        principalColumn: "theaterid");
                });

            migrationBuilder.CreateTable(
                name: "VenueSections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VenueId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    BasePriceMultiplier = table.Column<decimal>(type: "numeric", nullable: false),
                    RowCount = table.Column<int>(type: "integer", nullable: false),
                    SeatsPerRow = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueSections", x => x.SectionId);
                    table.ForeignKey(
                        name: "FK_VenueSections_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "showinstances",
                columns: table => new
                {
                    showinstanceid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    showtemplateid = table.Column<int>(type: "integer", nullable: true),
                    showdate = table.Column<DateOnly>(type: "date", nullable: true),
                    showtime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    availableseats = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("showinstances_pkey", x => x.showinstanceid);
                    table.ForeignKey(
                        name: "showinstances_showtemplateid_fkey",
                        column: x => x.showtemplateid,
                        principalTable: "showtemplates",
                        principalColumn: "showtemplateid");
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    bookingid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    seatnumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    showtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    bookingtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: true),
                    VenueId = table.Column<int>(type: "integer", nullable: true),
                    movieid = table.Column<int>(type: "integer", nullable: true),
                    theaterid = table.Column<int>(type: "integer", nullable: true),
                    seatid = table.Column<int>(type: "integer", nullable: true),
                    showinstanceid = table.Column<int>(type: "integer", nullable: true),
                    baseprice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    refundamount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    isrefunded = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    cancelledat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    emailsent = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("bookings_pkey", x => x.bookingid);
                    table.ForeignKey(
                        name: "FK_bookings_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_bookings_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId");
                    table.ForeignKey(
                        name: "bookings_movieid_fkey",
                        column: x => x.movieid,
                        principalTable: "movies",
                        principalColumn: "movieid");
                    table.ForeignKey(
                        name: "bookings_seatid_fkey",
                        column: x => x.seatid,
                        principalTable: "seats",
                        principalColumn: "seatid");
                    table.ForeignKey(
                        name: "bookings_showinstanceid_fkey",
                        column: x => x.showinstanceid,
                        principalTable: "showinstances",
                        principalColumn: "showinstanceid");
                    table.ForeignKey(
                        name: "bookings_theaterid_fkey",
                        column: x => x.theaterid,
                        principalTable: "theaters",
                        principalColumn: "theaterid");
                    table.ForeignKey(
                        name: "bookings_userid_fkey",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateTable(
                name: "showseatstatuses",
                columns: table => new
                {
                    showseatstatusid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    showinstanceid = table.Column<int>(type: "integer", nullable: false),
                    seatid = table.Column<int>(type: "integer", nullable: false),
                    isbooked = table.Column<bool>(type: "boolean", nullable: false),
                    LockedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("showseatstatuses_pkey", x => x.showseatstatusid);
                    table.ForeignKey(
                        name: "showseatstatuses_seatid_fkey",
                        column: x => x.seatid,
                        principalTable: "seats",
                        principalColumn: "seatid");
                    table.ForeignKey(
                        name: "showseatstatuses_showinstanceid_fkey",
                        column: x => x.showinstanceid,
                        principalTable: "showinstances",
                        principalColumn: "showinstanceid");
                });

            migrationBuilder.CreateTable(
                name: "billingsummaries",
                columns: table => new
                {
                    billingsummaryid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bookingid = table.Column<int>(type: "integer", nullable: true),
                    baseamount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    gst = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    servicefee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    discount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    totalamount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("billingsummaries_pkey", x => x.billingsummaryid);
                    table.ForeignKey(
                        name: "billingsummaries_bookingid_fkey",
                        column: x => x.bookingid,
                        principalTable: "bookings",
                        principalColumn: "bookingid");
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    paymentid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    amountpaid = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    paymentdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    paymentmethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    bookingid = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValueSql: "'Success'::character varying"),
                    refunddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("payments_pkey", x => x.paymentid);
                    table.ForeignKey(
                        name: "payments_bookingid_fkey",
                        column: x => x.bookingid,
                        principalTable: "bookings",
                        principalColumn: "bookingid");
                    table.ForeignKey(
                        name: "payments_userid_fkey",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_billingsummaries_bookingid",
                table: "billingsummaries",
                column: "bookingid");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_EventId",
                table: "bookings",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_movieid",
                table: "bookings",
                column: "movieid");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_seatid",
                table: "bookings",
                column: "seatid");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_showinstanceid",
                table: "bookings",
                column: "showinstanceid");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_theaterid",
                table: "bookings",
                column: "theaterid");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_userid",
                table: "bookings",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_VenueId",
                table: "bookings",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieTheater_TheatersTheaterid",
                table: "MovieTheater",
                column: "TheatersTheaterid");

            migrationBuilder.CreateIndex(
                name: "IX_movietheaters_theaterid",
                table: "movietheaters",
                column: "theaterid");

            migrationBuilder.CreateIndex(
                name: "IX_payments_bookingid",
                table: "payments",
                column: "bookingid");

            migrationBuilder.CreateIndex(
                name: "IX_payments_userid",
                table: "payments",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_seats_theaterid",
                table: "seats",
                column: "theaterid");

            migrationBuilder.CreateIndex(
                name: "IX_showinstances_showtemplateid",
                table: "showinstances",
                column: "showtemplateid");

            migrationBuilder.CreateIndex(
                name: "IX_showseatstatuses_seatid",
                table: "showseatstatuses",
                column: "seatid");

            migrationBuilder.CreateIndex(
                name: "IX_showseatstatuses_showinstanceid",
                table: "showseatstatuses",
                column: "showinstanceid");

            migrationBuilder.CreateIndex(
                name: "IX_showtemplates_EventId",
                table: "showtemplates",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_showtemplates_movieid",
                table: "showtemplates",
                column: "movieid");

            migrationBuilder.CreateIndex(
                name: "IX_showtemplates_theaterid",
                table: "showtemplates",
                column: "theaterid");

            migrationBuilder.CreateIndex(
                name: "IX_showtemplates_VenueId",
                table: "showtemplates",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueSections_VenueId",
                table: "VenueSections",
                column: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "billingsummaries");

            migrationBuilder.DropTable(
                name: "MovieTheater");

            migrationBuilder.DropTable(
                name: "movietheaters");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "showseatstatuses");

            migrationBuilder.DropTable(
                name: "VenueSections");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "seats");

            migrationBuilder.DropTable(
                name: "showinstances");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "showtemplates");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Venues");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "theaters");
        }
    }
}
