
using HowHttpClientWorks.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IWeatherClient, OpenWeatherClient>();

builder.Services.AddHttpClient<IWeatherClient, OpenWeatherClient>(client =>
{
    client.BaseAddress = new("https://api.openweathermap.org/data/2.5");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
