using CodeAnalyser.BL.Data;
using CodeAnalyser.BL.Models;
using CodeAnalyser.BL.Primitives;
using CodeAnalyser.BL.Services;
using FluentAssertions;
using FluentAssertions.Execution;

namespace CodeAnalyser.BL.Tests.Services;

public class CodeVerifierTests
{
	[Fact]
	public void Verify_EmptyApplicationIdentifierValuePairs_ReturnsFailure()
	{
		CodeVerifier verifier = new();
		CodeParsingResults parsingResults = new(
			string.Empty,
			true,
			[]);
		CodeValidationResults validationResults = new(
			parsingResults,
			true,
			[]);

		CodeVerificationResults actual = verifier.Verify(
			validationResults,
			CodeVerificationTemplate.RuTobaccoProducts,
			false);

		using (new AssertionScope())
		{
			actual.Approved
				.Should().BeFalse();

			actual.Errors
				.Should().BeEquivalentTo(
				[
					CodeAnalyserErrors.Code_InvalidLength
				]);

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI01,
						[
							CodeAnalyserErrors.Code_MissingAI
						]),
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI21,
						[
							CodeAnalyserErrors.Code_MissingAI
						]),
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI8005,
						[
							CodeAnalyserErrors.Code_MissingAI
						]),
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI93,
						[
							CodeAnalyserErrors.Code_MissingAI
						])
				]);
		}
	}

	[Fact]
	public void Verify_MissingApplicationIdentifierValuePairs_ReturnsFailure()
	{
		CodeVerifier verifier = new();
		CodeParsingResults parsingResults = new(
			"12345678901234",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234")
			]);
		CodeValidationResults validationResults = new(
			parsingResults,
			true,
			[]);

		CodeVerificationResults actual = verifier.Verify(
			validationResults,
			CodeVerificationTemplate.RuTobaccoProducts,
			false);

		using (new AssertionScope())
		{
			actual.Approved
				.Should().BeFalse();

			actual.Errors
				.Should().BeEquivalentTo(
				[
					CodeAnalyserErrors.Code_InvalidLength
				]);

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI21,
						[
							CodeAnalyserErrors.Code_MissingAI
						]),
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI8005,
						[
							CodeAnalyserErrors.Code_MissingAI
						]),
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI93,
						[
							CodeAnalyserErrors.Code_MissingAI
						])
				]);
		}
	}

	[Fact]
	public void Verify_WrongAndMissingApplicationIdentifierValuePairs_ReturnsFailure()
	{
		CodeVerifier verifier = new();
		CodeParsingResults parsingResults = new(
			"00123456789012345678",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI00, "123456789012345678")
			]);
		CodeValidationResults validationResults = new(
			parsingResults,
			true,
			[]);

		CodeVerificationResults actual = verifier.Verify(
			validationResults,
			CodeVerificationTemplate.RuTobaccoProductsGroupPack,
			true);

		using (new AssertionScope())
		{
			actual.Approved
				.Should().BeFalse();

			actual.Errors
				.Should().BeEquivalentTo(
				[
					CodeAnalyserErrors.Code_InvalidLength,
					CodeAnalyserErrors.Code_InvalidStructure
				]);

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI01,
						[
							CodeAnalyserErrors.Code_MissingAI
						]),
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI21,
						[
							CodeAnalyserErrors.Code_MissingAI
						]),
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI8005,
						[
							CodeAnalyserErrors.Code_MissingAI
						]),
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI93,
						[
							CodeAnalyserErrors.Code_MissingAI
						])
				]);
		}
	}

	[Fact]
	public void Verify_WrongApplicationIdentifierValuePairsOrder_ReturnsFailure()
	{
		CodeVerifier verifier = new();
		CodeParsingResults parsingResults = new(
			"21123456780051234569312340112345678901234",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI8005, "123456"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI93, "1234"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234")
			]);
		CodeValidationResults validationResults = new(
			parsingResults,
			true,
			[]);

		CodeVerificationResults actual = verifier.Verify(
			validationResults,
			CodeVerificationTemplate.RuTobaccoProductsGroupPack,
			true);

		using (new AssertionScope())
		{
			actual.Approved
				.Should().BeFalse();

			actual.Errors
				.Should().BeEquivalentTo(
				[
					CodeAnalyserErrors.Code_InvalidStructure
				]);

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEmpty();
		}
	}

	[Fact]
	public void Verify_CodeHasFNC1_ReturnsFailure()
	{
		CodeVerifier verifier = new();
		CodeParsingResults parsingResults = new(
			"12345678901234123456712341234",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI8005, "1234"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI93, "1234")
			]);
		CodeValidationResults validationResults = new(
			parsingResults,
			true,
			[]);

		CodeVerificationResults actual = verifier.Verify(
			validationResults,
			CodeVerificationTemplate.RuTobaccoProducts,
			true);

		using (new AssertionScope())
		{
			actual.Approved
				.Should().BeFalse();

			actual.Errors
				.Should().BeEquivalentTo(
				[
					CodeAnalyserErrors.Code_HasFNC1
				]);

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEmpty();
		}
	}

	[Fact]
	public void Verify_CodeMissingFNC1_ReturnsFailure()
	{
		CodeVerifier verifier = new();
		CodeParsingResults parsingResults = new(
			"01123456789012342112345678005123456931234",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI8005, "123456"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI93, "1234")
			]);
		CodeValidationResults validationResults = new(
			parsingResults,
			true,
			[]);

		CodeVerificationResults actual = verifier.Verify(
			validationResults,
			CodeVerificationTemplate.RuTobaccoProductsGroupPack,
			false);

		using (new AssertionScope())
		{
			actual.Approved
				.Should().BeFalse();

			actual.Errors
				.Should().BeEquivalentTo(
				[
					CodeAnalyserErrors.Code_MissingFNC1
				]);

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEmpty();
		}
	}

	[Fact]
	public void Verify_CodeInvalidApplicationIdentifierValueLength_ReturnsFailure()
	{
		CodeVerifier verifier = new();
		CodeParsingResults parsingResults = new(
			"011234567890123421123456780051234569312345",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI8005, "123456"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI93, "12345")
			]);
		CodeValidationResults validationResults = new(
			parsingResults,
			true,
			[]);

		CodeVerificationResults actual = verifier.Verify(
			validationResults,
			CodeVerificationTemplate.RuTobaccoProductsGroupPack,
			true);

		using (new AssertionScope())
		{
			actual.Approved
				.Should().BeFalse();

			actual.Errors
				.Should().BeEmpty();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEquivalentTo(
				[
					new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(
						ApplicationIdentifier.AI93,
						[
							CodeAnalyserErrors.AI_InvalidLength
						])
				]);
		}
	}

	[Fact]
	public void Verify_ValidCode_ReturnsSuccess()
	{
		CodeVerifier verifier = new();
		CodeParsingResults parsingResults = new(
			"12345678901234123456712341234",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI8005, "1234"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI93, "1234")
			]);
		CodeValidationResults validationResults = new(
			parsingResults,
			true,
			[]);

		CodeVerificationResults actual = verifier.Verify(
			validationResults,
			CodeVerificationTemplate.RuTobaccoProducts,
			false);

		using (new AssertionScope())
		{
			actual.Approved
				.Should().BeTrue();

			actual.Errors
				.Should().BeEmpty();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEmpty();
		}
	}

	[Fact]
	public void Verify_ValidCodeWithAlternative_ReturnsSuccess()
	{
		CodeVerifier verifier = new();
		CodeParsingResults parsingResults = new(
			"01123456789012342112345679312343351123456",
			true,
			[
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI01, "12345678901234"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI21, "1234567"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI93, "1234"),
				new KeyValuePair<ApplicationIdentifier, string>(ApplicationIdentifier.AI3351, "123456")
			]);
		CodeValidationResults validationResults = new(
			parsingResults,
			true,
			[]);

		CodeVerificationResults actual = verifier.Verify(
			validationResults,
			CodeVerificationTemplate.RuBeerProductsWithVolume,
			true);

		using (new AssertionScope())
		{
			actual.Approved
				.Should().BeTrue();

			actual.Errors
				.Should().BeEmpty();

			actual.ApplicationIdentifierErrorsPairs
				.Should().BeEmpty();
		}
	}
}