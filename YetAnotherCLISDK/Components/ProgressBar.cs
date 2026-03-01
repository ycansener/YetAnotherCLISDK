using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class ProgressBar : IDisposable
{
    private readonly double _total;
    private double _current = 0;
    private string _label   = "";
    private int    _width   = 40;
    private char   _fill    = '█';
    private char   _empty   = '░';
    private Style  _fillStyle  = new() { Foreground = Color.Cyan };
    private Style  _emptyStyle = new() { Foreground = Color.DarkGray };
    private bool   _disposed;

    public ProgressBar(double total) => _total = total;

    public ProgressBar WithLabel(string label) { _label = label; return this; }
    public ProgressBar WithWidth(int width)    { _width = width; return this; }
    public ProgressBar WithFillChar(char c)    { _fill  = c;     return this; }
    public ProgressBar WithEmptyChar(char c)   { _empty = c;     return this; }

    public void Advance(double amount) => SetValue(_current + amount);

    public void SetValue(double value)
    {
        _current = Math.Clamp(value, 0, _total);
        Render();
    }

    private void Render()
    {
        double pct      = _total > 0 ? _current / _total : 0;
        int    filled   = (int)Math.Round(pct * _width);
        int    unfilled = _width - filled;

        var bar   = _fillStyle.Apply(new string(_fill, filled))
                  + _emptyStyle.Apply(new string(_empty, unfilled));
        var label = string.IsNullOrEmpty(_label) ? "" : $"{_label} ";
        var pctStr= $" {(int)(pct * 100),3}%";

        Console.Write($"\r{label}[{bar}]{pctStr}");
    }

    public void Dispose()
    {
        if (!_disposed) { Console.WriteLine(); _disposed = true; }
    }
}
