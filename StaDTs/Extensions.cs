namespace StaDTs
{
    public static class Extensions
    {
        public static string ToCamelCase(this string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return identifier;

            if (char.IsLower(identifier[0]))
                return identifier;

            return char.ToLowerInvariant(identifier[0]) + identifier.Substring(1);
        }
    }
}
