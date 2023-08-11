namespace JsonReader.Common
{
    public static partial class Argument
    {
        public static void NotNull<T>(T item, string argumentName)
        {
            if (item == null)
                throw new ArgumentNullException(argumentName);
        }
    }
}