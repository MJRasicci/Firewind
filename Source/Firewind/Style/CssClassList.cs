namespace Firewind.Style;

using System.Collections;

internal struct ClassBucket
{
    public int Hash { get; set; }

    public object[] Objects { get; set; }
}

internal struct Node
{
    public override int GetHashCode()
    {
        if (this.Prefix is not null)
            return Prefix.GetHashCode();
        else
            return Value.GetHashCode();
    }

    public string? Prefix { get; set; }

    public string Value { get; set; }
}

internal sealed class Tree
{
    public List<Node> Nodes { get; } = [];
}

/// <summary>
/// Represents a list of CSS classes.
/// </summary>
internal sealed class CssClassList : ICollection<string>, IEnumerable<string>
{
    private readonly List<string> orderedClasses = [];
    private readonly HashSet<string> uniqueClasses = [];

    public CssClassList() { }

    public CssClassList(string classNames) => Add(classNames);

    public CssClassList(params string[] classNames) => Add(classNames);

    public void Add(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }

        var splitTokens = token.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var t in splitTokens)
        {
            if (this.uniqueClasses.Contains(t))
            {
                continue;
            }

            this.uniqueClasses.Add(t);
            this.orderedClasses.Add(t);
        }
    }

    public void Add(params string[] tokens)
    {
        foreach (var token in tokens)
        {
            Add(token);
        }
    }

    public bool Remove(string token)
    {
        if (this.uniqueClasses.Remove(token))
        {
            this.orderedClasses.Remove(token);
            return true;
        }

        return false;
    }

    public void Remove(params string[] tokens)
    {
        foreach (var token in tokens)
        {
            Add(token);
        }
    }

    public bool Toggle(string token)
    {
        if (this.uniqueClasses.Contains(token))
        {
            Remove(token);
            return false;
        }
        else
        {
            Add(token);
            return true;
        }
    }

    public bool Contains(string token) => this.uniqueClasses.Contains(token);

    public bool Replace(string oldToken, string newToken)
    {
        if (!this.uniqueClasses.Contains(oldToken) || this.uniqueClasses.Contains(newToken))
        {
            return false;
        }

        var index = this.orderedClasses.IndexOf(oldToken);
        this.orderedClasses[index] = newToken;
        this.uniqueClasses.Remove(oldToken);
        this.uniqueClasses.Add(newToken);
        return true;
    }

    public int Count => this.orderedClasses.Count;

    public bool IsReadOnly => false;

    public void Clear()
    {
        this.uniqueClasses.Clear();
        this.orderedClasses.Clear();
    }

    public void CopyTo(string[] array, int arrayIndex) => this.orderedClasses.CopyTo(array, arrayIndex);

    public IEnumerator<string> GetEnumerator() => this.orderedClasses.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString() => string.Join(' ', this.orderedClasses);
}
