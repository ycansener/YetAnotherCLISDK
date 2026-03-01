using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class SelectionMenu
{
    private record Option(string Label, Color? Highlight);

    private readonly string     _prompt;
    private readonly List<Option> _options = [];
    private Style _promptStyle   = new() { Bold = true };
    private Style _selectedStyle = new() { Foreground = Color.Cyan, Bold = true };
    private Style _normalStyle   = Style.Default;
    private string _pointer      = "❯";

    public SelectionMenu(string prompt) => _prompt = prompt;

    public SelectionMenu AddOption(string label, Color? highlight = null)
    {
        _options.Add(new Option(label, highlight));
        return this;
    }

    public SelectionMenu WithPointer(string pointer) { _pointer = pointer; return this; }

    public string? Show()
    {
        if (_options.Count == 0) return null;

        int selected = 0;
        Console.CursorVisible = false;

        try
        {
            Render(selected);

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.UpArrow)
                    selected = (selected - 1 + _options.Count) % _options.Count;
                else if (key.Key == ConsoleKey.DownArrow)
                    selected = (selected + 1) % _options.Count;
                else if (key.Key == ConsoleKey.Enter)
                    break;
                else if (key.Key == ConsoleKey.Escape)
                {
                    ClearRendered(selected);
                    Console.CursorVisible = true;
                    return null;
                }

                ClearRendered(selected);
                Render(selected);
            }

            ClearRendered(selected);
            var chosen = _options[selected].Label;
            Console.WriteLine(_promptStyle.Apply(Markup.Parse(_prompt)) + " "
                + _selectedStyle.Apply(Markup.Strip(chosen)));
            return Markup.Strip(chosen);
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }

    private void Render(int selected)
    {
        Console.WriteLine(_promptStyle.Apply(Markup.Parse(_prompt)));
        for (int i = 0; i < _options.Count; i++)
        {
            var opt    = _options[i];
            var label  = Markup.Parse(opt.Label);
            var plain  = Markup.Strip(opt.Label);

            if (i == selected)
            {
                var ptr = new Style { Foreground = Color.Cyan }.Apply(_pointer);
                Console.WriteLine($"  {ptr} {_selectedStyle.Apply(plain)}");
            }
            else
            {
                Console.WriteLine($"    {_normalStyle.Apply(label)}");
            }
        }
    }

    private void ClearRendered(int selected)
    {
        int lines = _options.Count + 1;
        for (int i = 0; i < lines; i++)
        {
            Console.Write(Ansi.Up());
            Console.Write(Ansi.ClearLine());
        }
    }
}
