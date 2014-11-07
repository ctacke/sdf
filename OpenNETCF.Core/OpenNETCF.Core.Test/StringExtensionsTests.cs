namespace OpenNETCF.Core.Test
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using OpenNETCF.Testing.Support.SmartDevice;
  using OpenNETCF;

  [TestClass]
  public class StringExtensionsTests : TestBase
  {
    [TestMethod]
    public void TestWhenNull()
    {
      string mock = null;
      Assert.IsTrue(mock.IsNullOrEmpty());
    }

    [TestMethod]
    public void TestWhenEmpty()
    {
      string mock = String.Empty;
      Assert.IsTrue(mock.IsNullOrEmpty());
    }

    [TestMethod]
    public void TestWhenNotNullOrEmpty()
    {
      string mock = "Hello World";
      Assert.IsFalse(mock.IsNullOrEmpty());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestPlurarlizeThrowsOnNull()
    {
      string mock = null;
      mock.Pluralize();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestPlurarlizeThrowsOnEmpty()
    {
      string mock = String.Empty;
      mock.Pluralize();
    }

    [TestMethod]
    public void TestPluralize()
    {
      string mock = "apple";
      Assert.IsTrue(mock.Pluralize().EndsWith("s"));
    }

    [TestMethod]
    public void TestPluralizeTransformation()
    {
      string mock = "Spy";
      Assert.IsTrue(mock.Pluralize().EndsWith("ies"));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestCapitalizeWordsThrowsOnNull()
    {
      string mock = null;
      mock.CapitalizeWords();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestCapitalizeWordsThrowsOnEmpty()
    {
      string mock = String.Empty;
      mock.CapitalizeWords();
    }

    [TestMethod]
    public void TestCapitalizeWords()
    {
      string mock = "today is the day";
      string result = mock.CapitalizeWords();

      foreach (string word in result.Split(' '))
      {
        Assert.IsTrue(char.IsUpper(word[0]));
      }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestCapitalizeThrowsOnNull()
    {
      string mock = null;
      mock.CapitalizeWords();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestCapitalizeThrowsOnEmpty()
    {
      string mock = String.Empty;
      mock.CapitalizeWords();
    }

    [TestMethod]
    public void TestCapitalizeSingleWord()
    {
      string mock = "today";
      string result = mock.Capitalize();
      Assert.IsTrue(char.IsUpper(result[0]));
    }

    [TestMethod]
    public void TestCapitalizeSentence()
    {
      string mock = "today is the day";
      string result = mock.Capitalize();
      string[] words = result.Split(' ');

      Assert.IsTrue(Char.IsUpper(words[0][0]));
      Assert.IsTrue(Char.IsLower(words[1][0]));
      Assert.IsTrue(Char.IsLower(words[2][0]));
      Assert.IsTrue(Char.IsLower(words[3][0]));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestForEach()
    {

      string mock = "abc1";
      // Will throw on the last character
      mock.ForEach(ThrowOnNonLetter);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestIsMatchThrowsOnNullPattern()
    {
      string mock = "Hello";
      mock.IsMatch(null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestIsMatchThrowsOnEmptyPattern()
    {
      string mock = "Hello";
      mock.IsMatch(String.Empty);
    }

    [TestMethod]
    public void TestIsMatchReturnsTrue()
    {
      string mock = "2008";
      Assert.IsTrue(mock.IsMatch(@"\d{4}"));
    }

    [TestMethod]
    public void TestIsMatchReturnsFalse()
    {
      string mock = "08";
      Assert.IsFalse(mock.IsMatch(@"\d{4}"));
    }

    private static void ThrowOnNonLetter(char c)
    {
      if (!Char.IsLetter(c))
      {
        throw new ArgumentException("c must be a letter.");
      }
    }

    [TestMethod]
    [ExpectedException(typeof(System.ArgumentException))]
    public void LuhnCheckTestNonDigits()
    {
      string number = "4000-0000-0000-0000";
      Assert.IsFalse(number.LuhnCheck());
    }

    [TestMethod]
    [ExpectedException(typeof(System.ArgumentException))]
    public void LuhnCheckTestNullInput()
    {
      string number = null;
      Assert.IsFalse(number.LuhnCheck());
    }

    [TestMethod]
    [ExpectedException(typeof(System.ArgumentException))]
    public void LuhnCheckTestEmptyInput()
    {
      string number = string.Empty;
      Assert.IsFalse(number.LuhnCheck());
    }

    [TestMethod]
    [ExpectedException(typeof(System.ArgumentException))]
    public void LuhnCheckTestTooLongInput()
    {
      string number = new string('0', 17);
      Assert.IsFalse(number.LuhnCheck());
    }

    [TestMethod]
    public void LuhnCheckTestKnownInvalidNumber()
    {
      string number = "5142987609806756";
      Assert.IsFalse(number.LuhnCheck());
    }

    [TestMethod]
    public void NormalizeNewlineTestDefault()
    {
      string input = "A\rB\nC\r\nD\n\rE\n\nF\r\rG\r\n\nH";
      string expected = "A\r\nB\r\nC\r\nD\r\nE\r\n\r\nF\r\n\r\nG\r\n\r\nH";
      string output = input.NormalizeNewlines();
      Assert.AreEqual(expected, output);
    }

    [TestMethod]
    public void NormalizeNewlineTestParamCrLf()
    {
      string input = "A\rB\nC\r\nD\n\rE\n\nF\r\rG\r\n\nH";
      string expected = "A\r\nB\r\nC\r\nD\r\nE\r\n\r\nF\r\n\r\nG\r\n\r\nH";
      string output = input.NormalizeNewlines("\r\n");
      Assert.AreEqual(expected, output);
    }

    [TestMethod]
    public void NormalizeNewlineTestParamCr()
    {
      string input = "A\rB\nC\r\nD\n\rE\n\nF\r\rG\r\n\nH";
      string expected = "A\rB\rC\rD\rE\r\rF\r\rG\r\rH";
      string output = input.NormalizeNewlines("\r");
      Assert.AreEqual(expected, output);
    }

    [TestMethod]
    public void NormalizeNewlineTestParamCrNegative()
    {
      string input = "A\rB\nC\r\nD\n\rE\n\nF\r\rG\r\n\nH";
      string expected = "A\rB\rC\rD\rE\r\rF\r\rG\r\rH";
      string output = input.NormalizeNewlines("\n");
      Assert.AreNotEqual(expected, output);
    }
  }
}
