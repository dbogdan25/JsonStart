using Xunit;
using static Json.JsonString;

namespace Json.Facts
{
    public class JsonStringFacts
    {
        [Fact]
        public void IsWrappedInDoubleQuotes()
        {
            Assert.True(IsJsonString(Quoted("abc")));
        }

        [Fact]
        public void AlwaysStartsWithQuotes()
        {
            Assert.False(IsJsonString("abc\""));
        }

        [Fact]
        public void AlwaysEndsWithQuotes()
        {
            Assert.False(IsJsonString("\"abc"));
        }

        [Fact]
        public void IsNotNull()
        {
            Assert.False(IsJsonString(null));
        }

        [Fact]
        public void IsNotAnEmptyString()
        {
            Assert.False(IsJsonString(string.Empty));
        }

        [Fact]
        public void IsAnEmptyDoubleQuotedString()
        {
            Assert.True(IsJsonString(Quoted(string.Empty)));
        }

        [Fact]
        public void DoesNotContainControlCharacters()
        {
            Assert.False(IsJsonString(Quoted("a\nb\rc")));
        }

        [Fact]
        public void CanContainLargeUnicodeCharacters()
        {
            Assert.True(IsJsonString(Quoted("⛅⚾")));
        }

        [Fact]
        public void CanContainEscapedQuotationMark()
        {
            Assert.True(IsJsonString(Quoted(@"\""a\"" b")));
        }

        [Fact]
        public void CanContainEscapedReverseSolidus()
        {
            Assert.True(IsJsonString(Quoted(@"a \\ b")));
        }

        [Fact]
        public void CanContainEscapedSolidus()
        {
            Assert.True(IsJsonString(Quoted(@"a \/ b")));
        }

        [Fact]
        public void CanContainEscapedBackspace()
        {
            Assert.True(IsJsonString(Quoted(@"a \b b")));
        }

        [Fact]
        public void CanContainEscapedFormFeed()
        {
            string input = Quoted(@"a \f b");
            bool actual = IsJsonString(input);
            Assert.True(actual, $"Failed for {input}");
        }

        [Fact]
        public void CanContainEscapedLineFeed()
        {
            string input = Quoted(@"a \n b");
            bool actual = IsJsonString(input);
            Assert.True(actual, $"Failed for {input}");
        }

        [Fact]
        public void CanContainEscapedCarrigeReturn()
        {
            string input = Quoted(@"a \r b");
            bool actual = IsJsonString(input);
            Assert.True(actual, $"Failed for {input}");
        }

        [Fact]
        public void CanContainEscapedHorizontalTab()
        {
            string input = Quoted(@"a \t b");
            bool actual = IsJsonString(input);
            Assert.True(actual, $"Failed for {input}");
        }

        [Fact]
        public void CanContainEscapedUnicodeCharacters()
        {
            string input = Quoted(@"a \u26Be b");
            bool actual = IsJsonString(input);
            Assert.True(actual, $"Failed for {input}");
        }

        [Fact]
        public void CanContainAnyMultipleEscapeSequences()
        {
            string input = Quoted(@"\\\u1212\n\t\r\\\b");
            bool actual = IsJsonString(input);
            Assert.True(actual, $"Failed for {input}");
        }

        [Fact]
        public void DoesNotContainUnrecognizedExcapceCharacters()
        {
            Assert.False(IsJsonString(Quoted(@"a\x")));
        }

        [Fact]
        public void DoesNotEndWithReverseSolidus()
        {
            Assert.False(IsJsonString(Quoted(@"a\")));
        }

        [Fact]
        public void DoesNotEndWithAnUnfinishedHexNumber()
        {
            Assert.False(IsJsonString(Quoted(@"a\u")));
            Assert.False(IsJsonString(Quoted(@"a\u123")));
        }

        public static string Quoted(string text)
            => $"\"{text}\"";
    }
}
