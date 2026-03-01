using System.Runtime.InteropServices;
using System.Text;
using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK;

public static class Cli
{
    public static void Initialize()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding  = Encoding.UTF8;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var stdout = GetStdHandle(-11);
            if (GetConsoleMode(stdout, out uint mode))
                SetConsoleMode(stdout, mode | 0x0004); // ENABLE_VIRTUAL_TERMINAL_PROCESSING
        }
    }

    [DllImport("kernel32.dll")] private static extern IntPtr GetStdHandle(int h);
    [DllImport("kernel32.dll")] private static extern bool GetConsoleMode(IntPtr h, out uint mode);
    [DllImport("kernel32.dll")] private static extern bool SetConsoleMode(IntPtr h, uint mode);

    public static int Width  => Math.Max(Console.WindowWidth,  20);
    public static int Height => Math.Max(Console.WindowHeight, 10);

    public static void Write(string markup)    => Console.Write(Markup.Parse(markup));
    public static void WriteLine(string markup = "") =>
        Console.WriteLine(markup.Length == 0 ? "" : Markup.Parse(markup));

    public static void Render(IRenderable renderable)
    {
        foreach (var line in renderable.GetLines())
            Console.WriteLine(line);
    }

    public static void Clear() => Console.Clear();

    public static void WriteError(string message)   => WriteLine($"[bold red]✗ {EscapeBrackets(message)}[/]");
    public static void WriteSuccess(string message) => WriteLine($"[bold green]✓ {EscapeBrackets(message)}[/]");
    public static void WriteWarning(string message) => WriteLine($"[bold yellow]⚠ {EscapeBrackets(message)}[/]");
    public static void WriteInfo(string message)    => WriteLine($"[bold cyan]ℹ {EscapeBrackets(message)}[/]");

    private static string EscapeBrackets(string s) => s.Replace("[", "[[");
}
