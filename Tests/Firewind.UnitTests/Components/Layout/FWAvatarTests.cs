namespace Firewind.UnitTests.Components.Layout;

using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies class composition and state validation for <see cref="FWAvatar"/>.
/// </summary>
public sealed class FWAvatarTests
{
    [Fact]
    public void OnParametersSet_WithPlaceholder_ComposesExpectedCssClasses()
    {
        var avatar = new TestAvatar();
        avatar.Configure(online: false, offline: false, placeholder: true);

        avatar.ApplyParameters();

        avatar.ComponentAttributes["class"].Should().Be("fw-avatar fw-avatar-placeholder");
    }

    [Fact]
    public void OnParametersSet_WhenOnlineAndOffline_ThrowsInvalidOperationException()
    {
        var avatar = new TestAvatar();
        avatar.Configure(online: true, offline: true, placeholder: false);

        var action = avatar.ApplyParameters;

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*cannot be both online and offline*");
    }

    private sealed class TestAvatar : FWAvatar
    {
        public void Configure(bool online, bool offline, bool placeholder)
        {
            this.Online = online;
            this.Offline = offline;
            this.Placeholder = placeholder;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
