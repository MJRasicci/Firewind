namespace Firewind.Style;

using System.Collections.Generic;
using Firewind.Base;

internal abstract class ComponentDecorator : IFirewindComponent
{
    protected readonly IFirewindComponent component;

    public ComponentDecorator(IFirewindComponent component)
    {
        this.component = component;
    }

    protected virtual Dictionary<string, object> GetAttributes()
    {
        return this.component.ComponentAttributes;
    }

    public string Id => this.component.Id;

    public Dictionary<string, object> ComponentAttributes => GetAttributes();
}

internal class StyleDecorator : ComponentDecorator
{
    public StyleDecorator(IFirewindComponent component) : base(component)
    {
    }

    protected override Dictionary<string, object> GetAttributes()
    {
        var attributes = base.GetAttributes();
        attributes["class"] = this.CssClassList.ToString();
        return attributes;
    }

    private CssClassList CssClassList = [];
}

internal class StateDecorator : ComponentDecorator
{
    public StateDecorator(IFirewindComponent component) : base(component)
    {
    }
}

internal class DataDecorator : ComponentDecorator
{
    public DataDecorator(IFirewindComponent component) : base(component)
    {
    }
}

public enum DropdownState
{
    Closed,
    Open
}

internal interface IComponentState<TState> where TState : Enum
{
    TState Current { get; }

    void ChangeState(TState state);

    Task ChangeStateAsync(TState state);

    event EventHandler<TState> StateHasChanged;
}

public class Dropdown
{
    private IComponentState<DropdownState> state;

    public void Show()
    {
        state.ChangeState(DropdownState.Open);
    }

    public void Hide()
    {
        state.ChangeState(DropdownState.Closed);
    }
}

internal delegate TComponent StyleRule<TComponent>(TComponent component);

internal interface IComponentStyle
{
    CssClassList CssClassList { get; }

    void Apply();

    void AddRule();
}
