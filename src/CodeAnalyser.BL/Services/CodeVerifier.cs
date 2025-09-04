using CodeAnalyser.BL.Data;
using CodeAnalyser.BL.Models;
using CodeAnalyser.BL.Primitives;
using CodeAnalyser.BL.Services.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace CodeAnalyser.BL.Services;

public class CodeVerifier : ICodeVerifier
{
	private static Dictionary<CodeVerificationTemplate, CodeVerificationTemplateDescriptor> s_codeVerificationTemplateDescriptorPairs;

	static CodeVerifier() => InitializeCodeVerificationTemplateDescriptorPairs();

	public CodeVerificationResults Verify(
		CodeValidationResults validationResults,
		CodeVerificationTemplate verificationTemplate,
		bool? hasFNC1)
	{
		CodeVerificationTemplateDescriptor templateDescriptor;

		try
		{
			templateDescriptor = s_codeVerificationTemplateDescriptorPairs[verificationTemplate];
		}
		catch (KeyNotFoundException)
		{
			throw new NotImplementedException(
				"Дескриптор для указанного шаблона еще не определен");
		}

		List<Error> errors = [];

		VerifyFNC1(templateDescriptor, hasFNC1, errors);

		ApplicationIdentifier[] desiredAiOrder =
			[.. templateDescriptor.CodeUnits.Select(u => u.ApplicationIdentifier)];

		IEnumerable<ApplicationIdentifier>?[] desiredAiAlternativesOrder =
			[.. templateDescriptor.CodeUnits.Select(u => u.Alternatives?.Select(au => au.ApplicationIdentifier))];

		ApplicationIdentifier[] actualAiOrder =
			[.. validationResults.ParsingResults.ApplicationIdentifierValuePairs.Select(p => p.Key)];

		int aisLength = VerifyCodeLength(desiredAiOrder.Length, actualAiOrder.Length, errors);
		VerifyCodeStructure(aisLength, desiredAiOrder, desiredAiAlternativesOrder, actualAiOrder, errors);

		Dictionary<ApplicationIdentifier, int> aiLengthPairs = GenerateAILengthPairs(templateDescriptor, desiredAiOrder);
		List<KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>> aiErrorsPairs = [];

		foreach (ApplicationIdentifier ai in desiredAiOrder)
		{
			List<Error> aiErrors = [];

			if (!actualAiOrder.Contains(ai))
				aiErrors.Add(CodeAnalyserErrors.Code_MissingAI);
			else if (aiLengthPairs.TryGetValue(ai, out int requiredLength))
				VerifyAILength(validationResults, ai, requiredLength, aiErrors);

			if (aiErrors.Count is not 0)
				aiErrorsPairs.Add(new KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>(ai, aiErrors));
		}

		return new CodeVerificationResults(
			validationResults,
			validationResults.Valid && (errors.Count is 0) && (aiErrorsPairs.Count is 0),
			errors,
			aiErrorsPairs);
	}

	private static void VerifyFNC1(
		CodeVerificationTemplateDescriptor templateDescriptor,
		bool? hasFNC1,
		List<Error> errors)
	{
		if (hasFNC1 is null)
			errors.Add(CodeAnalyserErrors.Code_UnknownFNC1);
		else if ((bool)hasFNC1 && !templateDescriptor.MustHaveFNC1)
			errors.Add(CodeAnalyserErrors.Code_HasFNC1);
		else if (!(bool)hasFNC1 && templateDescriptor.MustHaveFNC1)
			errors.Add(CodeAnalyserErrors.Code_MissingFNC1);
	}

	private static int VerifyCodeLength(
		int desiredAisLength,
		int actualAisLength,
		List<Error> errors)
	{
		int aisLength;

		if (actualAisLength == desiredAisLength)
			aisLength = actualAisLength;
		else
		{
			errors.Add(CodeAnalyserErrors.Code_InvalidLength);
			aisLength = Math.Min(desiredAisLength, actualAisLength);
		}

		return aisLength;
	}

