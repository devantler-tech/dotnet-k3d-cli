using System.Runtime.InteropServices;
using CliWrap;
using CliWrap.Exceptions;
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
  /// Runs the k3d CLI command with the given arguments.
  /// </summary>
  /// <param name="arguments"></param>
  /// <param name="validation"></param>
  /// <param name="silent"></param>
  /// <param name="includeStdErr"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public static async Task<(int ExitCode, string Message)> RunAsync(
    string[] arguments,
    CommandResultValidation validation = CommandResultValidation.ZeroExitCode,
    bool silent = false,
    bool includeStdErr = true,
    CancellationToken cancellationToken = default)
  {
    return await CLI.RunAsync(
      Command.WithArguments(arguments),
      validation: validation,
      silent: silent,
      includeStdErr: includeStdErr,
      cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  /// <summary>
  /// Creates a new k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="configPath"></param>
  /// <param name="cancellationToken"></param>
  [Obsolete("This method is deprecated. Use RunAsync instead.")]
  public static async Task CreateClusterAsync(string clusterName, string configPath, CancellationToken cancellationToken = default)
  {
    var cmd = Command.WithArguments(
        [
          "cluster",
          "create",
          $"{clusterName}",
          $"--config={configPath}"
        ]
      );
    try
    {
      var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
      if (exitCode != 0)
      {
        throw new K3dException($"Failed to create k3d cluster.");
      }
    }
    catch (CommandExecutionException ex)
    {
      throw new K3dException("Failed to create k3d cluster.", ex);
    }
  }

  /// <summary>
  /// Starts a k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="cancellationToken"></param>
  [Obsolete("This method is deprecated. Use RunAsync instead.")]
  public static async Task StartClusterAsync(string clusterName, CancellationToken cancellationToken = default)
  {
    var cmd = Command.WithArguments($"cluster start {clusterName}");
    try
    {
      var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
      if (exitCode != 0)
      {
        throw new K3dException($"Failed to start k3d cluster.");
      }
    }
    catch (CommandExecutionException ex)
    {
      throw new K3dException("Failed to start k3d cluster.", ex);
    }
  }

  /// <summary>
  /// Stops a k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="cancellationToken"></param>
  [Obsolete("This method is deprecated. Use RunAsync instead.")]
  public static async Task StopClusterAsync(string clusterName, CancellationToken cancellationToken = default)
  {
    var cmd = Command.WithArguments($"cluster stop {clusterName}");
    try
    {
      var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
      if (exitCode != 0)
      {
        throw new K3dException($"Failed to stop k3d cluster.");
      }
    }
    catch (CommandExecutionException ex)
    {
      throw new K3dException("Failed to stop k3d cluster.", ex);
    }
  }

  /// <summary>
  /// Deletes a k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="cancellationToken"></param>
  [Obsolete("This method is deprecated. Use RunAsync instead.")]
  public static async Task DeleteClusterAsync(string clusterName, CancellationToken cancellationToken = default)
  {
    var cmd = Command.WithArguments($"cluster delete {clusterName}");
    try
    {
      var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
      if (exitCode != 0)
      {
        throw new K3dException($"Failed to delete k3d cluster.");
      }
    }
    catch (CommandExecutionException ex)
    {
      throw new K3dException("Failed to delete k3d cluster.", ex);
    }
  }

  /// <summary>
  /// Gets a k3d cluster.
  /// </summary>
  /// <param name="clusterName"></param>
  /// <param name="cancellationToken"></param>
  [Obsolete("This method is deprecated. Use RunAsync instead.")]
  public static async Task<bool> GetClusterAsync(string clusterName, CancellationToken cancellationToken = default)
  {
    var cmd = Command.WithArguments($"cluster get {clusterName}").WithValidation(CommandResultValidation.None);
    try
    {
      var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
      return exitCode == 0;
    }
    catch (CommandExecutionException ex)
    {
      throw new K3dException("Failed to get k3d cluster.", ex);
    }
  }

  /// <summary>
  /// Lists all k3d clusters.
  /// </summary>
  /// <param name="cancellationToken"></param>
  [Obsolete("This method is deprecated. Use RunAsync instead.")]
  public static async Task<string[]> ListClustersAsync(CancellationToken cancellationToken = default)
  {
    var cmd = Command.WithArguments("cluster list");
    try
    {
      var (_, result) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
      string[] lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      string[] clusterLines = [.. lines.Skip(1)];
      string[] clusterNames = [.. clusterLines.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).First())];
      return clusterNames;
    }
    catch (CommandExecutionException ex)
    {
      throw new K3dException("Failed to list k3d clusters.", ex);
    }
  }
}
