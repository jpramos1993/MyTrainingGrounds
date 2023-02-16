using FluentValidation;
using Microsoft.Extensions.Options;
using OptionsValidation;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddOptions<ExampleOptions>()
                .Bind(config.GetSection(ExampleOptions.SectionName))
                //.Validate(x =>
                //{
                //    if(x.Retries is <= 10 or > 10)
                //    {
                //        return false;
                //    }
                //    return true;
                //})
                //.ValidateDataAnnotations()
                .ValidateFluently()
                .ValidateOnStart();

var app = builder.Build();

app.MapGet("hello", (IOptions<ExampleOptions> opt) => opt.Value);

app.Run();
