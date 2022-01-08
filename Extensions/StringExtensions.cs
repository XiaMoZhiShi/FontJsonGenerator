namespace FontJsonGenerator.Extensions;

public static class StringExtensions
{
    public static string ToResourcePath(this string rawPath)
    {
        string[] spiltResult = rawPath.Split(':');
        char separator = Path.DirectorySeparatorChar;

        if (spiltResult.Length != 2)
            throw new ArrayLengthNotMatchException($"{spiltResult}的长度不为2");

        return $"{separator}assets{separator}" + spiltResult[0] + $"{separator}textures{separator}" + spiltResult[1];
    }

    public class ArrayLengthNotMatchException : Exception
    {
        public ArrayLengthNotMatchException(string message)
            : base(message)
        {
        }
    }
}