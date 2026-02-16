namespace Firewind.Data;

/// <summary>
/// Represents hierarchical data projected from a source sequence.
/// </summary>
/// <typeparam name="TDataItem">The data item type represented by tree nodes.</typeparam>
public interface IDataTree<TDataItem>
{
    /// <summary>
    /// Gets the root-level nodes in this tree.
    /// </summary>
    public IReadOnlyList<IDataTreeNode<TDataItem>> RootNodes { get; }
}

/// <summary>
/// Represents semantic kinds used by tree-aware components when rendering nodes.
/// </summary>
public enum DataTreeNodeKind
{
    /// <summary>
    /// A standard interactive node.
    /// </summary>
    Item,
    /// <summary>
    /// A non-interactive heading node.
    /// </summary>
    Header
}

/// <summary>
/// Represents a node in an <see cref="IDataTree{TDataItem}"/>.
/// </summary>
/// <typeparam name="TDataItem">The data item type represented by this node.</typeparam>
public interface IDataTreeNode<TDataItem>
{
    /// <summary>
    /// Gets the unique key for this node.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the original data item associated with this node.
    /// </summary>
    public TDataItem? DataItem { get; }

    /// <summary>
    /// Gets the zero-based hierarchy level of this node.
    /// </summary>
    public int Level { get; }

    /// <summary>
    /// Gets the semantic kind of this node.
    /// </summary>
    public DataTreeNodeKind Kind { get; }

    /// <summary>
    /// Gets child nodes under this node.
    /// </summary>
    public IReadOnlyList<IDataTreeNode<TDataItem>> Children { get; }

    /// <summary>
    /// Gets a value indicating whether this node should render as collapsible.
    /// </summary>
    public bool IsCollapsible { get; }

    /// <summary>
    /// Gets a value indicating whether this node is expanded by default.
    /// </summary>
    public bool IsExpanded { get; }

    /// <summary>
    /// Gets arbitrary metadata associated with this node.
    /// </summary>
    /// <remarks>
    /// This allows application-specific values (for example URLs, badges, or custom flags)
    /// to flow into display template logic without modifying the core tree contract.
    /// </remarks>
    public IReadOnlyDictionary<string, object?> Metadata { get; }
}
