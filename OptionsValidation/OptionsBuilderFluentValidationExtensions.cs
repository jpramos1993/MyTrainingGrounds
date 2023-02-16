using FluentValidation;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OptionsValidation
{
    public static class OptionsBuilderFluentValidationExtensions
    {
        public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
        {
            optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
                s => new FluentValidationOptions<TOptions>(optionsBuilder.Name, s.GetRequiredService<IValidator<TOptions>>()));
            return optionsBuilder;
        }
    }

    public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        private readonly IValidator<TOptions> _validator;

        public FluentValidationOptions(string? name, IValidator<TOptions> validator)
        {
            Name = name;
            _validator = validator;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string? Name { get; }

        public ValidateOptionsResult Validate(string? name, TOptions options)
        {
            // Null name is used to configure all named options.
            if (Name != null && Name != name)
            {
                // Ignored if not validating this instance.
                return ValidateOptionsResult.Skip;
            }

            ArgumentNullException.ThrowIfNull(options);

            var validationResult = _validator.Validate(options);
            if (validationResult.IsValid)
            {
                return ValidateOptionsResult.Success;
            }

            var errors = validationResult.Errors.Select(x => $"Options validation failed for '{x.PropertyName}' with error: '{x.ErrorMessage}'");

            return ValidateOptionsResult.Fail(errors);
        }
    }
}
