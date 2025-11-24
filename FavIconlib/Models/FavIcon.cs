namespace FavIconlib.Models
{
    public class FavIcon : IEquatable<FavIcon?>
    {
        public string href { get; set; } = "";
        public int sizes { get; set; }
        public string type { get; set; } = "";

        public override bool Equals(object? obj)
        {
            return Equals(obj as FavIcon);
        }

        public bool Equals(FavIcon? other)
        {
            return other is not null &&
                   href == other.href &&
                   sizes == other.sizes &&
                   type == other.type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(href, sizes, type);
        }
    }
}
