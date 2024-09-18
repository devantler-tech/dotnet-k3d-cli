using System.Runtime.InteropServices;
using CliWrap;
using Devantler.CLIRunner;

namespace Devantler.K3dCLI;

/// <summary>
/// A class to run k3d CLI commands.
/// </summary>
public static class K3d
{
  /// <summary>
  /// The K3d CLI command.
  /// </summary>
  static Command Command => GetCommand();

  internal static Command GetCommand(PlatformID? platformID = default, Architecture? architecture = default, string? runtimeIdentifier = default)
  {
    platformID ??= Environment.OSVersion.Platform;
    architecture ??= RuntimeInformation.ProcessArchitecture;
    runtimeIdentifier ??= RuntimeInformation.RuntimeIdentifier;

    string binary = (platformID, architecture, runtimeIdentifier) switch
    {
      (PlatformID.Unix, Architecture.X64, "osx-x64") => "k3d-osx-x64",
      (PlatformID.Unix, Architecture.Arm64, "osx-arm64") => "k3d-osx-arm64",
      (PlatformID.Unix, Architecture.X64, "linux-x64") => "k3d-linux-x64",
      (PlatformID.Unix, Architecture.Arm64, "linux-arm64") => "k3d-linux-arm64",
      (PlatformID.Win32NT, Architecture.X64, "win-x64") => "k3d-win-x64.exe",
      _ => throw new PlatformNotSupportedException($"Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
    };
    string binaryPath = Path.Combine(AppContext.BaseDirectory, binary);
    return !File.Exists(binaryPath) ?
      throw new FileNotFoundException($"{binaryPath} not found.") :
      Cli.Wrap(binaryPath);
  }

  /// <summary>
  /// Creates a new k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="configPath"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public static async Task<int> CreateClusterAsync(string clusterName, string configPath, CancellationToken cancellationToken)
  {
    var cmd = Command.WithArguments(
        [
          "cluster",
          "create",
          $"{clusterName}",
          $"--config={configPath}"
        ]
      );
    var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
    return exitCode;
  }

  /// <summary>
  /// Starts a k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public static async Task<int> StartClusterAsync(string clusterName, CancellationToken cancellationToken)
  {
    var cmd = Command.WithArguments($"cluster start {clusterName}");
    var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
    return exitCode;
  }

  /// <summary>
  /// Stops a k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public static async Task<int> StopClusterAsync(string clusterName, CancellationToken cancellationToken)
  {
    var cmd = Command.WithArguments($"cluster stop {clusterName}");
    var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
    return exitCode;
  }

  /// <summary>
  /// Deletes a k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public static async Task<int> DeleteClusterAsync(string clusterName, CancellationToken cancellationToken)
  {
    var cmd = Command.WithArguments($"cluster delete {clusterName}");
    var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
    return exitCode;
  }

  /// <summary>
  /// Gets a k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public static async Task<(int exitCode, bool Result)> GetClusterAsync(string clusterName, CancellationToken cancellationToken)
  {
    var cmd = Command.WithArguments($"cluster get {clusterName}").WithValidation(CommandResultValidation.None);
    var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
    return (exitCode, exitCode == 0);
  }

  /// <summary>
  /// Lists all k3d clusters.
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public static async Task<(int exitCode, string result)> ListClustersAsync(CancellationToken cancellationToken)
  {
    var cmd = Command.WithArguments("cluster list");
    var (exitCode, result) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
    return (exitCode, result);
  }
}
