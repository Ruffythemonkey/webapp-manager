namespace FavIconlib.Extensions
{
    internal static class IntExtensions
    {
        public static int SizeToInt(this string value) 
        {
            var ret = int.TryParse(value.Split("x").First(), out var val);
            return ret ? val : 0;
        }
    }
}
