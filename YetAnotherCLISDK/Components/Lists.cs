using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class BulletList : IRenderable
{
    private record Item(string Label, BulletList? Children);

    private readonly List<Item> _items  = [];
    private readonly string?    _title;
    private string              _bullet = "●";
    private int                 _indent = 2;

    public BulletList(string? title = null) => _title = title;

    public BulletList WithBullet(string bullet) { _bullet = bullet; return this; }
    public BulletList WithIndent(int indent)    { _indent = indent; return this; }

    public BulletList AddItem(string label, BulletList? nested = null)
    {
        _items.Add(new Item(label, nested));
        return this;
    }

    public IEnumerable<string> GetLines() => GetLines(0);

    internal IEnumerable<string> GetLines(int depth)
    {
        var lines = new List<string>();
        var pad   = new string(' ', depth * _indent);

        if (_title is not null && depth == 0)
        {
            lines.Add(new Style { Bold = true }.Apply(Markup.Parse(_title)));
        }

        foreach (var item in _items)
        {
            var bullet  = new Style { Foreground = Color.Cyan }.Apply(_bullet);
            var label   = Markup.Parse(item.Label);
            lines.Add($"{pad} {bullet} {label}");

            if (item.Children is not null)
                lines.AddRange(item.Children.GetLines(depth + 1));
        }

        return lines;
    }
}

public sealed class OrderedList : IRenderable
{
    private readonly List<string> _items = [];
    private readonly string?      _title;
    private Style _numberStyle = new() { Foreground = Color.Yellow, Bold = true };

    public OrderedList(string? title = null) => _title = title;

    public OrderedList AddItem(string label)
    {
        _items.Add(label);
        return this;
    }

    public OrderedList WithNumberStyle(Style style) { _numberStyle = style; return this; }

    public IEnumerable<string> GetLines()
    {
        var lines = new List<string>();

        if (_title is not null)
            lines.Add(new Style { Bold = true }.Apply(Markup.Parse(_title)));

        int width = _items.Count.ToString().Length;

        for (int i = 0; i < _items.Count; i++)
        {
            var num   = _numberStyle.Apply($"{i + 1}.".PadLeft(width + 1));
            var label = Markup.Parse(_items[i]);
            lines.Add($" {num} {label}");
        }

        return lines;
    }
}
