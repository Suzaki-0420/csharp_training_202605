namespace csharp_training_202605.Exceptions;

/// <summary>
/// データが存在することを表す例外クラス
/// </summary>
public class ExistsException : Exception
{
    public ExistsException(string message)
    : base(message) { }
    public ExistsException(string message, Exception innerException)
    : base(message, innerException) { }
}