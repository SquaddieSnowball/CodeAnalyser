using CodeAnalyser.BL.Data;
using CodeAnalyser.BL.Models;
using CodeAnalyser.BL.Primitives;
using CodeAnalyser.BL.Services;
using FluentAssertions;
using FluentAssertions.Execution;

namespace CodeAnalyser.BL.Tests.Services;

public class CodeValidatorTests
{
	[Fact]
	public void Validate_EmptyApplicationIdentifierValuePairs_ReturnsSuccess()
	{
		CodeValidator validator = new();
		CodeParsingResults parsingResults = new(
			string.Empty,
			true,
			[]);

		CodeValidationResults actual = validator.Validate(parsingResults);

		using (new AssertionScope())
		{
			actual.Valid
				.Should().BeTrue();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEmpty();
		}
	}

	[Fact]
	public void Validate_ApplicationIdentifierWithInvalidValueLength_ReturnsFailure()
	{
		CodeValidator validator = new();
		CodeParsingResults parsingResults = new(
			"00123",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123")
			]);

		CodeValidationResults actual = validator.Validate(parsingResults);

		using (new AssertionScope())
		{
			actual.Valid
				.Should().BeFalse();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI00,
						[
							CodeAnalyserErrors.AI_InvalidLength
						])
				]);
		}
	}

	[Fact]
	public void Validate_ApplicationIdentifierWithValueContainingLettersNotAllowed_ReturnsFailure()
	{
		CodeValidator validator = new();
		CodeParsingResults parsingResults = new(
			"001234567890abcdefgh",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "1234567890abcdefgh")
			]);

		CodeValidationResults actual = validator.Validate(parsingResults);

		using (new AssertionScope())
		{
			actual.Valid
				.Should().BeFalse();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI00,
						[
							CodeAnalyserErrors.AI_LettersNotAllowed
						])
				]);
		}
	}

	[Fact]
	public void Validate_ApplicationIdentifierWithValueContainingSpecialCharsNotAllowed_ReturnsFailure()
	{
		CodeValidator validator = new();
		CodeParsingResults parsingResults = new(
			"0012345678901234567!",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "12345678901234567!")
			]);

		CodeValidationResults actual = validator.Validate(parsingResults);

		using (new AssertionScope())
		{
			actual.Valid
				.Should().BeFalse();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI00,
						[
							CodeAnalyserErrors.AI_SpecialCharsNotAllowed
						])
				]);
		}
	}

	[Fact]
	public void Validate_ApplicationIdentifierWithMultipleViolationsInValue_ReturnsFailure()
	{
		CodeValidator validator = new();
		CodeParsingResults parsingResults = new(
			"00123456789abcd,.?!",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123456789abcd,.?!")
			]);

		CodeValidationResults actual = validator.Validate(parsingResults);

		using (new AssertionScope())
		{
			actual.Valid
				.Should().BeFalse();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI00,
						[
							CodeAnalyserErrors.AI_InvalidLength,
							CodeAnalyserErrors.AI_LettersNotAllowed,
							CodeAnalyserErrors.AI_SpecialCharsNotAllowed
						])
				]);
		}
	}

	[Fact]
	public void Validate_ApplicationIdentifierWithValidValue_ReturnsSuccess()
	{
		CodeValidator validator = new();
		CodeParsingResults parsingResults = new(
			"00123456789012345678",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123456789012345678")
			]);

		CodeValidationResults actual = validator.Validate(parsingResults);

		using (new AssertionScope())
		{
			actual.Valid
				.Should().BeTrue();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEmpty();
		}
	}

	[Fact]
	public void Validate_MultipleInvalidApplicationIdentifierValuePairs_ReturnsFailure()
	{
		CodeValidator validator = new();
		CodeParsingResults parsingResults = new(
			"00123011234567890abcd",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "1234567890abcd")
			]);

		CodeValidationResults actual = validator.Validate(parsingResults);

		using (new AssertionScope())
		{
			actual.Valid
				.Should().BeFalse();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI00,
						[
							CodeAnalyserErrors.AI_InvalidLength
						]),

					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI01,
						[
							CodeAnalyserErrors.AI_LettersNotAllowed
						])
				]);
		}
	}

	[Fact]
	public void Validate_MultipleValidApplicationIdentifierValuePairs_ReturnsSuccess()
	{
		CodeValidator validator = new();
		CodeParsingResults parsingResults = new(
			"001234567890123456780112345678901234",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123456789012345678"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234")
			]);

		CodeValidationResults actual = validator.Validate(parsingResults);

		using (new AssertionScope())
		{
			actual.Valid
				.Should().BeTrue();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEmpty();
		}
	}
}