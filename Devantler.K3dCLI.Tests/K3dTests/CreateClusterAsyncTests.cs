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
    var createClusterException = await Record.ExceptionAsync(async () => await K3d.CreateClusterAsync(clusterName, configPath, CancellationToken.None).ConfigureAwait(false));
    string[] clusters = await K3d.ListClustersAsync(CancellationToken.None);
    bool clusterExists = clusters.Contains(clusterName);
    var stopClusterException = await Record.ExceptionAsync(async () => await K3d.StopClusterAsync(clusterName, CancellationToken.None).ConfigureAwait(false));
    var startClusterException = await Record.ExceptionAsync(async () => await K3d.StartClusterAsync(clusterName, CancellationToken.None).ConfigureAwait(false));

    // Assert
    Assert.Null(createClusterException);
    string expectedClusterName = Assert.Single(clusters);
    Assert.Equal(clusterName, expectedClusterName);
    Assert.True(clusterExists);
    Assert.Null(stopClusterException);
    Assert.Null(startClusterException);

    // Cleanup
    await K3d.DeleteClusterAsync(clusterName, CancellationToken.None);
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
    var createClusterException = await Record.ExceptionAsync(async () => await K3d.CreateClusterAsync(clusterName, configPath, CancellationToken.None).ConfigureAwait(false));

    // Assert
    Assert.NotNull(createClusterException);
  }
}
