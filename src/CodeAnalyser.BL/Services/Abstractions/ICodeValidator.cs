using CodeAnalyser.BL.Models;

namespace CodeAnalyser.BL.Services.Abstractions;

public interface ICodeValidator
{
	CodeValidationResults Validate(CodeParsingResults parsingResults);
}