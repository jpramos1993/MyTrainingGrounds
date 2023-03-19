using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.API.Data;
using ShopOnline.API.Repositories;
using ShopOnline.API.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<ShopOnlineDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection")));
//
// Go To Package Manager Console in Tools => NuGet Package Manager => Console
// In the console insert => Add-Migration <MigrationName>
// This will create the migration folder where all data, tables, etc. are located
// Next in the console insert => update-database
//

//
// AddTransient => New instance is provided in dependency injection
// AddSingleton => Same class is provided in dependency injection
// Add Scoped => inbetween between transient and singleton
//

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.WithOrigins("http://localhost:7275", "https://localhost:7275")
                            .AllowAnyMethod()
                            .WithHeaders(HeaderNames.ContentType)
); // Do not place a last /

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
