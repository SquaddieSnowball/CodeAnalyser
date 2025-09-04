namespace CodeAnalyser.BL.Models;

public record class CodeParsingTemplateDescriptor(
	CodeParsingTemplate Template,
	string Description,
	IEnumerable<CodeParsingTemplateUnit> CodeUnits);