namespace Devantler.K3dCLI;

/// <summary>
/// A custom exception for k3d CLI errors.
/// </summary>
[Serializable]
public class K3dException : Exception
{
  /// <inheritdoc />
  public K3dException()
  {
  }

  /// <inheritdoc />
  public K3dException(string? message) : base(message)
  {
  }

  /// <inheritdoc />
  public K3dException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
