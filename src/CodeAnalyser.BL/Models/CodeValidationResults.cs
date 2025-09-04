using CodeAnalyser.BL.Primitives;

namespace CodeAnalyser.BL.Models;

public record class CodeValidationResults(
	CodeParsingResults ParsingResults,
	bool Valid,
	IEnumerable<KeyValuePair<ApplicationIdentifier, IEnumerable<Error>>> ApplicationIdentifierErrorsPairs);