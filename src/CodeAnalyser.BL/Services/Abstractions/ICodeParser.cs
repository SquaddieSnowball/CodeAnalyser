using CodeAnalyser.BL.Models;

namespace CodeAnalyser.BL.Services.Abstractions;

public interface ICodeParser
{
	CodeParsingResults Parse(string code);

	CodeParsingResults Parse(string code, CodeParsingTemplate parsingTemplate);
}