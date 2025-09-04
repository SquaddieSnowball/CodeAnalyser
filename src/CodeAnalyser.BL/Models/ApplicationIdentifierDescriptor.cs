namespace CodeAnalyser.BL.Models;

public record class ApplicationIdentifierDescriptor(
	ApplicationIdentifier Identifier,
	string Description,
	string DataTitle,
	ApplicationIdentifierConstraints Constraints);