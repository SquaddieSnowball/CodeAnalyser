namespace CodeAnalyser.BL.Models;

public record class CodeVerificationTemplateDescriptor(
	CodeVerificationTemplate Template,
	string Description,
	bool MustHaveFNC1,
	IEnumerable<CodeVerificationTemplateUnit> CodeUnits);