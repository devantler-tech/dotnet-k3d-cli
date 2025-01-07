using CliWrap;

namespace Devantler.K3dCLI.Tests.K3dTests;

/// <summary>
/// Tests for the <see cref="K3d.RunAsync(string[], CommandResultValidation, bool, bool, CancellationToken)" /> method.
/// </summary>
public class RunAsyncTests
{
  /// <summary>
  /// Tests that the binary can return the version of the flux CLI command.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task RunAsync_Version_ReturnsVersion()
  {
    // Act
    var (exitCode, message) = await K3d.RunAsync(["--version"]);

    // Assert
    Assert.Equal(0, exitCode);
    Assert.Matches(@"^k3d version v\d+\.\d+\.\d+$", message.Trim().Split('\n')[0]);
  }
}
