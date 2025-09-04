using CodeAnalyser.BL.Models;

namespace CodeAnalyser.BL.Services.Abstractions;

public interface ICodeVerifier
{
	CodeVerificationResults Verify(
		CodeValidationResults validationResults,
		CodeVerificationTemplate verificationTemplate,
		bool? hasFNC1);
}