using CodeAnalyser.BL.Data;
using CodeAnalyser.BL.Models;
using CodeAnalyser.BL.Primitives;
using CodeAnalyser.BL.Services.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace CodeAnalyser.BL.Services;

public class CodeValidator : ICodeValidator
{
	private record struct ApplicationIdentifierConstraintsTracker(
		bool DigitsTracked,
		bool LettersTracked,
		bool SpecialCharsTracked);

	private static readonly HashSet<char> s_allowedSpecialChars =
		[
			'!','"', '%', '&', '(', ')', '*', '+', ',', '-', '.', '/', '_', ':', ';', '=', '<', '>', '?', '\''
		];

	private static Dictionary<ApplicationIdentifier, ApplicationIdentifierConstraints> s_applicationIdentifierConstraintsPairs;

	static CodeValidator() => InitializeApplicationIdentifierConstraintsPairs();

	public CodeValidationResults Validate(CodeParsingResults parsingResults)
	{
		List<KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>> aiErrorsPairs = [];

		foreach (KeyValuePair<ApplicationIdentifier, string> aiValuePair in parsingResults.ApplicationIdentifierValuePairs)
		{
			ApplicationIdentifier ai = aiValuePair.Key;
			ApplicationIdentifierConstraints aiConstraints;

			try
			{
				aiConstraints = s_applicationIdentifierConstraintsPairs[ai];
			}
			catch (KeyNotFoundException)
			{
				throw new NotImplementedException(
					"Ограничения для указанного идентификатора применения еще не определены");
			}

			string value = aiValuePair.Value;
			List<Error> errors = [];

			if ((value.Length < aiConstraints.MinLength) || (value.Length > aiConstraints.MaxLength))
				errors.Add(CodeAnalyserErrors.AI_InvalidLength);

			ApplicationIdentifierConstraintsTracker aiConstraintsTracker = new();

			foreach (char currentChar in value)
			{
				if (char.IsDigit(currentChar))
				{
					aiConstraintsTracker = ValidateDigit(aiConstraints, aiConstraintsTracker, errors);

					continue;
				}

				if (char.IsLetter(currentChar))
				{
					aiConstraintsTracker = ValidateLetter(aiConstraints, aiConstraintsTracker, errors);

					continue;
				}

				if (aiConstraints.AllowSpecialChars && s_allowedSpecialChars.Contains(currentChar))
					continue;

				aiConstraintsTracker = TrackSpecialChars(aiConstraintsTracker, errors);
			}

			if (errors.Count is not 0)
				aiErrorsPairs.Add(new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(ai, errors));
		}

		return new CodeValidationResults(
			parsingResults,
			parsingResults.Success && (aiErrorsPairs.Count is 0),
			aiErrorsPairs);
	}

	private static ApplicationIdentifierConstraintsTracker ValidateDigit(
		ApplicationIdentifierConstraints aiConstraints,
		ApplicationIdentifierConstraintsTracker aiConstraintsTracker,
		List<Error> errors)
	{
		if (!aiConstraints.AllowDigits && !aiConstraintsTracker.DigitsTracked)
		{
			errors.Add(CodeAnalyserErrors.AI_DigitsNotAllowed);
			aiConstraintsTracker.DigitsTracked = true;
		}

		return aiConstraintsTracker;
	}

	private static ApplicationIdentifierConstraintsTracker ValidateLetter(
		ApplicationIdentifierConstraints aiConstraints,
		ApplicationIdentifierConstraintsTracker aiConstraintsTracker,
		List<Error> errors)
	{
		if (!aiConstraints.AllowLetters && !aiConstraintsTracker.LettersTracked)
		{
			errors.Add(CodeAnalyserErrors.AI_LettersNotAllowed);
			aiConstraintsTracker.LettersTracked = true;
		}

		return aiConstraintsTracker;
	}

	private static ApplicationIdentifierConstraintsTracker TrackSpecialChars(
		ApplicationIdentifierConstraintsTracker aiConstraintsTracker,
		List<Error> errors)
	{
		if (!aiConstraintsTracker.SpecialCharsTracked)
		{
			errors.Add(CodeAnalyserErrors.AI_SpecialCharsNotAllowed);
			aiConstraintsTracker.SpecialCharsTracked = true;
		}

		return aiConstraintsTracker;
	}

	[MemberNotNull(nameof(s_applicationIdentifierConstraintsPairs))]
	private static void InitializeApplicationIdentifierConstraintsPairs() => s_applicationIdentifierConstraintsPairs =
		CodeAnalyserData.ApplicationIdentifierDescriptors
			.Select(d => new KeyValuePair<ApplicationIdentifier, ApplicationIdentifierConstraints>(
				d.Identifier,
				d.Constraints))
			.ToDictionary();
}