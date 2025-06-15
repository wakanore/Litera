using Application;
using Application.Services;
using FluentValidation;
using Infrastructure;
using Npgsql;
using System.Data;
using Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Domain;
using Application.Validators;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<CreateBookRequestValidator>();
        fv.AutomaticValidationEnabled = true;
        fv.ImplicitlyValidateChildProperties = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
builder.Services.AddSingleton<NpgsqlDataSource>(_ => NpgsqlDataSource.Create(connectionString));
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connection = sp.GetRequiredService<NpgsqlDataSource>().CreateConnection();
    connection.Open();
    return connection;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IAuthorRepository, AuthorPostgresRepository>();
builder.Services.AddScoped<IBookRepository, BookPostgresRepository>();
builder.Services.AddScoped<IReaderRepository, ReaderPostgresRepository>();
builder.Services.AddScoped<IFavouriteRepository, FavouritePostgresRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IFavouriteService, FavouriteService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateAuthorRequest>, UpdateAuthorRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateReaderRequest>, UpdateReaderRequestValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();



