namespace CodeAnalyser.BL.Models;

public record class ApplicationIdentifierConstraints(
	int MinLength,
	int MaxLength,
	bool AllowDigits,
	bool AllowLetters,
	bool AllowSpecialChars);