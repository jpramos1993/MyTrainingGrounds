
List<int> numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

var greater = numbers.WhereGreaterThan(5);
// Equivalent Extensions.WhereGreaterThan(numbers, 5);

// var newList = List<int>.Create();

Console.WriteLine(greater.Count());
// Console.WriteLine(greater.IsEmpty);

public static class Extensions
{
    public static IEnumerable<int> WhereGreaterThan(this IEnumerable<int> source, int threshold)
        => source.Where(x => x > threshold);
}

//public static class NewExtensions
//{
//    // Not static its an instance member
//    // Generic can be used
//    // INumber Allows Comparisons
//    extension<T>(IEnumerable<T> source) where T : INumber<T>
//    {
//        public bool IsEmpty => source.Any() is false;

//        public IEnumerable<int> WhereGreaterThan(int threshold)
//            => source.Where(x => x > threshold);
//    }
//
//extensions(List<int>)
//    {
//    public static List<int> Create() => [];
//}
//}