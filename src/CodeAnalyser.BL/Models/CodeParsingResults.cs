namespace CodeAnalyser.BL.Models;

public record class CodeParsingResults(
	string Raw,
	bool Success,
	IEnumerable<KeyValuePair<ApplicationIdentifier, string>> ApplicationIdentifierValuePairs,
	string? Tail = default);