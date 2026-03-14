using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

/// <summary>
/// Wraps any <see cref="IRenderable"/> and centers it both horizontally and
/// vertically within the terminal window.
/// </summary>
public sealed class CenteredLayout : IRenderable
{
    private readonly IRenderable _content;
    private int  _fixedWidth       = 0;
    private int  _fixedHeight      = 0;
    private bool _clearScreen      = false;

    public CenteredLayout(IRenderable content) => _content = content;

    /// <summary>Override the width used for horizontal centering.</summary>
    public CenteredLayout WithWidth(int width)         { _fixedWidth  = width; return this; }

    /// <summary>Override the height used for vertical centering.</summary>
    public CenteredLayout WithHeight(int height)       { _fixedHeight = height; return this; }

    /// <summary>Clear the terminal before rendering (useful for form/dialog UX).</summary>
    public CenteredLayout WithClearScreen(bool clear = true) { _clearScreen = clear; return this; }

    public IEnumerable<string> GetLines()
    {
        if (_clearScreen) Console.Clear();

        var contentLines = _content.GetLines().ToList();

        // Measure the visual width of the content
        int contentWidth = _fixedWidth > 0
            ? _fixedWidth
            : contentLines.Select(l => Markup.VisualLength(l)).DefaultIfEmpty(0).Max();

        int contentHeight = _fixedHeight > 0 ? _fixedHeight : contentLines.Count;

        int termW = Cli.Width;
        int termH = Cli.Height;

        // Vertical centering: blank lines above
        int topPad = Math.Max(0, (termH - contentHeight) / 2);

        // Horizontal centering: left padding
        int leftPad = Math.Max(0, (termW - contentWidth) / 2);
        var leftStr = new string(' ', leftPad);

        var lines = new List<string>(topPad + contentLines.Count);

        for (int i = 0; i < topPad; i++)
            lines.Add(string.Empty);

        foreach (var line in contentLines)
            lines.Add(leftStr + line);

        return lines;
    }
}
