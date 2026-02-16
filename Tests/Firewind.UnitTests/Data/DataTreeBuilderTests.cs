namespace Firewind.UnitTests.Data;

using Firewind.Data;
using FluentAssertions;

/// <summary>
/// Verifies fluent projection behavior for <see cref="DataTreeBuilder{TDataItem, TKey}"/>.
/// </summary>
public sealed class DataTreeBuilderTests
{
    /// <summary>
    /// Ensures flat key/parent-key projection builds expected hierarchy and levels.
    /// </summary>
    [Fact]
    public void Build_FromFlat_ProjectsExpectedHierarchy()
    {
        var sourceItems = new[]
        {
            new FlatNode("1", null, "Workspace"),
            new FlatNode("2", "1", "Projects"),
            new FlatNode("3", "2", "Firewind"),
            new FlatNode("4", "1", "Settings")
        };

        var tree = DataTree
            .FromFlat<FlatNode, string>(static item => item.Id, static item => item.ParentId)
            .Build(sourceItems);

        tree.RootNodes.Should().ContainSingle();

        var workspaceNode = tree.RootNodes[0];
        workspaceNode.Key.Should().Be("1");
        workspaceNode.Level.Should().Be(0);
        workspaceNode.Children.Should().HaveCount(2);

        var projectsNode = workspaceNode.Children[0];
        projectsNode.Key.Should().Be("2");
        projectsNode.Level.Should().Be(1);
        projectsNode.Children.Should().ContainSingle();
        projectsNode.Children[0].Key.Should().Be("3");
        projectsNode.Children[0].Level.Should().Be(2);
    }

    /// <summary>
    /// Ensures node selectors control kind, collapsibility, expanded state, and metadata.
    /// </summary>
    [Fact]
    public void Build_FromFlat_AppliesConfiguredNodeSelectors()
    {
        var sourceItems = new[]
        {
            new FlatNode("1", null, "Workspace", IsHeader: true, IsExpanded: true, CanCollapse: true),
            new FlatNode("2", "1", "Dashboard")
        };

        var tree = DataTree
            .FromFlat<FlatNode, string>(static item => item.Id, static item => item.ParentId)
            .WithNodeKind(static item => item.IsHeader ? DataTreeNodeKind.Header : DataTreeNodeKind.Item)
            .WithCollapsible(static item => item.CanCollapse)
            .WithExpanded(static item => item.IsExpanded)
            .WithMetadata("label", static item => item.Label)
            .Build(sourceItems);

        var rootNode = tree.RootNodes.Single();
        rootNode.Kind.Should().Be(DataTreeNodeKind.Header);
        rootNode.IsCollapsible.Should().BeTrue();
        rootNode.IsExpanded.Should().BeTrue();
        rootNode.Metadata.Should().ContainKey("label").WhoseValue.Should().Be("Workspace");
        rootNode.Children.Should().ContainSingle();
    }

    /// <summary>
    /// Ensures unresolved parent references are treated as roots by default.
    /// </summary>
    [Fact]
    public void Build_FromFlat_WhenParentIsMissing_TreatsNodeAsRootByDefault()
    {
        var sourceItems = new[]
        {
            new FlatNode("1", "42", "Orphan")
        };

        var tree = DataTree
            .FromFlat<FlatNode, string>(static item => item.Id, static item => item.ParentId)
            .Build(sourceItems);

        tree.RootNodes.Should().ContainSingle();
        tree.RootNodes[0].Key.Should().Be("1");
    }

    /// <summary>
    /// Ensures unresolved parent references can be rejected for strict projections.
    /// </summary>
    [Fact]
    public void Build_FromFlat_WhenParentIsMissingAndRequired_ThrowsInvalidOperationException()
    {
        var sourceItems = new[]
        {
            new FlatNode("1", "42", "Orphan")
        };

        var act = () => DataTree
            .FromFlat<FlatNode, string>(static item => item.Id, static item => item.ParentId)
            .RequireKnownParents()
            .Build(sourceItems);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*parent key*");
    }

    /// <summary>
    /// Ensures cycles in flat parent relationships are rejected.
    /// </summary>
    [Fact]
    public void Build_FromFlat_WhenGraphContainsCycle_ThrowsInvalidOperationException()
    {
        var sourceItems = new[]
        {
            new FlatNode("1", "2", "A"),
            new FlatNode("2", "1", "B")
        };

        var act = () => DataTree
            .FromFlat<FlatNode, string>(static item => item.Id, static item => item.ParentId)
            .Build(sourceItems);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Cycle*");
    }

    /// <summary>
    /// Ensures hierarchy traversal builds expected child structure and depth.
    /// </summary>
    [Fact]
    public void Build_FromHierarchy_TraversesChildren()
    {
        var firewindNode = new HierNode(3, "Firewind", []);
        var projectsNode = new HierNode(2, "Projects", [firewindNode]);
        var workspaceNode = new HierNode(1, "Workspace", [projectsNode]);

        var tree = DataTree
            .FromHierarchy<HierNode, int>(static item => item.Id, static item => item.Children)
            .Build([workspaceNode]);

        tree.RootNodes.Should().ContainSingle();
        tree.RootNodes[0].Key.Should().Be("1");
        tree.RootNodes[0].Children.Should().ContainSingle();
        tree.RootNodes[0].Children[0].Key.Should().Be("2");
        tree.RootNodes[0].Children[0].Children.Should().ContainSingle();
        tree.RootNodes[0].Children[0].Children[0].Key.Should().Be("3");
        tree.RootNodes[0].Children[0].Children[0].Level.Should().Be(2);
    }

    /// <summary>
    /// Ensures duplicate keys in hierarchical data are rejected.
    /// </summary>
    [Fact]
    public void Build_FromHierarchy_WhenDuplicateKeysExist_ThrowsInvalidOperationException()
    {
        var duplicateNode = new HierNode(1, "Duplicate", []);

        var act = () => DataTree
            .FromHierarchy<HierNode, int>(static item => item.Id, static item => item.Children)
            .Build([
                new HierNode(1, "Root", []),
                duplicateNode
            ]);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Duplicate tree node key*");
    }

    private sealed record FlatNode(
        string Id,
        string? ParentId,
        string Label,
        bool IsHeader = false,
        bool IsExpanded = false,
        bool? CanCollapse = null);

    private sealed record HierNode(int Id, string Label, IReadOnlyList<HierNode> Children);
}
