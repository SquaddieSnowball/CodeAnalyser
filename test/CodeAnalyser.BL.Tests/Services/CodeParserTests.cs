using CodeAnalyser.BL.Models;
using CodeAnalyser.BL.Services;
using FluentAssertions;
using FluentAssertions.Execution;

namespace CodeAnalyser.BL.Tests.Services;

public class CodeParserTests
{
	[Fact]
	public void Parse_EmptyCodeString_ReturnsSuccess()
	{
		CodeParser parser = new();
		string code = string.Empty;

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeTrue();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEmpty();

			actual.Tail
				.Should().BeNull();
		}
	}

	[Fact]
	public void Parse_CodeWithNoApplicationIdentifiers_ReturnsFailure()
	{
		CodeParser parser = new();
		string code = "abc";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeFalse();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEmpty();

			actual.Tail
				.Should().BeEquivalentTo("abc");
		}
	}

	[Fact]
	public void Parse_CodeContainingApplicationIdentifierWithoutValue_ReturnsFailure()
	{
		CodeParser parser = new();
		string code = "00";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeFalse();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEmpty();

			actual.Tail
				.Should().BeEquivalentTo("00");
		}
	}

	[Fact]
	public void Parse_CodeContainingApplicationIdentifierWithShortenedValue_ReturnsSuccess()
	{
		CodeParser parser = new();
		string code = "00123";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeTrue();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123")
				]);

			actual.Tail
				.Should().BeNull();
		}
	}

	[Fact]
	public void Parse_CodeContainingApplicationIdentifierWithFullValue_ReturnsSuccess()
	{
		CodeParser parser = new();
		string code = "00123456789012345678";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeTrue();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123456789012345678")
				]);

			actual.Tail
				.Should().BeNull();
		}
	}

	[Fact]
	public void Parse_CodeContainingApplicationIdentifierWithFullValueAndTail_ReturnsFailure()
	{
		CodeParser parser = new();
		string code = "00123456789012345678abc";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeFalse();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123456789012345678")
				]);

			actual.Tail
				.Should().BeEquivalentTo("abc");
		}
	}

	[Fact]
	public void Parse_CodeContainingApplicationIdentifierWithVariableValueAndTail_ReturnsFailure()
	{
		CodeParser parser = new();
		string code = "211234567890\u001dabc";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeFalse();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567890")
				]);

			actual.Tail
				.Should().BeEquivalentTo("abc");
		}
	}

	[Fact]
	public void Parse_CodeContainingApplicationIdentifierWithFullValueAndAnotherApplicationIdentifier_ReturnsFailure()
	{
		CodeParser parser = new();
		string code = "0012345678901234567801";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeFalse();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123456789012345678")
				]);

			actual.Tail
				.Should().BeEquivalentTo("01");
		}
	}

	[Fact]
	public void Parse_CodeContainingApplicationIdentifierWithVariableValueAndAnotherApplicationIdentifier_ReturnsFailure()
	{
		CodeParser parser = new();
		string code = "211234567890\u001d00";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeFalse();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567890")
				]);

			actual.Tail
				.Should().BeEquivalentTo("00");
		}
	}

	[Fact]
	public void Parse_CodeContainingMultipleApplicationIdentifierValuePairsWithFullValue_ReturnsSuccess()
	{
		CodeParser parser = new();
		string code = "001234567890123456780112345678901234";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeTrue();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123456789012345678"),
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234")
				]);

			actual.Tail
				.Should().BeNull();
		}
	}

	[Fact]
	public void Parse_CodeContainingMultipleApplicationIdentifierValuePairsWithVariableValue_ReturnsSuccess()
	{
		CodeParser parser = new();
		string code = "211234567890\u001d00123456789012345678";

		CodeParsingResults actual = parser.Parse(code);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeTrue();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567890"),
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123456789012345678")
				]);

			actual.Tail
				.Should().BeNull();
		}
	}

	[Fact]
	public void Parse_InvalidCodeAgainstTemplate_ReturnsFailure()
	{
		CodeParser parser = new();
		string code = "12345678901234123456712341234abc";
		CodeParsingTemplate parsingTemplate = CodeParsingTemplate.RuTobaccoProducts;

		CodeParsingResults actual = parser.Parse(code, parsingTemplate);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeFalse();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234"),
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567"),
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI8005, "1234"),
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI93, "1234")
				]);

			actual.Tail
				.Should().BeEquivalentTo("abc");
		}
	}

	[Fact]
	public void Parse_ValidCodeAgainstTemplate_ReturnsSuccess()
	{
		CodeParser parser = new();
		string code = "12345678901234123456712341234";
		CodeParsingTemplate parsingTemplate = CodeParsingTemplate.RuTobaccoProducts;

		CodeParsingResults actual = parser.Parse(code, parsingTemplate);

		using (new AssertionScope())
		{
			actual.Success
				.Should().BeTrue();

			actual.ApplicationIdentifierValuePairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234"),
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567"),
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI8005, "1234"),
					new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI93, "1234")
				]);

			actual.Tail
				.Should().BeNull();
		}
	}
}