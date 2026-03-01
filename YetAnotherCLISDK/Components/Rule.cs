using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class Rule : IRenderable
{
    private readonly string? _title;
    private char  _char  = '─';
    private Style _style = new() { Foreground = Color.Gray };
    private int   _width = 0;

    public Rule(string? title = null) => _title = title;

    public Rule WithChar(char ch)      { _char  = ch;    return this; }
    public Rule WithStyle(Style style) { _style = style; return this; }
    public Rule WithWidth(int width)   { _width = width; return this; }

    public IEnumerable<string> GetLines()
    {
        int w = _width > 0 ? _width : Cli.Width;

        if (_title is null)
        {
            yield return _style.Apply(new string(_char, w));
            yield break;
        }

        var plain  = Markup.Strip(_title);
        var parsed = Markup.Parse(_title);
        int tlen   = plain.Length + 2;
        int rem    = Math.Max(0, w - tlen);
        int left   = rem / 2;
        int right  = rem - left;

        var lh = _style.Apply(new string(_char, left));
        var rh = _style.Apply(new string(_char, right));
        yield return $"{lh} {parsed} {rh}";
    }
}
