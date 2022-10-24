namespace SplitExpense.Core.Services.Extensions
{
    public static class IEnumerableExtensions
    {
        public static string ToCSV<T>(this IEnumerable<T> list)
        {
            return string.Join(",", list);
        }
    }
}
