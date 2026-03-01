using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class BarChart : IRenderable
{
    private record Bar(string Label, double Value, Color? BarColor);

    private readonly List<Bar> _bars = [];
    private string? _title;
    private int     _barWidth    = 0; // 0 = auto
    private char    _fillChar    = '█';
    private Style   _labelStyle  = Style.Default;
    private Style   _valueStyle  = new() { Bold = true };
    private Color   _defaultColor = Color.Cyan;

    public BarChart WithTitle(string title)         { _title = title; return this; }
    public BarChart WithBarWidth(int w)             { _barWidth = w;  return this; }
    public BarChart WithDefaultColor(Color color)   { _defaultColor = color; return this; }

    public BarChart AddBar(string label, double value, Color? color = null)
    {
        _bars.Add(new Bar(label, value, color));
        return this;
    }

    public IEnumerable<string> GetLines()
    {
        if (_bars.Count == 0) yield break;

        var lines = new List<string>();

        if (_title is not null)
            lines.Add(new Style { Bold = true }.Apply(Markup.Parse(_title)));

        int labelMaxWidth = _bars.Select(b => b.Label.Length).Max();
        double maxVal     = _bars.Select(b => b.Value).Max();
        int barMaxWidth   = _barWidth > 0 ? _barWidth : Math.Max(20, Cli.Width - labelMaxWidth - 12);

        foreach (var bar in _bars)
        {
            int filled = maxVal > 0 ? (int)Math.Round(bar.Value / maxVal * barMaxWidth) : 0;
            var color  = bar.BarColor ?? _defaultColor;
            var barStr = new Style { Foreground = color }.Apply(new string(_fillChar, filled));
            var label  = _labelStyle.Apply(bar.Label.PadLeft(labelMaxWidth));
            var val    = _valueStyle.Apply($" {bar.Value}");

            lines.Add($"  {label} │{barStr}{val}");
        }

        foreach (var line in lines) yield return line;
    }
}
