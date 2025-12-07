namespace FavIconlib.Extensions
{
    internal static class IntExtensions
    {
        public static uint SizeStringToInt(this string value) 
            => uint.TryParse(value.Split("x").First(), out var val) ? val : uint.MinValue;
           
    }
}
