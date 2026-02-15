namespace Firewind.UnitTests.Components.Layout;

using Firewind.Components;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using System.Reflection;
using System.Runtime.ExceptionServices;

/// <summary>
/// Verifies registration and active-item resolution behavior for <see cref="FWMultiView{TKey}"/>.
/// </summary>
public sealed class FWMultiViewTests
{
    /// <summary>
    /// Ensures duplicate item keys are rejected.
    /// </summary>
    [Fact]
    public void RegisterItem_WhenDuplicateKeyDetected_ThrowsInvalidOperationException()
    {
        var multiView = new TestMultiView();
        using var first = CreateItem("overview");
        using var second = CreateItem("overview");

        multiView.Register(first);

        var act = () => multiView.Register(second);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Duplicate multi-view key*");
    }

    /// <summary>
    /// Ensures explicit active key selection targets only the matching item.
    /// </summary>
    [Fact]
    public void IsItemActive_WhenActiveKeyProvided_ReturnsTrueOnlyForMatchingKey()
    {
        var multiView = new TestMultiView();
        multiView.SetActiveKey("details");

        using var overview = CreateItem("overview");
        using var details = CreateItem("details");
        multiView.Register(overview);
        multiView.Register(details);

        multiView.IsActive("details").Should().BeTrue();
        multiView.IsActive("overview").Should().BeFalse();
    }

    /// <summary>
    /// Ensures first-item fallback is used when no active key is provided.
    /// </summary>
    [Fact]
    public void IsItemActive_WhenActiveKeyIsNull_AutoSelectsFirstItem()
    {
        var multiView = new TestMultiView();
        using var first = CreateItem("first");
        using var second = CreateItem("second");
        multiView.Register(first);
        multiView.Register(second);

        multiView.IsActive("first").Should().BeTrue();
        multiView.IsActive("second").Should().BeFalse();
    }

    /// <summary>
    /// Ensures missing explicit keys do not silently fall back to another item.
    /// </summary>
    [Fact]
    public void IsItemActive_WhenActiveKeyIsMissing_ReturnsFalseForRegisteredItems()
    {
        var multiView = new TestMultiView();
        multiView.SetActiveKey("missing");
        using var first = CreateItem("first");
        using var second = CreateItem("second");
        multiView.Register(first);
        multiView.Register(second);

        multiView.IsActive("first").Should().BeFalse();
        multiView.IsActive("second").Should().BeFalse();
    }

    /// <summary>
    /// Ensures first-item fallback can be disabled.
    /// </summary>
    [Fact]
    public void IsItemActive_WhenAutoSelectFirstItemDisabled_ReturnsFalseWithoutActiveKey()
    {
        var multiView = new TestMultiView();
        multiView.SetAutoSelectFirstItem(false);
        using var first = CreateItem("first");
        multiView.Register(first);

        multiView.IsActive("first").Should().BeFalse();
    }

    /// <summary>
    /// Ensures unregistering an active item removes it from active resolution.
    /// </summary>
    [Fact]
    public void UnregisterItem_WhenActiveItemRemoved_ResolvesNoActiveItemForMissingExplicitKey()
    {
        var multiView = new TestMultiView();
        multiView.SetActiveKey("details");
        using var overview = CreateItem("overview");
        using var details = CreateItem("details");

        multiView.Register(overview);
        multiView.Register(details);
        multiView.Unregister(details, "details");

        multiView.IsActive("overview").Should().BeFalse();
        multiView.IsActive("details").Should().BeFalse();
    }

    /// <summary>
    /// Ensures empty-content templates can be customized by callers.
    /// </summary>
    [Fact]
    public void EmptyContent_WhenSet_UsesCustomTemplate()
    {
        var multiView = new TestMultiView();
        RenderFragment customEmptyContent = static _ => { };

        multiView.SetEmptyContent(customEmptyContent);
        multiView.EmptyContent.Should().BeSameAs(customEmptyContent);
    }

    private static TestMultiViewItem CreateItem(string key)
    {
        var item = new TestMultiViewItem();
        item.Configure(key, static _ => { });
        return item;
    }

    private sealed class TestMultiView : FWMultiView<string>
    {
        private static readonly MethodInfo RegisterItemMethod = typeof(FWMultiView<string>)
            .GetMethod("RegisterItem", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Expected internal RegisterItem method.");

        private static readonly MethodInfo UnregisterItemMethod = typeof(FWMultiView<string>)
            .GetMethod("UnregisterItem", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Expected internal UnregisterItem method.");

        private static readonly MethodInfo IsItemActiveMethod = typeof(FWMultiView<string>)
            .GetMethod("IsItemActive", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Expected internal IsItemActive method.");

        public void SetActiveKey(string key) => this.ActiveKey = key;

        public void SetAutoSelectFirstItem(bool value) => this.AutoSelectFirstItem = value;

        public void SetEmptyContent(RenderFragment fragment) => this.EmptyContent = fragment;

        public bool IsActive(string key) => InvokeInternal<bool>(IsItemActiveMethod, [key]);

        public void Register(FWMultiViewItem<string> item) => InvokeInternal(RegisterItemMethod, [item]);

        public void Unregister(FWMultiViewItem<string> item, string key) => InvokeInternal(UnregisterItemMethod, [item, key]);

        protected override void OnItemsChanged()
        {
            // Suppress rerender requests during unit tests where no renderer is attached.
        }

        private void InvokeInternal(MethodInfo method, object?[] parameters)
        {
            try
            {
                _ = method.Invoke(this, parameters);
            }
            catch (TargetInvocationException ex) when (ex.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        private TValue InvokeInternal<TValue>(MethodInfo method, object?[] parameters)
        {
            try
            {
                return (TValue)method.Invoke(this, parameters)!;
            }
            catch (TargetInvocationException ex) when (ex.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }

    private sealed class TestMultiViewItem : FWMultiViewItem<string>
    {
        public void Configure(string key, RenderFragment childContent)
        {
            this.Key = key;
            this.ChildContent = childContent;
        }
    }
}
