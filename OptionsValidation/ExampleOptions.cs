using System.ComponentModel.DataAnnotations;

namespace OptionsValidation
{
    public class ExampleOptions
    {
        public const string SectionName = "Example";

        [EnumDataType(typeof(LogLevel))]
        public required LogLevel LogLevel { get; init; }

        [Range(1, 9)]
        public required int Retries { get; init; }
    }
}
