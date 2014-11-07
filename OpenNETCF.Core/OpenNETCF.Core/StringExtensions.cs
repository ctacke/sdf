namespace OpenNETCF
{
  using System;
  using System.Text.RegularExpressions;
  using System.Collections.Generic;

  public static partial class Extensions
  {
    /// <summary>
    /// Determines if the string is null or empty. 
    /// </summary>
    /// <returns>True: if null or empty, otherwise False.</returns>
    public static bool IsNullOrEmpty(this string s)
    {
      return String.IsNullOrEmpty(s);
    }

    /// <summary>
    /// Pluralizes the last word in the string. 
    /// Transforms "apple" into "apples" and "spy" into "spies".
    /// </summary>
    /// <returns></returns>
    public static string Pluralize(this string s)
    {
      if (String.IsNullOrEmpty(s))
      {
        throw new ArgumentNullException("s");
      }

      if (s[s.Length - 1] == 's')
      {
        return s;
      }

      return s[s.Length - 1] == 'y' ? String.Format("{0}ies", s.Substring(0, s.Length - 1)) : String.Format("{0}s", s);
    }

    /// <summary>
    /// Transforms the casing of the first character 
    /// of each word in the string to upper-case.
    /// </summary>
    /// <returns>A title-cased string.</returns>
    public static string CapitalizeWords(this string s)
    {
      if (String.IsNullOrEmpty(s))
      {
        throw new ArgumentNullException("s");
      }

      char[] array = s.ToCharArray();
      array[0] = char.ToUpper(array[0]);

      for (int i = 1; i < array.Length; i++)
      {
        if (Char.IsWhiteSpace(array[i - 1]))
        {
          array[i] = Char.ToUpper(array[i]);
        }
      }

      return new string(array);
    }

    /// <summary>
    /// Transforms the casing of the first character to upper-case.
    /// </summary>
    /// <returns></returns>
    public static string Capitalize(this string s)
    {
      if (String.IsNullOrEmpty(s))
      {
        throw new ArgumentNullException("s");
      }

      char[] array = s.ToCharArray();
      array[0] = char.ToUpper(array[0]);

      return new string(array);
    }

    /// <summary>
    /// Performs the specified Action for each character 
    /// in the string.
    /// </summary>
    /// <param name="action">The Action to perform for each character in the string.</param>
    public static void ForEach(this string s, Action<Char> action)
    {
      foreach (char c in s)
      {
        action(c);
      }
    }

    /// <summary>
    /// Determines if a regular expression pattern matches the specified string.
    /// </summary>
    /// <param name="pattern">The regular expression to match against the string.</param>
    /// <returns></returns>
    public static bool IsMatch(this string s, string pattern)
    {
      if (String.IsNullOrEmpty(pattern))
      {
        throw new ArgumentNullException("pattern");
      }

      return Regex.IsMatch(s, pattern);
    }

    /// <summary>
    /// Luhn Checks a credit card number to see if it's valid.
    /// </summary>
    /// <remarks>
    /// This method does not verify that the number is an actual valid credit card, it simply verifies the number itself it self is valid.
    /// </remarks>
    /// <param name="creditCardNumber">The credit card number, in all digits (no dashes or spaces)</param>
    /// <returns><b>true</b> if the number is valid, else <b>false</b></returns>
    public static bool LuhnCheck(this string creditCardNumber)
    {
      if (string.IsNullOrEmpty(creditCardNumber)) throw new ArgumentException("creditCardNumber cannot be empty");
      if (creditCardNumber.Length > 16) throw new ArgumentException("creditCardNumber cannot exceed 16 digits");

      List<byte> digits = new List<byte>();

      foreach (char c in creditCardNumber)
      {
        if (!c.IsDigit()) throw new ArgumentException("creditCardNumber charactes must all be digits");

        digits.Add(byte.Parse(c.ToString()));
      }

      int sum = 0;

      for (int i = 0; i < digits.Count; i++)
      {
        if (i % 2 == 0)
        { // odd chars
          int n = digits[i] * 2;
          sum += (n / 10) + (n % 10);
        }
        else
        { // even chars
          sum += digits[i];
        }
      }

      return ((sum % 10) == 0);
    }

    /// <summary>
    /// Normalizes all "new lines" to the "\r\n" form
    /// </summary>
    /// <param name="input">string to normalize</param>
    /// <returns>The normalized string</returns>
    public static string NormalizeNewlines(this string input)
    {
      return System.Text.RegularExpressions.Regex.Replace(input, @"\r\n|\n\r|\n|\r", "\r\n");
    }

    /// <summary>
    /// Normalizes all "new lines" to the provided form
    /// </summary>
    /// <param name="input">string to normalize</param>
    /// <param name="newline">The newline form of choice (e.g. "\r\n")</param>
    /// <returns>The normalized string</returns>
    public static string NormalizeNewlines(this string input, string newline)
    {
      return System.Text.RegularExpressions.Regex.Replace(input, @"\r\n|\n\r|\n|\r", newline);
    }
  }
}
