using FluentValidation;

namespace OptionsValidation
{
    public class ExampleOptionsValidator : AbstractValidator<ExampleOptions>
    {
        public ExampleOptionsValidator() 
        {
            //RuleFor(x => x.LogLevel).IsEnumName(typeof(LogLevel));

            RuleFor(x => x.Retries).InclusiveBetween(1, 9);
        }
    }
}
