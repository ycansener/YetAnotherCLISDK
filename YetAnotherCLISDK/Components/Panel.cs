using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class Panel : IRenderable
{
    private readonly string[] _contentLines;
    private string?     _title;
    private BorderStyle _border      = BorderStyle.Rounded;
    private Style       _borderStyle = Style.Default;
    private Style       _titleStyle  = new() { Bold = true };
    private int         _paddingX    = 1;
    private int         _paddingY    = 0;
    private int         _fixedWidth  = 0;

    public Panel(string content)
        => _contentLines = content.Replace("\r\n", "\n").Split('\n');

    public Panel WithTitle(string title)              { _title       = title;  return this; }
    public Panel WithBorder(BorderStyle border)       { _border      = border; return this; }
    public Panel WithBorderStyle(Style style)         { _borderStyle = style;  return this; }
    public Panel WithTitleStyle(Style style)          { _titleStyle  = style;  return this; }
    public Panel WithPadding(int x, int y = 0)       { _paddingX = x; _paddingY = y; return this; }
    public Panel WithWidth(int width)                 { _fixedWidth  = width;  return this; }

    public IEnumerable<string> GetLines()
    {
        // Determine inner width (excluding border chars)
        int contentMaxWidth = _contentLines.Select(l => Markup.VisualLength(Markup.Strip(l))).DefaultIfEmpty(0).Max();
        int titleWidth      = _title is null ? 0 : Markup.VisualLength(Markup.Strip(_title));

        int innerWidth = _fixedWidth > 0
            ? _fixedWidth - 2
            : Math.Max(contentMaxWidth + _paddingX * 2, titleWidth + _paddingX * 2);

        innerWidth = Math.Max(innerWidth, 4);

        var lines = new List<string>();

        // ─── Top border ───
        lines.Add(BuildTopBorder(innerWidth));

        // Vertical padding top
        for (int i = 0; i < _paddingY; i++)
            lines.Add(BuildEmptyRow(innerWidth));

        // Content
        foreach (var raw in _contentLines)
        {
            var parsed = Markup.Parse(raw);
            var plain  = Markup.Strip(raw);
            int vis    = Markup.VisualLength(parsed);
            int avail  = innerWidth - _paddingX * 2;
            int padR   = Math.Max(0, avail - Markup.VisualLength(plain));

            var left  = _borderStyle.Apply(_border.Vertical.ToString());
            var right = _borderStyle.Apply(_border.Vertical.ToString());
            lines.Add($"{left}{new string(' ', _paddingX)}{parsed}{new string(' ', padR)}{new string(' ', _paddingX)}{right}");
        }

        // Vertical padding bottom
        for (int i = 0; i < _paddingY; i++)
            lines.Add(BuildEmptyRow(innerWidth));

        // ─── Bottom border ───
        var bl = _borderStyle.Apply(_border.BottomLeft.ToString());
        var br = _borderStyle.Apply(_border.BottomRight.ToString());
        var bh = _borderStyle.Apply(new string(_border.Horizontal, innerWidth));
        lines.Add($"{bl}{bh}{br}");

        return lines;
    }

    private string BuildTopBorder(int innerWidth)
    {
        var tl = _borderStyle.Apply(_border.TopLeft.ToString());
        var tr = _borderStyle.Apply(_border.TopRight.ToString());

        if (_title is null)
        {
            var h = _borderStyle.Apply(new string(_border.Horizontal, innerWidth));
            return $"{tl}{h}{tr}";
        }

        var plainTitle  = Markup.Strip(_title);
        var parsedTitle = _titleStyle.Apply(Markup.Parse(_title));
        int titleLen    = plainTitle.Length;

        if (titleLen + 4 > innerWidth)
        {
            var h = _borderStyle.Apply(new string(_border.Horizontal, innerWidth));
            return $"{tl}{h}{tr}";
        }

        int remaining = innerWidth - titleLen - 2; // 2 for spaces around title
        int leftLen   = remaining / 2;
        int rightLen  = remaining - leftLen;

        var leftH  = _borderStyle.Apply(new string(_border.Horizontal, leftLen));
        var rightH = _borderStyle.Apply(new string(_border.Horizontal, rightLen));
        return $"{tl}{leftH} {parsedTitle} {rightH}{tr}";
    }

    private string BuildEmptyRow(int innerWidth)
    {
        var left  = _borderStyle.Apply(_border.Vertical.ToString());
        var right = _borderStyle.Apply(_border.Vertical.ToString());
        return $"{left}{new string(' ', innerWidth)}{right}";
    }
}
