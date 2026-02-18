namespace Firewind.Variant;

/// <summary>
/// Represents field styling options for text-like form controls.
/// </summary>
public enum FieldStyle
{
    /// <summary>
    /// Uses the default style for the control.
    /// </summary>
    Default,
    /// <summary>
    /// Applies the ghost visual style.
    /// </summary>
    Ghost
}

/// <summary>
/// Resolves CSS classes for form-control style, color, and size variants.
/// </summary>
public static class FormControlStyleExtensions
{
    /// <summary>
    /// Gets style classes for a text-like control token.
    /// </summary>
    /// <param name="style">The visual style variant.</param>
    /// <param name="componentToken">The daisyUI component token without a prefix (for example, <c>input</c>).</param>
    /// <returns>A CSS class string for the selected style.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="componentToken"/> is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="componentToken"/> is unsupported for field styling.</exception>
    public static string ClassNames(this FieldStyle style, string componentToken)
    {
        var normalizedToken = NormalizeComponentToken(componentToken);

        return normalizedToken switch
        {
            "BADGE" => style switch
            {
                FieldStyle.Ghost => "fw-badge-ghost",
                _ => string.Empty
            },
            "FILE-INPUT" => style switch
            {
                FieldStyle.Ghost => "fw-file-input-ghost",
                _ => string.Empty
            },
            "INPUT" => style switch
            {
                FieldStyle.Ghost => "fw-input-ghost",
                _ => string.Empty
            },
            "SELECT" => style switch
            {
                FieldStyle.Ghost => "fw-select-ghost",
                _ => string.Empty
            },
            "TEXTAREA" => style switch
            {
                FieldStyle.Ghost => "fw-textarea-ghost",
                _ => string.Empty
            },
            _ => throw new ArgumentOutOfRangeException(nameof(componentToken), componentToken, "Unsupported form control component token.")
        };
    }

    /// <summary>
    /// Gets color classes for the provided form-control token.
    /// </summary>
    /// <param name="color">The semantic color token.</param>
    /// <param name="componentToken">The daisyUI component token without a prefix.</param>
    /// <returns>A CSS class string for the selected color.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="componentToken"/> is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="componentToken"/> is unsupported for color variants.</exception>
    public static string FormControlClassNames(this ThemeColor color, string componentToken)
    {
        var normalizedToken = NormalizeComponentToken(componentToken);

        return normalizedToken switch
        {
            "BADGE" => BadgeColorClassNames(color),
            "CHECKBOX" => CheckboxColorClassNames(color),
            "FILE-INPUT" => FileInputColorClassNames(color),
            "INPUT" => InputColorClassNames(color),
            "PROGRESS" => ProgressColorClassNames(color),
            "RADIO" => RadioColorClassNames(color),
            "RANGE" => RangeColorClassNames(color),
            "SELECT" => SelectColorClassNames(color),
            "STATUS" => StatusColorClassNames(color),
            "TEXTAREA" => TextareaColorClassNames(color),
            "TOGGLE" => ToggleColorClassNames(color),
            _ => throw new ArgumentOutOfRangeException(nameof(componentToken), componentToken, "Unsupported form control component token.")
        };
    }

    /// <summary>
    /// Gets size classes for the provided form-control token.
    /// </summary>
    /// <param name="size">The component size value.</param>
    /// <param name="componentToken">The daisyUI component token without a prefix.</param>
    /// <returns>A CSS class string for the selected size.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="componentToken"/> is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="componentToken"/> is unsupported for size variants.</exception>
    public static string FormControlClassNames(this ComponentSize size, string componentToken)
    {
        var normalizedToken = NormalizeComponentToken(componentToken);

        return normalizedToken switch
        {
            "BADGE" => BadgeSizeClassNames(size),
            "CHECKBOX" => CheckboxSizeClassNames(size),
            "FILE-INPUT" => FileInputSizeClassNames(size),
            "INPUT" => InputSizeClassNames(size),
            "KBD" => KbdSizeClassNames(size),
            "LOADING" => LoadingSizeClassNames(size),
            "RADIO" => RadioSizeClassNames(size),
            "RANGE" => RangeSizeClassNames(size),
            "RATING" => RatingSizeClassNames(size),
            "SELECT" => SelectSizeClassNames(size),
            "STATUS" => StatusSizeClassNames(size),
            "TABLE" => TableSizeClassNames(size),
            "TEXTAREA" => TextareaSizeClassNames(size),
            "TOGGLE" => ToggleSizeClassNames(size),
            _ => throw new ArgumentOutOfRangeException(nameof(componentToken), componentToken, "Unsupported form control component token.")
        };
    }

