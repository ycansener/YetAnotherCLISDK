namespace YetAnotherCLISDK.Core;

internal static class Ansi
{
    public const string Reset         = "\x1b[0m";
    public const string Bold          = "\x1b[1m";
    public const string Dim           = "\x1b[2m";
    public const string Italic        = "\x1b[3m";
    public const string Underline     = "\x1b[4m";
    public const string Blink         = "\x1b[5m";
    public const string Inverse       = "\x1b[7m";
    public const string Strikethrough = "\x1b[9m";

    public static string Fg(int r, int g, int b) => $"\x1b[38;2;{r};{g};{b}m";
    public static string Bg(int r, int g, int b) => $"\x1b[48;2;{r};{g};{b}m";

    public static string Up(int n = 1)   => $"\x1b[{n}A";
    public static string ClearLine()     => "\x1b[2K\r";
    public static string HideCursor()    => "\x1b[?25l";
    public static string ShowCursor()    => "\x1b[?25h";
}
