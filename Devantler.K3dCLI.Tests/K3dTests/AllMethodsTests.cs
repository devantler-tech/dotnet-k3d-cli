namespace Devantler.K3dCLI.Tests.K3dTests;

/// <summary>
/// Tests for all methods in the <see cref="K3d"/> class.
/// </summary>
public class AllMethodsTests
{
  /// <summary>
  /// Test to verify that all methods in the <see cref="K3d"/> class work as expected.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AllMethods_WithValidParameters_Succeeds()
  {
    // Arrange
    string clusterName = "test-cluster";
    string configPath = $"{AppContext.BaseDirectory}assets/k3d-config.yaml";

    // Act
    var createClusterException = await Record.ExceptionAsync(async () => await K3d.CreateClusterAsync(clusterName, configPath, CancellationToken.None).ConfigureAwait(false));
    string[] clusters = await K3d.ListClustersAsync(CancellationToken.None);
    bool clusterExists = await K3d.GetClusterAsync(clusterName, CancellationToken.None);
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
  /// Test to verify that all methods in the <see cref="K3d"/> class fail as expected.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task WithInvalidParameters_Fails()
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
