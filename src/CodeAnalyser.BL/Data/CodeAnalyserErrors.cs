using CodeAnalyser.BL.Primitives;

namespace CodeAnalyser.BL.Data;

internal static class CodeAnalyserErrors
{
	public static readonly Error AI_InvalidLength =
		new(100, "Значение идентификатора применения не соответствует ограничениям по длине");

	public static readonly Error AI_DigitsNotAllowed =
		new(101, "Идентификатор применения не допускает использования цифр");

	public static readonly Error AI_LettersNotAllowed =
		new(102, "Идентификатор применения не допускает использования букв");

	public static readonly Error AI_SpecialCharsNotAllowed =
		new(103, "Идентификатор применения не допускает использования специальных символов");

	public static readonly Error Code_UnknownFNC1 =
		new(200, "Неизвестно, присутствует ли символ FNC1 в коде");

	public static readonly Error Code_HasFNC1 =
		new(201, "В коде присутствует символ FNC1");

	public static readonly Error Code_MissingFNC1 =
		new(202, "В коде отсутствует символ FNC1");

	public static readonly Error Code_InvalidLength =
		new(203, "Код содержит неверное количество идентификаторов применения");

	public static readonly Error Code_InvalidStructure =
		new(204, "Код имеет недопустимую структуру");

	public static readonly Error Code_MissingAI =
		new(205, "В коде отсутствует идентификатор применения");
}