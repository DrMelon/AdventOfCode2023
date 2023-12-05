namespace AOC2023.Common;

public static class CharHelpers
{
    public static bool IsDigit(char testChar)
    {
        return 48 <= testChar && testChar <= 57;
    }
}