	private static void VerifyCodeStructure(
		int aisLength,
		ApplicationIdentifier[] desiredAiOrder,
		IEnumerable<ApplicationIdentifier>?[] desiredAiAlternativesOrder,
		ApplicationIdentifier[] actualAiOrder,
		List<Error> errors)
	{
		for (int i = 0; i < aisLength; i++)
		{
			if (actualAiOrder[i] != desiredAiOrder[i])
			{
				IEnumerable<ApplicationIdentifier>? alternativeAis = desiredAiAlternativesOrder[i];
				bool hasAlternative = false;

				if (alternativeAis is not null)
				{
					foreach (ApplicationIdentifier alternativeAi in alternativeAis)
					{
						if (actualAiOrder[i] == alternativeAi)
						{
							hasAlternative = true;
							desiredAiOrder[i] = alternativeAi;

							break;
						}
					}
				}

				if (!hasAlternative)
				{
					errors.Add(CodeAnalyserErrors.Code_InvalidStructure);

					break;
				}
			}
		}
	}

	private static void VerifyAILength(
		CodeValidationResults validationResults,
		ApplicationIdentifier ai,
		int requiredLength,
		List<Error> aiErrors)
	{
		string value = validationResults
			.ParsingResults
			.ApplicationIdentifierValuePairs
			.First(p => p.Key == ai)
			.Value;

		if (value.Length != requiredLength)
		{
			KeyValuePair<ApplicationIdentifier, IEnumerable<Error>> aiErrorsPair = validationResults
				.ApplicationIdentifierErrorsPairs
				.FirstOrDefault(p => p.Key == ai);

			bool containsLengthError = false;

			if (!aiErrorsPair.Equals(default(KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>)))
			{
				Error? lengthError = aiErrorsPair
					.Value
					.FirstOrDefault(e => e.Code == CodeAnalyserErrors.AI_InvalidLength.Code);

				if (lengthError is not null)
					containsLengthError = true;
			}

			if (!containsLengthError)
				aiErrors.Add(CodeAnalyserErrors.AI_InvalidLength);
		}
	}

	private static Dictionary<ApplicationIdentifier, int> GenerateAILengthPairs(
		CodeVerificationTemplateDescriptor templateDescriptor,
		ApplicationIdentifier[] desiredAiOrder)
	{
		Dictionary<ApplicationIdentifier, int> aiLengthPairs = [];
		CodeVerificationTemplateUnit[] codeUnits = [.. templateDescriptor.CodeUnits];

		for (int i = 0; i < codeUnits.Length; i++)
		{
			ApplicationIdentifier desiredAi = desiredAiOrder[i];
			CodeVerificationTemplateUnit codeUnit = codeUnits[i];

			if ((desiredAi == codeUnit.ApplicationIdentifier) && (codeUnit.ApplicationIdentifierLength is not null))
			{
				aiLengthPairs.Add(
					codeUnit.ApplicationIdentifier,
					(int)codeUnit.ApplicationIdentifierLength);
			}
			else if ((desiredAi != codeUnit.ApplicationIdentifier) && (codeUnit.Alternatives is not null))
			{
				CodeVerificationTemplateUnit? alternativeCodeUnit = codeUnit
					.Alternatives
					.FirstOrDefault(u => u.ApplicationIdentifier == desiredAi);

				if ((alternativeCodeUnit is not null) && (alternativeCodeUnit.ApplicationIdentifierLength is not null))
				{
					aiLengthPairs.Add(
						alternativeCodeUnit.ApplicationIdentifier,
						(int)alternativeCodeUnit.ApplicationIdentifierLength);
				}
			}
		}

		return aiLengthPairs;
	}

	[MemberNotNull(nameof(s_codeVerificationTemplateDescriptorPairs))]
	private static void InitializeCodeVerificationTemplateDescriptorPairs() => s_codeVerificationTemplateDescriptorPairs =
		CodeAnalyserData.CodeVerificationTemplateDescriptors
			.Select(d => new KeyValuePair<CodeVerificationTemplate, CodeVerificationTemplateDescriptor>(
				d.Template,
				d))
			.ToDictionary();
}