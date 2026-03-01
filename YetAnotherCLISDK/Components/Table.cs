using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class Table : IRenderable
{
    private record Column(string Header, Alignment Align);
    private readonly List<Column>   _columns = [];
    private readonly List<string[]> _rows    = [];
    private string?     _title;
    private BorderStyle _border      = BorderStyle.Single;
    private Style       _borderStyle = Style.Default;
    private Style       _headerStyle = new() { Bold = true };

    public Table AddColumn(string header, Alignment align = Alignment.Left)
    {
        _columns.Add(new Column(header, align));
        return this;
    }

    public Table AddRow(params string[] cells)
    {
        _rows.Add(cells);
        return this;
    }

    public Table WithTitle(string title)              { _title       = title;  return this; }
    public Table WithBorder(BorderStyle border)       { _border      = border; return this; }
    public Table WithBorderStyle(Style style)         { _borderStyle = style;  return this; }
    public Table WithHeaderStyle(Style style)         { _headerStyle = style;  return this; }

    public IEnumerable<string> GetLines()
    {
        if (_columns.Count == 0) yield break;

        // Calculate column widths
        var widths = _columns.Select(c => Markup.VisualLength(Markup.Strip(c.Header))).ToArray();
        foreach (var row in _rows)
            for (int i = 0; i < Math.Min(row.Length, widths.Length); i++)
                widths[i] = Math.Max(widths[i], Markup.VisualLength(Markup.Strip(row[i])));

        int cols = _columns.Count;

        string Line(char l, char m, char r, char h) =>
            _borderStyle.Apply(l + string.Join(m, widths.Select(w => new string(h, w + 2))) + r.ToString());

        var lines = new List<string>();

        // Title
        if (_title is not null)
        {
            int tableWidth = widths.Sum() + widths.Length * 3 + 1;
            int titleVis   = Markup.VisualLength(Markup.Strip(_title));
            int pad        = Math.Max(0, tableWidth - 2 - titleVis);
            int lp = pad / 2, rp = pad - lp;
            var styledTitle = new Style { Bold = true }.Apply(Markup.Parse(_title));
            lines.Add(_borderStyle.Apply(_border.TopLeft.ToString())
                + _borderStyle.Apply(new string(_border.Horizontal, tableWidth - 2))
                + _borderStyle.Apply(_border.TopRight.ToString()));
            lines.Add(_borderStyle.Apply(_border.Vertical.ToString())
                + new string(' ', lp) + styledTitle + new string(' ', rp)
                + _borderStyle.Apply(_border.Vertical.ToString()));
            lines.Add(Line(_border.LeftT, _border.TopT, _border.RightT, _border.Horizontal));
        }
        else
        {
            lines.Add(Line(_border.TopLeft, _border.TopT, _border.TopRight, _border.Horizontal));
        }

        // Headers
        var headerCells = _columns.Select((c, i) =>
        {
            var parsed = _headerStyle.Apply(Markup.Parse(c.Header));
            var plain  = Markup.Strip(c.Header);
            int pad    = widths[i] - plain.Length;
            return " " + parsed + new string(' ', pad + 1);
        });
        lines.Add(_borderStyle.Apply(_border.Vertical.ToString())
            + string.Join(_borderStyle.Apply(_border.Vertical.ToString()), headerCells)
            + _borderStyle.Apply(_border.Vertical.ToString()));

        // Header/body separator
        lines.Add(Line(_border.LeftT, _border.Cross, _border.RightT, _border.Horizontal));

        // Rows
        foreach (var row in _rows)
        {
            var cells = _columns.Select((c, i) =>
            {
                var cell   = i < row.Length ? row[i] : "";
                var parsed = Markup.Parse(cell);
                var plain  = Markup.Strip(cell);
                int vis    = plain.Length;
                int w      = widths[i];

                string content = c.Align switch
                {
                    Alignment.Right  => new string(' ', w - vis) + parsed + " ",
                    Alignment.Center => Markup.Center(parsed, w),
                    _                => parsed + new string(' ', w - vis),
                };
                return " " + content + " ";
            });

            lines.Add(_borderStyle.Apply(_border.Vertical.ToString())
                + string.Join(_borderStyle.Apply(_border.Vertical.ToString()), cells)
                + _borderStyle.Apply(_border.Vertical.ToString()));
        }

        // Bottom border
        lines.Add(Line(_border.BottomLeft, _border.BottomT, _border.BottomRight, _border.Horizontal));

        foreach (var line in lines) yield return line;
    }
}
