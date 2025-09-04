namespace CodeAnalyser.BL.Primitives;

public record class Error(
	int Code,
	string Message);