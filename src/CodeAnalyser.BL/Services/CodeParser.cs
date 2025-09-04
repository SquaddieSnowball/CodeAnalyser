using CodeAnalyser.BL.Data;
using CodeAnalyser.BL.Models;
using CodeAnalyser.BL.Services.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CodeAnalyser.BL.Services;

public class CodeParser : ICodeParser
{
	private const char CharToIntConvertValue = '0';
	private const string ApplicationIdentifierEnumPrefix = "AI";
	private const char ApplicationIdentifierSeparator = '\u001d';

	private const int ApplicationIdentifiersDataRowCount = 5;
	private static int s_applicationIdentifiersDataColumnCount;

	private static int[,] s_applicationIdentifiersData;
	private static Dictionary<CodeParsingTemplate, CodeParsingTemplateDescriptor> s_codeParsingTemplateDescriptorPairs;

	static CodeParser()
	{
		InitializeApplicationIdentifiersData();
		InitializeCodeParsingTemplateDescriptorPairs();
	}

	public CodeParsingResults Parse(string code)
	{
		Dictionary<ApplicationIdentifier, string> aiValuePairs = [];
		int codePointer = 0;

		while (codePointer < code.Length)
		{
			int aiDataPosition = 0;
			int aiDataDepth = 0;
			int aiDataLastMatchedDigit = -1;

			StringBuilder aiStringBuilder = new();

			while (aiDataDepth < ApplicationIdentifiersDataRowCount - 1)
			{
				CodeParsingResults? parsingResults = TryFindNextAIDigit(
					code,
					aiValuePairs,
					ref codePointer,
					ref aiDataPosition,
					ref aiDataDepth,
					ref aiDataLastMatchedDigit,
					out int aiDigit);

				if (parsingResults is not null)
					return parsingResults;

				aiStringBuilder.Append(aiDigit);

				if (s_applicationIdentifiersData[aiDataDepth, aiDataPosition] is -1)
					break;
			}

			ApplicationIdentifier ai = Enum.Parse<ApplicationIdentifier>($"{ApplicationIdentifierEnumPrefix}{aiStringBuilder}");

			if (aiValuePairs.ContainsKey(ai))
				return new CodeParsingResults(code, false, aiValuePairs, code[(codePointer - aiDataDepth)..]);

			string value = TakeAIValue(
				code,
				ref codePointer,
				s_applicationIdentifiersData[ApplicationIdentifiersDataRowCount - 1, aiDataPosition]);

			aiValuePairs.Add(ai, value);
		}

		return new CodeParsingResults(code, true, aiValuePairs);
	}

	public CodeParsingResults Parse(string code, CodeParsingTemplate parsingTemplate)
	{
		CodeParsingTemplateDescriptor templateDescriptor;

		try
		{
			templateDescriptor = s_codeParsingTemplateDescriptorPairs[parsingTemplate];
		}
		catch (KeyNotFoundException)
		{
			throw new NotImplementedException(
				"Дескриптор для указанного шаблона еще не определен");
		}

		Dictionary<ApplicationIdentifier, string> aiValuePairs = [];
		int codePointer = 0;

		foreach (CodeParsingTemplateUnit codeUnit in templateDescriptor.CodeUnits)
		{
			if (codePointer >= code.Length)
				break;

			string value = TakeAIValue(
				code,
				ref codePointer,
				codeUnit.ApplicationIdentifierLength);

			aiValuePairs.Add(codeUnit.ApplicationIdentifier, value);
		}

		if (codePointer < code.Length)
			return new CodeParsingResults(code, false, aiValuePairs, code[codePointer..]);

		return new CodeParsingResults(code, true, aiValuePairs);
	}

	private static CodeParsingResults? TryFindNextAIDigit(
		string code,
		Dictionary<ApplicationIdentifier, string> aiValuePairs,
		ref int codePointer,
		ref int aiDataPosition,
		ref int aiDataDepth,
		ref int aiDataLastMatchedDigit,
		out int aiDigit)
	{
		aiDigit = code[codePointer] - CharToIntConvertValue;

		while ((aiDataPosition < s_applicationIdentifiersDataColumnCount)
			&& (aiDigit != s_applicationIdentifiersData[aiDataDepth, aiDataPosition]))
		{
			if ((aiDataLastMatchedDigit is not -1)
				&& (s_applicationIdentifiersData[aiDataDepth - 1, aiDataPosition] != aiDataLastMatchedDigit))
				return new CodeParsingResults(code, false, aiValuePairs, code[(codePointer - aiDataDepth)..]);

			aiDataPosition++;
		}

		if (aiDataPosition == s_applicationIdentifiersDataColumnCount)
			return new CodeParsingResults(code, false, aiValuePairs, code[(codePointer - aiDataDepth)..]);

		codePointer++;
		aiDataDepth++;

		if (codePointer == code.Length)
			return new CodeParsingResults(code, false, aiValuePairs, code[(codePointer - aiDataDepth)..]);

		aiDataLastMatchedDigit = aiDigit;

		return default;
	}

	private static string TakeAIValue(string code, ref int codePointer, int limit)
	{
		StringBuilder valueStringBuilder = new();

		if (limit is not -1)
		{
			int taken = 0;

			while ((codePointer < code.Length) && (taken < limit))
			{
				valueStringBuilder.Append(code[codePointer]);

				codePointer++;
				taken++;
			}
		}
		else
		{
			while (codePointer < code.Length)
			{
				if (code[codePointer] == ApplicationIdentifierSeparator)
				{
					codePointer++;

					break;
				}

				valueStringBuilder.Append(code[codePointer]);

				codePointer++;
			}
		}

		return valueStringBuilder.ToString();
	}

	[MemberNotNull(nameof(s_applicationIdentifiersData))]
	private static void InitializeApplicationIdentifiersData()
	{
		ApplicationIdentifierDescriptor[] aiDescriptors = [.. CodeAnalyserData.ApplicationIdentifierDescriptors];

		s_applicationIdentifiersDataColumnCount = aiDescriptors.Length;
		s_applicationIdentifiersData = new int[ApplicationIdentifiersDataRowCount, s_applicationIdentifiersDataColumnCount];

		for (int j = 0; j < aiDescriptors.Length; j++)
		{
			ApplicationIdentifierDescriptor aiDescriptor = aiDescriptors[j];
			string aiString = aiDescriptor.Identifier.ToString()[2..];

			for (int i = 0; i < ApplicationIdentifiersDataRowCount - 1; i++)
			{
				s_applicationIdentifiersData[i, j] =
					i < aiString.Length
					? aiString[i] - CharToIntConvertValue
					: -1;
			}

			s_applicationIdentifiersData[ApplicationIdentifiersDataRowCount - 1, j] =
				aiDescriptor.Constraints.MinLength == aiDescriptor.Constraints.MaxLength
				? aiDescriptor.Constraints.MinLength
				: -1;
		}
	}

	[MemberNotNull(nameof(s_codeParsingTemplateDescriptorPairs))]
	private static void InitializeCodeParsingTemplateDescriptorPairs() => s_codeParsingTemplateDescriptorPairs =
		CodeAnalyserData.CodeParsingTemplateDescriptors
			.Select(d => new KeyValuePair<CodeParsingTemplate, CodeParsingTemplateDescriptor>(
				d.Template,
				d))
			.ToDictionary();
}