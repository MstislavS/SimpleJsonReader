namespace JsonReader.Common
{
    /// <summary>
    /// Holds a necessary assertions for method arguments
    /// </summary>
    public static partial class Argument
    {
        /// <summary>
        /// Asserts that argument is not null, throws <see cref="ArgumentNullException"/>
        /// if it is
        /// </summary>
        /// <typeparam name="T">The argument type</typeparam>
        /// <param name="item">The argument</param>
        /// <param name="argumentName">The argument name</param>
        public static void NotNull<T>(T item, string argumentName)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(argumentName));
        }
    }
}