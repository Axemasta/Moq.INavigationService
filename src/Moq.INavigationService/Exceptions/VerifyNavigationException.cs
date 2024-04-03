namespace Moq;

/// <summary>
/// Verify Navigation Exception
/// Thrown when a Moq verification exception is thrown
/// </summary>
public class VerifyNavigationException : Exception
{
	/// <summary>
	/// Default VerifyNavigationException constructor
	/// </summary>
	public VerifyNavigationException()
	{
	}

	/// <summary>
	/// VerifyNavigationException constructor from a string message
	/// </summary>
	/// <param name="message">The exception message</param>
	public VerifyNavigationException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// VerifyNavigationException constructor from a string message and an inner exception
	/// </summary>
	/// <param name="message">The exception message</param>
	/// <param name="innerException">The inner exception message</param>
	public VerifyNavigationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
