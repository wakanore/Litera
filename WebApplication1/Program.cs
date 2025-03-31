using Application;
using Application.Services;
using Infrastructure;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Registration repositories
builder.Services.AddScoped<IAuthorRepository, AuthorPostgresRepository>();
builder.Services.AddScoped<IBookRepository, BookPostgresRepository>();
builder.Services.AddScoped<IReaderRepository, ReaderPostgresRepository>();
builder.Services.AddScoped<IFavouriteRepository, FavouritePostgresRepository>();

// Registration service
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IFavouriteService, FavouriteService>();

builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connection = new NpgsqlConnection(builder.Configuration.GetConnectionString("PostgreSQL"));
    connection.Open();
    return connection;
});

builder.Services.AddScoped<AppDbContext>();



builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


