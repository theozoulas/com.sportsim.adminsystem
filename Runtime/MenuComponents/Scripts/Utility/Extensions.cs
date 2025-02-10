namespace MenuComponents.Utility
{
    /// <summary>
    /// Extenstion Class <c>Extensions</c>
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Static Get Method <c>WriteCsvLine</c> Writes string as a CSV Line.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string WriteCsvLine(this string text) => '"' + text + '"' + ',';
    }
}
