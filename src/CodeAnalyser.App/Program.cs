using CodeAnalyser.BL.Data;
using CodeAnalyser.BL.Models;
using CodeAnalyser.BL.Primitives;
using CodeAnalyser.BL.Services;
using System.Text;

Dictionary<ApplicationIdentifier, ApplicationIdentifierDescriptor> aiDescriptorPairs =
	CodeAnalyserData.ApplicationIdentifierDescriptors
		.Select(d => new KeyValuePair<ApplicationIdentifier, ApplicationIdentifierDescriptor>(
			d.Identifier,
			d))
		.ToDictionary();

Dictionary<CodeVerificationTemplate, CodeVerificationTemplateDescriptor> verificationTemplateDescriptorPairs =
	CodeAnalyserData.CodeVerificationTemplateDescriptors
		.Select(d => new KeyValuePair<CodeVerificationTemplate, CodeVerificationTemplateDescriptor>(
			d.Template,
			d))
		.ToDictionary();

string code = "12345678901234123456712341234";
CodeParsingTemplate parsingTemplate = CodeParsingTemplate.RuTobaccoProducts;
CodeVerificationTemplate verificationTemplate = CodeVerificationTemplate.RuTobaccoProducts;
CodeVerificationTemplateDescriptor verificationTemplateDescriptor = verificationTemplateDescriptorPairs[verificationTemplate];

CodeParser parser = new();
CodeParsingResults parsingResults = parser.Parse(code, parsingTemplate);

CodeValidator validator = new();
CodeValidationResults validationResults = validator.Validate(parsingResults);

CodeVerifier verifier = new();
CodeVerificationResults verificationResults = verifier.Verify(validationResults, verificationTemplate, false);

Console.WriteLine($"Код: {parsingResults.Raw}{Environment.NewLine}");
Console.WriteLine($"Шаблон: {verificationTemplateDescriptor.Description}{Environment.NewLine}");

if (!parsingResults.Success || verificationResults.Errors.Any())
{
	Console.WriteLine("Ошибки кода:");

	if (!parsingResults.Success)
		Console.WriteLine($"\tВ коде присутствует нераспознанная часть: {parsingResults.Tail}");

	foreach (Error error in verificationResults.Errors)
		Console.WriteLine($"\t{error.Message}");

	Console.WriteLine();
}

StringBuilder datailsTable = new();

datailsTable.AppendLine($"{"AI",-10}{"Описание",-50}{"Наименование данных",-30}{"Значение",-30}{"Ошибки"}");

foreach (CodeVerificationTemplateUnit codeUnit in verificationTemplateDescriptor.CodeUnits)
{
	CodeVerificationTemplateUnit currentCodeUnit = codeUnit;

	KeyValuePair<ApplicationIdentifier, string> aiValuePair = parsingResults
		.ApplicationIdentifierValuePairs
		.FirstOrDefault(p => p.Key == currentCodeUnit.ApplicationIdentifier);

	if (aiValuePair.Equals(default(KeyValuePair<ApplicationIdentifier, string>)) && (codeUnit.Alternatives is not null))
	{
		foreach (CodeVerificationTemplateUnit alternativeCodeUnit in codeUnit.Alternatives)
		{
			aiValuePair = parsingResults
				.ApplicationIdentifierValuePairs
				.FirstOrDefault(p => p.Key == alternativeCodeUnit.ApplicationIdentifier);

			if (!aiValuePair.Equals(default(KeyValuePair<ApplicationIdentifier, string>)))
			{
				currentCodeUnit = alternativeCodeUnit;

				break;
			}
		}
	}

	ApplicationIdentifier ai = aiValuePair.Equals(default(KeyValuePair<ApplicationIdentifier, string>))
		? currentCodeUnit.ApplicationIdentifier
		: aiValuePair.Key;

	datailsTable.Append($"{ai,-10}");

	datailsTable.Append($"{(currentCodeUnit.ApplicationIdentifierDescription is null
		? aiDescriptorPairs[ai].Description
		: currentCodeUnit.ApplicationIdentifierDescription),-50}");

	datailsTable.Append($"{aiDescriptorPairs[ai].DataTitle,-30}");

	string value = parsingResults
		.ApplicationIdentifierValuePairs
		.FirstOrDefault(p => p.Key == ai)
		.Value;

	datailsTable.Append($"{value,-30}");

	IEnumerable<Error>? validationErrors = validationResults
		.ApplicationIdentifierErrorsPairs
		.FirstOrDefault(p => p.Key == ai)
		.Value;

	IEnumerable<Error>? verificationErrors = verificationResults
		.ApplicationIdentifierErrorsPairs
		.FirstOrDefault(p => p.Key == ai)
		.Value;

	IEnumerable<Error>? errors = [];

	errors = errors
		.Concat(validationErrors ?? [])
		.Concat(verificationErrors ?? []);

	datailsTable.Append(string.Join("; ", errors.Select(e => e.Message)));

	datailsTable.AppendLine();
}

Console.WriteLine("Детализация:");
Console.WriteLine();
Console.Write(datailsTable.ToString());

Console.ReadLine();