    private static string NormalizeComponentToken(string componentToken)
    {
        if (string.IsNullOrWhiteSpace(componentToken))
        {
            throw new ArgumentException("Component token cannot be null or whitespace.", nameof(componentToken));
        }

        return componentToken.Trim().ToUpperInvariant();
    }

    private static string BadgeColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-badge-neutral",
        ThemeColor.Primary => "fw-badge-primary",
        ThemeColor.Secondary => "fw-badge-secondary",
        ThemeColor.Accent => "fw-badge-accent",
        ThemeColor.Info => "fw-badge-info",
        ThemeColor.Success => "fw-badge-success",
        ThemeColor.Warning => "fw-badge-warning",
        ThemeColor.Error => "fw-badge-error",
        _ => string.Empty
    };

    private static string BadgeSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-badge-xs",
        ComponentSize.Small => "fw-badge-sm",
        ComponentSize.Large => "fw-badge-lg",
        ComponentSize.ExtraLarge => "fw-badge-xl",
        ComponentSize.Responsive => "fw-badge-xs sm:fw-badge-sm md:fw-badge-md lg:fw-badge-lg xl:fw-badge-xl",
        _ => string.Empty
    };

    private static string CheckboxColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-checkbox-neutral",
        ThemeColor.Primary => "fw-checkbox-primary",
        ThemeColor.Secondary => "fw-checkbox-secondary",
        ThemeColor.Accent => "fw-checkbox-accent",
        ThemeColor.Info => "fw-checkbox-info",
        ThemeColor.Success => "fw-checkbox-success",
        ThemeColor.Warning => "fw-checkbox-warning",
        ThemeColor.Error => "fw-checkbox-error",
        _ => string.Empty
    };

    private static string CheckboxSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-checkbox-xs",
        ComponentSize.Small => "fw-checkbox-sm",
        ComponentSize.Large => "fw-checkbox-lg",
        ComponentSize.ExtraLarge => "fw-checkbox-xl",
        ComponentSize.Responsive => "fw-checkbox-xs sm:fw-checkbox-sm md:fw-checkbox-md lg:fw-checkbox-lg xl:fw-checkbox-xl",
        _ => string.Empty
    };

    private static string FileInputColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-file-input-neutral",
        ThemeColor.Primary => "fw-file-input-primary",
        ThemeColor.Secondary => "fw-file-input-secondary",
        ThemeColor.Accent => "fw-file-input-accent",
        ThemeColor.Info => "fw-file-input-info",
        ThemeColor.Success => "fw-file-input-success",
        ThemeColor.Warning => "fw-file-input-warning",
        ThemeColor.Error => "fw-file-input-error",
        _ => string.Empty
    };

    private static string FileInputSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-file-input-xs",
        ComponentSize.Small => "fw-file-input-sm",
        ComponentSize.Large => "fw-file-input-lg",
        ComponentSize.ExtraLarge => "fw-file-input-xl",
        ComponentSize.Responsive => "fw-file-input-xs sm:fw-file-input-sm md:fw-file-input-md lg:fw-file-input-lg xl:fw-file-input-xl",
        _ => string.Empty
    };

    private static string InputColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-input-neutral",
        ThemeColor.Primary => "fw-input-primary",
        ThemeColor.Secondary => "fw-input-secondary",
        ThemeColor.Accent => "fw-input-accent",
        ThemeColor.Info => "fw-input-info",
        ThemeColor.Success => "fw-input-success",
        ThemeColor.Warning => "fw-input-warning",
        ThemeColor.Error => "fw-input-error",
        _ => string.Empty
    };

    private static string InputSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-input-xs",
        ComponentSize.Small => "fw-input-sm",
        ComponentSize.Large => "fw-input-lg",
        ComponentSize.ExtraLarge => "fw-input-xl",
        ComponentSize.Responsive => "fw-input-xs sm:fw-input-sm md:fw-input-md lg:fw-input-lg xl:fw-input-xl",
        _ => string.Empty
    };

    private static string KbdSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-kbd-xs",
        ComponentSize.Small => "fw-kbd-sm",
        ComponentSize.Large => "fw-kbd-lg",
        ComponentSize.ExtraLarge => "fw-kbd-xl",
        ComponentSize.Responsive => "fw-kbd-xs sm:fw-kbd-sm md:fw-kbd-md lg:fw-kbd-lg xl:fw-kbd-xl",
        _ => string.Empty
    };

    private static string LoadingSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-loading-xs",
        ComponentSize.Small => "fw-loading-sm",
        ComponentSize.Large => "fw-loading-lg",
        ComponentSize.ExtraLarge => "fw-loading-xl",
        ComponentSize.Responsive => "fw-loading-xs sm:fw-loading-sm md:fw-loading-md lg:fw-loading-lg xl:fw-loading-xl",
        _ => string.Empty
    };

    private static string ProgressColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-progress-neutral",
        ThemeColor.Primary => "fw-progress-primary",
        ThemeColor.Secondary => "fw-progress-secondary",
        ThemeColor.Accent => "fw-progress-accent",
        ThemeColor.Info => "fw-progress-info",
        ThemeColor.Success => "fw-progress-success",
        ThemeColor.Warning => "fw-progress-warning",
        ThemeColor.Error => "fw-progress-error",
        _ => string.Empty
    };

    private static string RadioColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-radio-neutral",
        ThemeColor.Primary => "fw-radio-primary",
        ThemeColor.Secondary => "fw-radio-secondary",
        ThemeColor.Accent => "fw-radio-accent",
        ThemeColor.Info => "fw-radio-info",
        ThemeColor.Success => "fw-radio-success",
        ThemeColor.Warning => "fw-radio-warning",
        ThemeColor.Error => "fw-radio-error",
        _ => string.Empty
    };

    private static string RadioSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-radio-xs",
        ComponentSize.Small => "fw-radio-sm",
        ComponentSize.Large => "fw-radio-lg",
        ComponentSize.ExtraLarge => "fw-radio-xl",
        ComponentSize.Responsive => "fw-radio-xs sm:fw-radio-sm md:fw-radio-md lg:fw-radio-lg xl:fw-radio-xl",
        _ => string.Empty
    };

    private static string RangeColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-range-neutral",
        ThemeColor.Primary => "fw-range-primary",
        ThemeColor.Secondary => "fw-range-secondary",
        ThemeColor.Accent => "fw-range-accent",
        ThemeColor.Info => "fw-range-info",
        ThemeColor.Success => "fw-range-success",
        ThemeColor.Warning => "fw-range-warning",
        ThemeColor.Error => "fw-range-error",
        _ => string.Empty
    };

    private static string RangeSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-range-xs",
        ComponentSize.Small => "fw-range-sm",
        ComponentSize.Large => "fw-range-lg",
        ComponentSize.ExtraLarge => "fw-range-xl",
        ComponentSize.Responsive => "fw-range-xs sm:fw-range-sm md:fw-range-md lg:fw-range-lg xl:fw-range-xl",
        _ => string.Empty
    };

    private static string RatingSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-rating-xs",
        ComponentSize.Small => "fw-rating-sm",
        ComponentSize.Large => "fw-rating-lg",
        ComponentSize.ExtraLarge => "fw-rating-xl",
        ComponentSize.Responsive => "fw-rating-xs sm:fw-rating-sm md:fw-rating-md lg:fw-rating-lg xl:fw-rating-xl",
        _ => string.Empty
    };

    private static string SelectColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-select-neutral",
        ThemeColor.Primary => "fw-select-primary",
        ThemeColor.Secondary => "fw-select-secondary",
        ThemeColor.Accent => "fw-select-accent",
        ThemeColor.Info => "fw-select-info",
        ThemeColor.Success => "fw-select-success",
        ThemeColor.Warning => "fw-select-warning",
        ThemeColor.Error => "fw-select-error",
        _ => string.Empty
    };

    private static string SelectSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-select-xs",
        ComponentSize.Small => "fw-select-sm",
        ComponentSize.Large => "fw-select-lg",
        ComponentSize.ExtraLarge => "fw-select-xl",
        ComponentSize.Responsive => "fw-select-xs sm:fw-select-sm md:fw-select-md lg:fw-select-lg xl:fw-select-xl",
        _ => string.Empty
    };

    private static string StatusColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-status-neutral",
        ThemeColor.Primary => "fw-status-primary",
        ThemeColor.Secondary => "fw-status-secondary",
        ThemeColor.Accent => "fw-status-accent",
        ThemeColor.Info => "fw-status-info",
        ThemeColor.Success => "fw-status-success",
        ThemeColor.Warning => "fw-status-warning",
        ThemeColor.Error => "fw-status-error",
        _ => string.Empty
    };

    private static string StatusSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-status-xs",
        ComponentSize.Small => "fw-status-sm",
        ComponentSize.Large => "fw-status-lg",
        ComponentSize.ExtraLarge => "fw-status-xl",
        ComponentSize.Responsive => "fw-status-xs sm:fw-status-sm md:fw-status-md lg:fw-status-lg xl:fw-status-xl",
        _ => string.Empty
    };

    private static string TableSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-table-xs",
        ComponentSize.Small => "fw-table-sm",
        ComponentSize.Large => "fw-table-lg",
        ComponentSize.ExtraLarge => "fw-table-xl",
        ComponentSize.Responsive => "fw-table-xs sm:fw-table-sm md:fw-table-md lg:fw-table-lg xl:fw-table-xl",
        _ => string.Empty
    };

    private static string TextareaColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-textarea-neutral",
        ThemeColor.Primary => "fw-textarea-primary",
        ThemeColor.Secondary => "fw-textarea-secondary",
        ThemeColor.Accent => "fw-textarea-accent",
        ThemeColor.Info => "fw-textarea-info",
        ThemeColor.Success => "fw-textarea-success",
        ThemeColor.Warning => "fw-textarea-warning",
        ThemeColor.Error => "fw-textarea-error",
        _ => string.Empty
    };

    private static string TextareaSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-textarea-xs",
        ComponentSize.Small => "fw-textarea-sm",
        ComponentSize.Large => "fw-textarea-lg",
        ComponentSize.ExtraLarge => "fw-textarea-xl",
        ComponentSize.Responsive => "fw-textarea-xs sm:fw-textarea-sm md:fw-textarea-md lg:fw-textarea-lg xl:fw-textarea-xl",
        _ => string.Empty
    };

    private static string ToggleColorClassNames(ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-toggle-neutral",
        ThemeColor.Primary => "fw-toggle-primary",
        ThemeColor.Secondary => "fw-toggle-secondary",
        ThemeColor.Accent => "fw-toggle-accent",
        ThemeColor.Info => "fw-toggle-info",
        ThemeColor.Success => "fw-toggle-success",
        ThemeColor.Warning => "fw-toggle-warning",
        ThemeColor.Error => "fw-toggle-error",
        _ => string.Empty
    };

    private static string ToggleSizeClassNames(ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-toggle-xs",
        ComponentSize.Small => "fw-toggle-sm",
        ComponentSize.Large => "fw-toggle-lg",
        ComponentSize.ExtraLarge => "fw-toggle-xl",
        ComponentSize.Responsive => "fw-toggle-xs sm:fw-toggle-sm md:fw-toggle-md lg:fw-toggle-lg xl:fw-toggle-xl",
        _ => string.Empty
    };
}
