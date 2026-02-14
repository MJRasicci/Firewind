namespace Firewind.Variant;

using System.Collections;

/// <summary>
/// Represents a list of CSS classes.
/// </summary>
internal sealed class CssClassList : ICollection<string>, IEnumerable<string>
{
    private readonly List<string> orderedClasses = [];
    private readonly HashSet<string> uniqueClasses = [];

    /// <summary>
    /// Initializes a new empty <see cref="CssClassList"/> instance.
    /// </summary>
    public CssClassList() { }

    /// <summary>
    /// Initializes a new instance and adds CSS classes from a space-delimited string.
    /// </summary>
    /// <param name="classNames">Space-delimited CSS class names.</param>
    public CssClassList(string classNames)
    {
        Add(classNames);
    }

    /// <summary>
    /// Initializes a new instance and adds all provided class tokens.
    /// </summary>
    /// <param name="classNames">CSS class names to add.</param>
    public CssClassList(params string[] classNames)
    {
        Add(classNames);
    }

    /// <summary>
    /// Adds one or more class tokens from a space-delimited string.
    /// </summary>
    /// <param name="token">A space-delimited class string.</param>
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

    /// <summary>
    /// Adds a set of class tokens.
    /// </summary>
    /// <param name="tokens">Class tokens to add.</param>
    public void Add(params string[] tokens)
    {
        foreach (var token in tokens)
        {
            Add(token);
        }
    }

    /// <summary>
    /// Removes a class token if present.
    /// </summary>
    /// <param name="token">The class token to remove.</param>
    /// <returns><see langword="true"/> if the token was removed; otherwise <see langword="false"/>.</returns>
    public bool Remove(string token)
    {
        if (this.uniqueClasses.Remove(token))
        {
            this.orderedClasses.Remove(token);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes each provided class token when present.
    /// </summary>
    /// <param name="tokens">Class tokens to remove.</param>
    public void Remove(params string[] tokens)
    {
        foreach (var token in tokens)
        {
            Remove(token);
        }
    }

    /// <summary>
    /// Adds or removes a class token depending on whether it already exists.
    /// </summary>
    /// <param name="token">The class token to toggle.</param>
    /// <returns>
    /// <see langword="true"/> when the token was added; <see langword="false"/> when it was removed.
    /// </returns>
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

    /// <summary>
    /// Determines whether the list contains a class token.
    /// </summary>
    /// <param name="token">The class token to locate.</param>
    /// <returns><see langword="true"/> if the token exists; otherwise <see langword="false"/>.</returns>
    public bool Contains(string token) => this.uniqueClasses.Contains(token);

    /// <summary>
    /// Replaces an existing class token while preserving order.
    /// </summary>
    /// <param name="oldToken">The class token to replace.</param>
    /// <param name="newToken">The replacement class token.</param>
    /// <returns>
    /// <see langword="true"/> if replacement succeeded; otherwise <see langword="false"/>.
    /// </returns>
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

    /// <summary>
    /// Gets the number of distinct class tokens.
    /// </summary>
    public int Count => this.orderedClasses.Count;

    /// <summary>
    /// Gets a value indicating whether the collection is read-only.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Removes all class tokens.
    /// </summary>
    public void Clear()
    {
        this.uniqueClasses.Clear();
        this.orderedClasses.Clear();
    }

    /// <summary>
    /// Copies class tokens into a target array.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="arrayIndex">The zero-based destination index.</param>
    public void CopyTo(string[] array, int arrayIndex) => this.orderedClasses.CopyTo(array, arrayIndex);

    /// <summary>
    /// Returns a generic enumerator for class tokens in insertion order.
    /// </summary>
    /// <returns>An enumerator over class tokens.</returns>
    public IEnumerator<string> GetEnumerator() => this.orderedClasses.GetEnumerator();

    /// <summary>
    /// Returns a non-generic enumerator for class tokens.
    /// </summary>
    /// <returns>A non-generic enumerator over class tokens.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns class tokens joined by a single space.
    /// </summary>
    /// <returns>A space-delimited CSS class string.</returns>
    public override string ToString() => string.Join(' ', this.orderedClasses);
}
