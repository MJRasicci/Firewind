namespace Firewind.Data;

using System.Globalization;

/// <summary>
/// Provides factory methods for creating fluent data tree builders.
/// </summary>
public static class DataTree
{
    /// <summary>
    /// Creates a builder that projects a flat sequence into a hierarchy using key and parent-key selectors.
    /// </summary>
    /// <typeparam name="TDataItem">The source data item type.</typeparam>
    /// <typeparam name="TKey">The key type used to correlate parent and child items.</typeparam>
    /// <param name="keySelector">Selects a unique key for each item.</param>
    /// <param name="parentKeySelector">Selects the parent key for each item, or <see langword="null"/> for root items.</param>
    /// <returns>A configured tree builder.</returns>
    public static DataTreeBuilder<TDataItem, TKey> FromFlat<TDataItem, TKey>(
        Func<TDataItem, TKey> keySelector,
        Func<TDataItem, TKey?> parentKeySelector)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(keySelector);
        ArgumentNullException.ThrowIfNull(parentKeySelector);

        return new DataTreeBuilder<TDataItem, TKey>(keySelector, parentKeySelector, childrenSelector: null);
    }

    /// <summary>
    /// Creates a builder that projects a hierarchical sequence by traversing child collections.
    /// </summary>
    /// <typeparam name="TDataItem">The source data item type.</typeparam>
    /// <typeparam name="TKey">The key type used to identify each item.</typeparam>
    /// <param name="keySelector">Selects a unique key for each item.</param>
    /// <param name="childrenSelector">Selects child items for each parent item.</param>
    /// <returns>A configured tree builder.</returns>
    public static DataTreeBuilder<TDataItem, TKey> FromHierarchy<TDataItem, TKey>(
        Func<TDataItem, TKey> keySelector,
        Func<TDataItem, IEnumerable<TDataItem>?> childrenSelector)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(keySelector);
        ArgumentNullException.ThrowIfNull(childrenSelector);

        return new DataTreeBuilder<TDataItem, TKey>(keySelector, parentKeySelector: null, childrenSelector);
    }
}

