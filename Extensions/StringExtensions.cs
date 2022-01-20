namespace FontJsonGenerator.Extensions;

public static class StringExtensions
{
    public static string ToFilePath(this string rawPath)
    {
        string[] spiltResult = rawPath.Split(':');
        char separator = Path.DirectorySeparatorChar;

        if (spiltResult.Length != 2)
            throw new ArrayLengthNotMatchException($"{spiltResult}的长度不为2，无法处理{rawPath}");

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