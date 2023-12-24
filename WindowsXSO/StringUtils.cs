namespace WindowsXSO;

public static class StringUtils {
    /// <summary>
    /// Checks if the string contains multiple values
    /// </summary>
    /// <param name="str1">this</param>
    /// <param name="strs">As many strings as you want to compare to the target string</param>
    /// <returns>Boolean indicating that any and all of your specified strings are contained in the target string (this)</returns>
    public static bool ContainsMultiple(this string str1, params string[] strs) => strs.Any(str1.Contains);
}