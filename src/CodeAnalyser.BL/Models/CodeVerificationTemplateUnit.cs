namespace CodeAnalyser.BL.Models;

public record class CodeVerificationTemplateUnit(
	ApplicationIdentifier ApplicationIdentifier,
	int? ApplicationIdentifierLength = default,
	string? ApplicationIdentifierDescription = default,
	IEnumerable<CodeVerificationTemplateUnit>? Alternatives = default);