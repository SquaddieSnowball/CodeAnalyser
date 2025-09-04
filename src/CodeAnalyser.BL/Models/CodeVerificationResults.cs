using CodeAnalyser.BL.Primitives;

namespace CodeAnalyser.BL.Models;

public record class CodeVerificationResults(
	CodeValidationResults ValidationResults,
	bool Approved,
	IEnumerable<Error> Errors,
	IEnumerable<KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>> ApplicationIdentifierErrorsPairs);