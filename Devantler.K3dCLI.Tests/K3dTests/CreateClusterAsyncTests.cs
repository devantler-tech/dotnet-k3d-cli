namespace Devantler.K3dCLI.Tests.K3dTests;

/// <summary>
/// Tests for the <see cref="K3d.CreateClusterAsync(string, string, CancellationToken)"/> method.
/// </summary>
public class CreateClusterAsyncTests
{
  /// <summary>
  /// Test to verify that the method returns a zero exit code when valid parameters are passed.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task CreateClusterAsync_ValidParameters_ReturnsZeroExitCode()
  {
    // Arrange
    string clusterName = "test-cluster";
    string configPath = $"{AppContext.BaseDirectory}assets/k3d-config.yaml";

    // Act
    int createExitCode = await K3d.CreateClusterAsync(clusterName, configPath, CancellationToken.None);
    var (listExitCode, listResult) = await K3d.ListClustersAsync(CancellationToken.None);
    var (getExitCode, getResult) = await K3d.GetClusterAsync(clusterName, CancellationToken.None);
    int stopExitCode = await K3d.StopClusterAsync(clusterName, CancellationToken.None);
    int startExitCode = await K3d.StartClusterAsync(clusterName, CancellationToken.None);

    // Assert
    Assert.Equal(0, createExitCode);
    Assert.Equal(0, listExitCode);
    Assert.Equal(0, getExitCode);
    Assert.Equal(0, stopExitCode);
    Assert.Equal(0, startExitCode);
    Assert.True(getResult);
    _ = await Verify(listResult);

    // Cleanup
    _ = await K3d.DeleteClusterAsync(clusterName, CancellationToken.None);
  }

  /// <summary>
  /// Test to verify that the method throws an exception when invalid parameters are passed.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task CreateClusterAsync_InvalidParameters_ReturnsNonZeroExitCode()
  {
    // Arrange
    string clusterName = "test-cluster";
    string configPath = $"{AppContext.BaseDirectory}assets/invalid-config.yaml";

    // Act
    int exitCode = await K3d.CreateClusterAsync(clusterName, configPath, CancellationToken.None);

    // Assert
    Assert.NotEqual(0, exitCode);
  }
}
