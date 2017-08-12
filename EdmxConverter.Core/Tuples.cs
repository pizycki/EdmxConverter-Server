namespace EdmxConverter.Core
{
    public static class Tuples
    {
        /// <summary>
        /// Uses Equal implementation of <typeparam name="T1">first argument</typeparam> to compare equality of both objects.
        /// </summary>
        public static bool AreValuesEqual<T1, T2>((T1, T2) t)
        {
            return t.Item1.Equals(t.Item2);
        }
    }
}
