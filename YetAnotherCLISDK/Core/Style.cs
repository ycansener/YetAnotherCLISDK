using System.Text;

namespace YetAnotherCLISDK.Core;

public sealed class Style
{
    public Color? Foreground    { get; init; }
    public Color? Background    { get; init; }
    public bool Bold            { get; init; }
    public bool Italic          { get; init; }
    public bool Underline       { get; init; }
    public bool Strikethrough   { get; init; }
    public bool Dim             { get; init; }
    public bool Blink           { get; init; }
    public bool Inverse         { get; init; }

    public static readonly Style Default = new();

    public string GetPrefix()
    {
        var sb = new StringBuilder();
        if (Foreground.HasValue)  sb.Append(Foreground.Value.ToFg());
        if (Background.HasValue)  sb.Append(Background.Value.ToBg());
        if (Bold)          sb.Append(Ansi.Bold);
        if (Dim)           sb.Append(Ansi.Dim);
        if (Italic)        sb.Append(Ansi.Italic);
        if (Underline)     sb.Append(Ansi.Underline);
        if (Blink)         sb.Append(Ansi.Blink);
        if (Inverse)       sb.Append(Ansi.Inverse);
        if (Strikethrough) sb.Append(Ansi.Strikethrough);
        return sb.ToString();
    }

    public string Apply(string text)
    {
        var prefix = GetPrefix();
        return string.IsNullOrEmpty(prefix) ? text : $"{prefix}{text}{Ansi.Reset}";
    }
}