/// <summary>
/// Builds immutable <see cref="IDataTree{TDataItem}"/> instances from source data.
/// </summary>
/// <typeparam name="TDataItem">The source data item type.</typeparam>
/// <typeparam name="TKey">The key type used to identify nodes.</typeparam>
public sealed class DataTreeBuilder<TDataItem, TKey>
    where TKey : notnull
{
    private static readonly IReadOnlyDictionary<string, object?> EmptyMetadata = new Dictionary<string, object?>();

    private readonly Func<TDataItem, TKey> keySelector;
    private readonly Func<TDataItem, TKey?>? parentKeySelector;
    private readonly Func<TDataItem, IEnumerable<TDataItem>?>? childrenSelector;
    private readonly Dictionary<string, Func<TDataItem, object?>> metadataSelectors = new(StringComparer.Ordinal);
    private Func<TDataItem, DataTreeNodeKind>? nodeKindSelector;
    private Func<TDataItem, bool?>? collapsibleSelector;
    private Func<TDataItem, bool>? expandedSelector;
    private bool requireKnownParents;

    internal DataTreeBuilder(
        Func<TDataItem, TKey> keySelector,
        Func<TDataItem, TKey?>? parentKeySelector,
        Func<TDataItem, IEnumerable<TDataItem>?>? childrenSelector)
    {
        this.keySelector = keySelector;
        this.parentKeySelector = parentKeySelector;
        this.childrenSelector = childrenSelector;
    }

    /// <summary>
    /// Sets a selector that determines each node's semantic kind.
    /// </summary>
    /// <param name="selector">The selector used to determine node kinds.</param>
    /// <returns>The current builder instance.</returns>
    public DataTreeBuilder<TDataItem, TKey> WithNodeKind(Func<TDataItem, DataTreeNodeKind> selector)
    {
        this.nodeKindSelector = selector ?? throw new ArgumentNullException(nameof(selector));
        return this;
    }

    /// <summary>
    /// Sets a selector that determines whether each node should be collapsible.
    /// </summary>
    /// <param name="selector">
    /// The selector used to determine collapsibility.
    /// Return <see langword="null"/> to use default behavior (collapsible when children exist).
    /// </param>
    /// <returns>The current builder instance.</returns>
    public DataTreeBuilder<TDataItem, TKey> WithCollapsible(Func<TDataItem, bool?> selector)
    {
        this.collapsibleSelector = selector ?? throw new ArgumentNullException(nameof(selector));
        return this;
    }

    /// <summary>
    /// Sets a selector that determines whether each node starts expanded.
    /// </summary>
    /// <param name="selector">The selector used to determine expanded state.</param>
    /// <returns>The current builder instance.</returns>
    public DataTreeBuilder<TDataItem, TKey> WithExpanded(Func<TDataItem, bool> selector)
    {
        this.expandedSelector = selector ?? throw new ArgumentNullException(nameof(selector));
        return this;
    }

    /// <summary>
    /// Registers an additional metadata value to attach to every built node.
    /// </summary>
    /// <param name="metadataKey">The metadata key.</param>
    /// <param name="selector">Selects the metadata value from a source item.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="metadataKey"/> is null, empty, or whitespace.</exception>
    public DataTreeBuilder<TDataItem, TKey> WithMetadata(string metadataKey, Func<TDataItem, object?> selector)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(metadataKey);

        this.metadataSelectors[metadataKey] = selector ?? throw new ArgumentNullException(nameof(selector));
        return this;
    }

    /// <summary>
    /// Configures whether flat projections should fail when an item's parent key does not resolve.
    /// </summary>
    /// <param name="required">
    /// <see langword="true"/> to throw for missing parents; otherwise unresolved items are treated as roots.
    /// </param>
    /// <returns>The current builder instance.</returns>
    public DataTreeBuilder<TDataItem, TKey> RequireKnownParents(bool required = true)
    {
        this.requireKnownParents = required;
        return this;
    }

    /// <summary>
    /// Builds an immutable tree from the provided source items.
    /// </summary>
    /// <param name="sourceItems">The source items to project.</param>
    /// <returns>A tree representation of <paramref name="sourceItems"/>.</returns>
    public IDataTree<TDataItem> Build(IEnumerable<TDataItem> sourceItems)
    {
        ArgumentNullException.ThrowIfNull(sourceItems);

        return this.childrenSelector is null
            ? BuildFromFlat(sourceItems)
            : BuildFromHierarchy(sourceItems);
    }

    private DataTreeModel BuildFromFlat(IEnumerable<TDataItem> sourceItems)
    {
        if (this.parentKeySelector is null)
        {
            throw new InvalidOperationException("A parent key selector is required for flat tree projections.");
        }

        var nodesByKey = new Dictionary<TKey, MutableNode>(EqualityComparer<TKey>.Default);
        var parentKeys = new Dictionary<TKey, TKey?>(EqualityComparer<TKey>.Default);
        var keyOrder = new List<TKey>();

        foreach (var sourceItem in sourceItems)
        {
            var key = this.keySelector(sourceItem);
            if (!nodesByKey.TryAdd(key, new MutableNode(sourceItem, ToNodeKeyString(key))))
            {
                throw new InvalidOperationException($"Duplicate tree node key '{ToNodeKeyString(key)}' was detected.");
            }

            parentKeys.Add(key, this.parentKeySelector(sourceItem));
            keyOrder.Add(key);
        }

        var roots = new List<MutableNode>(keyOrder.Count);
        foreach (var key in keyOrder)
        {
            var node = nodesByKey[key];
            var parentKey = parentKeys[key];

            if (parentKey is null)
            {
                roots.Add(node);
                continue;
            }

            if (nodesByKey.TryGetValue(parentKey, out var parentNode))
            {
                parentNode.Children.Add(node);
                continue;
            }

            if (this.requireKnownParents)
            {
                throw new InvalidOperationException(
                    $"Unable to resolve parent key '{ToNodeKeyString(parentKey)}' for node '{node.Key}'.");
            }

            roots.Add(node);
        }

        DetectCycles(nodesByKey.Values);
        return MaterializeTree(roots);
    }

    private DataTreeModel BuildFromHierarchy(IEnumerable<TDataItem> sourceItems)
    {
        if (this.childrenSelector is null)
        {
            throw new InvalidOperationException("A children selector is required for hierarchical tree projections.");
        }

        var nodesByKey = new Dictionary<TKey, MutableNode>(EqualityComparer<TKey>.Default);
        var activeBranch = new HashSet<TKey>(EqualityComparer<TKey>.Default);
        var roots = new List<MutableNode>();

        foreach (var sourceItem in sourceItems)
        {
            roots.Add(BuildHierarchyNode(sourceItem, nodesByKey, activeBranch));
        }

        DetectCycles(nodesByKey.Values);
        return MaterializeTree(roots);
    }

    private MutableNode BuildHierarchyNode(
        TDataItem sourceItem,
        IDictionary<TKey, MutableNode> nodesByKey,
        ISet<TKey> activeBranch)
    {
        if (this.childrenSelector is null)
        {
            throw new InvalidOperationException("A children selector is required for hierarchical tree projections.");
        }

        var key = this.keySelector(sourceItem);
        if (!activeBranch.Add(key))
        {
            throw new InvalidOperationException($"Cycle detected while traversing node '{ToNodeKeyString(key)}'.");
        }

        if (nodesByKey.ContainsKey(key))
        {
            throw new InvalidOperationException(
                $"Duplicate tree node key '{ToNodeKeyString(key)}' was detected while traversing hierarchy.");
        }

        var node = new MutableNode(sourceItem, ToNodeKeyString(key));
        nodesByKey.Add(key, node);

        var children = this.childrenSelector(sourceItem);
        if (children is not null)
        {
            foreach (var child in children)
            {
                node.Children.Add(BuildHierarchyNode(child, nodesByKey, activeBranch));
            }
        }

        activeBranch.Remove(key);
        return node;
    }

    private DataTreeModel MaterializeTree(IEnumerable<MutableNode> roots)
    {
        var rootNodes = new List<IDataTreeNode<TDataItem>>();
        foreach (var root in roots)
        {
            rootNodes.Add(MaterializeNode(root, level: 0));
        }

        return new DataTreeModel(rootNodes);
    }

    private DataTreeNodeModel MaterializeNode(MutableNode node, int level)
    {
        var children = new List<IDataTreeNode<TDataItem>>(node.Children.Count);
        foreach (var child in node.Children)
        {
            children.Add(MaterializeNode(child, level + 1));
        }

        var kind = this.nodeKindSelector?.Invoke(node.SourceItem) ?? DataTreeNodeKind.Item;
        var hasChildren = children.Count > 0;
        var isCollapsible = hasChildren && (this.collapsibleSelector?.Invoke(node.SourceItem) ?? true);
        var isExpanded = isCollapsible && (this.expandedSelector?.Invoke(node.SourceItem) ?? false);

        return new DataTreeNodeModel(
            key: node.Key,
            dataItem: node.SourceItem,
            level,
            kind,
            children,
            isCollapsible,
            isExpanded,
            BuildMetadata(node.SourceItem));
    }

    private IReadOnlyDictionary<string, object?> BuildMetadata(TDataItem sourceItem)
    {
        if (this.metadataSelectors.Count == 0)
        {
            return EmptyMetadata;
        }

        var metadata = new Dictionary<string, object?>(this.metadataSelectors.Count, StringComparer.Ordinal);
        foreach (var metadataEntry in this.metadataSelectors)
        {
            metadata[metadataEntry.Key] = metadataEntry.Value(sourceItem);
        }

        return metadata;
    }

    private static void DetectCycles(IEnumerable<MutableNode> nodes)
    {
        var nodeVisitStates = new Dictionary<MutableNode, VisitState>();
        foreach (var node in nodes)
        {
            VisitNode(node, nodeVisitStates);
        }
    }

    private static void VisitNode(MutableNode node, IDictionary<MutableNode, VisitState> nodeVisitStates)
    {
        if (nodeVisitStates.TryGetValue(node, out var state))
        {
            if (state == VisitState.Visiting)
            {
                throw new InvalidOperationException($"Cycle detected while traversing node '{node.Key}'.");
            }

            if (state == VisitState.Visited)
            {
                return;
            }
        }

        nodeVisitStates[node] = VisitState.Visiting;
        foreach (var child in node.Children)
        {
            VisitNode(child, nodeVisitStates);
        }

        nodeVisitStates[node] = VisitState.Visited;
    }

    private static string ToNodeKeyString(TKey key) => Convert.ToString(key, CultureInfo.InvariantCulture)
        ?? throw new InvalidOperationException("Tree node keys must convert to non-null strings.");

    private enum VisitState
    {
        Visiting,
        Visited
    }

    private sealed class MutableNode
    {
        public MutableNode(TDataItem sourceItem, string key)
        {
            this.SourceItem = sourceItem;
            this.Key = key;
        }

        public TDataItem SourceItem { get; }

        public string Key { get; }

        public List<MutableNode> Children { get; } = [];
    }

    private sealed class DataTreeModel : IDataTree<TDataItem>
    {
        public DataTreeModel(IReadOnlyList<IDataTreeNode<TDataItem>> rootNodes)
        {
            this.RootNodes = rootNodes;
        }

        public IReadOnlyList<IDataTreeNode<TDataItem>> RootNodes { get; }
    }

    private sealed class DataTreeNodeModel : IDataTreeNode<TDataItem>
    {
        public DataTreeNodeModel(
            string key,
            TDataItem? dataItem,
            int level,
            DataTreeNodeKind kind,
            IReadOnlyList<IDataTreeNode<TDataItem>> children,
            bool isCollapsible,
            bool isExpanded,
            IReadOnlyDictionary<string, object?> metadata)
        {
            this.Key = key;
            this.DataItem = dataItem;
            this.Level = level;
            this.Kind = kind;
            this.Children = children;
            this.IsCollapsible = isCollapsible;
            this.IsExpanded = isExpanded;
            this.Metadata = metadata;
        }

        public string Key { get; }

        public TDataItem? DataItem { get; }

        public int Level { get; }

        public DataTreeNodeKind Kind { get; }

        public IReadOnlyList<IDataTreeNode<TDataItem>> Children { get; }

        public bool IsCollapsible { get; }

        public bool IsExpanded { get; }

        public IReadOnlyDictionary<string, object?> Metadata { get; }
    }
}
