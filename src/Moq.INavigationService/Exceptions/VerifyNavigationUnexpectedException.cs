namespace Moq;

/// <summary>
/// Verify Navigation Unexpected Exception
/// Thrown when an unhandled exception is raised, if you see this thrown please file a bug report.
/// </summary>
public class VerifyNavigationUnexpectedException : Exception
{
	/// <summary>
	/// Default VerifyNavigationUnexpectedException constructor
	/// </summary>
	public VerifyNavigationUnexpectedException()
	{
	}

	/// <summary>
	/// VerifyNavigationUnexpectedException constructor from a string message
	/// </summary>
	/// <param name="message">The exception message</param>
	public VerifyNavigationUnexpectedException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// VerifyNavigationUnexpectedException constructor from a string message and an inner exception
	/// </summary>
	/// <param name="message">The exception message</param>
	/// <param name="innerException">The inner exception message</param>
	public VerifyNavigationUnexpectedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
