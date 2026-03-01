using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class MultiSelectMenu
{
    private readonly string       _prompt;
    private readonly List<string> _options    = [];
    private Style _promptStyle   = new() { Bold = true };
    private Style _selectedStyle = new() { Foreground = Color.Cyan, Bold = true };
    private Style _checkStyle    = new() { Foreground = Color.Green };
    private string _checked   = "◉";
    private string _unchecked = "○";
    private string _pointer   = "❯";

    public MultiSelectMenu(string prompt) => _prompt = prompt;

    public MultiSelectMenu AddOption(string label)  { _options.Add(label); return this; }
    public MultiSelectMenu WithCheckedChar(string c)   { _checked = c; return this; }
    public MultiSelectMenu WithUncheckedChar(string c) { _unchecked = c; return this; }

    public string[] Show()
    {
        if (_options.Count == 0) return [];

        int cursor  = 0;
        var ticked  = new HashSet<int>();
        Console.CursorVisible = false;

        try
        {
            Render(cursor, ticked);

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.UpArrow)
                    cursor = (cursor - 1 + _options.Count) % _options.Count;
                else if (key.Key == ConsoleKey.DownArrow)
                    cursor = (cursor + 1) % _options.Count;
                else if (key.Key == ConsoleKey.Spacebar)
                {
                    if (!ticked.Remove(cursor)) ticked.Add(cursor);
                }
                else if (key.Key == ConsoleKey.Enter)
                    break;
                else if (key.Key == ConsoleKey.Escape)
                {
                    ClearRendered();
                    Console.CursorVisible = true;
                    return [];
                }

                ClearRendered();
                Render(cursor, ticked);
            }

            ClearRendered();
            var selected = ticked.OrderBy(x => x).Select(x => Markup.Strip(_options[x])).ToArray();
            Console.WriteLine(_promptStyle.Apply(Markup.Parse(_prompt)));
            foreach (var s in selected)
                Console.WriteLine("  " + _checkStyle.Apply(_checked) + " " + s);
            return selected;
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }

    private void Render(int cursor, HashSet<int> ticked)
    {
        Console.WriteLine(_promptStyle.Apply(Markup.Parse(_prompt)));
        for (int i = 0; i < _options.Count; i++)
        {
            var label   = Markup.Strip(_options[i]);
            var check   = ticked.Contains(i)
                ? _checkStyle.Apply(_checked)
                : new Style { Foreground = Color.Gray }.Apply(_unchecked);

            if (i == cursor)
            {
                var ptr = new Style { Foreground = Color.Cyan }.Apply(_pointer);
                Console.WriteLine($"  {ptr} {check} {_selectedStyle.Apply(label)}");
            }
            else
            {
                Console.WriteLine($"    {check} {label}");
            }
        }
    }

    private void ClearRendered()
    {
        int lines = _options.Count + 1;
        for (int i = 0; i < lines; i++)
        {
            Console.Write(Ansi.Up());
            Console.Write(Ansi.ClearLine());
        }
    }
}
