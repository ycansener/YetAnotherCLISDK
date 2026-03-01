using System.Text;
using System.Text.RegularExpressions;

namespace YetAnotherCLISDK.Core;

public static class Markup
{
    // Strips ANSI escape codes to get the visual (printable) length.
    private static readonly Regex AnsiPattern = new(@"\x1b\[[^m]*m", RegexOptions.Compiled);

    public static int VisualLength(string text) =>
        AnsiPattern.Replace(text, "").Length;

    public static string PadRight(string text, int totalVisualWidth)
    {
        int pad = totalVisualWidth - VisualLength(text);
        return pad > 0 ? text + new string(' ', pad) : text;
    }

    public static string PadLeft(string text, int totalVisualWidth)
    {
        int pad = totalVisualWidth - VisualLength(text);
        return pad > 0 ? new string(' ', pad) + text : text;
    }

    public static string Center(string text, int totalVisualWidth)
    {
        int pad = totalVisualWidth - VisualLength(text);
        if (pad <= 0) return text;
        int left = pad / 2;
        int right = pad - left;
        return new string(' ', left) + text + new string(' ', right);
    }

    /// <summary>Parse markup tags into ANSI-escaped string.</summary>
    public static string Parse(string markup)
    {
        var result = new StringBuilder();
        var stack = new Stack<Style>();
        stack.Push(Style.Default);

        int i = 0;
        while (i < markup.Length)
        {
            if (markup[i] == '[')
            {
                // Check for escaped bracket [[
                if (i + 1 < markup.Length && markup[i + 1] == '[')
                {
                    result.Append('[');
                    i += 2;
                    continue;
                }

                int end = markup.IndexOf(']', i + 1);
                if (end == -1) { result.Append(markup[i++]); continue; }

                var tag = markup[(i + 1)..end];
                i = end + 1;

                if (tag.StartsWith('/'))
                {
                    if (stack.Count > 1) stack.Pop();
                    result.Append(Ansi.Reset);
                    if (stack.Count > 0)
                        result.Append(stack.Peek().GetPrefix());
                }
                else
                {
                    var style = ParseTag(tag, stack.Peek());
                    stack.Push(style);
                    result.Append(style.GetPrefix());
                }
            }
            else
            {
                result.Append(markup[i++]);
            }
        }

        if (stack.Count > 1)
            result.Append(Ansi.Reset);

        return result.ToString();
    }

    /// <summary>Strip all markup tags, returning plain text.</summary>
    public static string Strip(string markup)
    {
        var sb = new StringBuilder();
        int i = 0;
        while (i < markup.Length)
        {
            if (markup[i] == '[')
            {
                if (i + 1 < markup.Length && markup[i + 1] == '[')
                {
                    sb.Append('[');
                    i += 2;
                    continue;
                }
                int end = markup.IndexOf(']', i + 1);
                if (end == -1) { sb.Append(markup[i++]); continue; }
                i = end + 1;
            }
            else
            {
                sb.Append(markup[i++]);
            }
        }
        return sb.ToString();
    }

    private static Style ParseTag(string tag, Style current)
    {
        var parts = tag.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Color? fg   = current.Foreground;
        Color? bg   = current.Background;
        bool bold   = current.Bold;
        bool italic = current.Italic;
        bool under  = current.Underline;
        bool strike = current.Strikethrough;
        bool dim    = current.Dim;
        bool blink  = current.Blink;
        bool inv    = current.Inverse;

        bool onNext = false;

        foreach (var raw in parts)
        {
            var p = raw.ToLowerInvariant();

            if (p == "on") { onNext = true; continue; }

            Color c;
            bool resolved = true;

            switch (p)
            {
                case "bold":                          bold   = true; break;
                case "italic":                        italic = true; break;
                case "underline" or "u":              under  = true; break;
                case "strikethrough" or "strike" or "s": strike = true; break;
                case "dim":                           dim    = true; break;
                case "blink":                         blink  = true; break;
                case "inverse" or "reverse" or "rev": inv    = true; break;
                case "not bold":                      bold   = false; break;
                case "not italic":                    italic = false; break;
                default:
                    if (p.StartsWith('#') && p.Length == 7)
                        c = Color.FromHex(p);
                    else if (p.StartsWith("rgb(") && p.EndsWith(")"))
                    {
                        var nums = p[4..^1].Split(',');
                        c = new Color(byte.Parse(nums[0].Trim()), byte.Parse(nums[1].Trim()), byte.Parse(nums[2].Trim()));
                    }
                    else if (!Color.TryParse(p, out c))
                    {
                        resolved = false;
                        break;
                    }

                    if (resolved)
                    {
                        if (onNext) { bg = c; onNext = false; }
                        else fg = c;
                    }
                    break;
            }
        }

        return new Style
        {
            Foreground    = fg,
            Background    = bg,
            Bold          = bold,
            Italic        = italic,
            Underline     = under,
            Strikethrough = strike,
            Dim           = dim,
            Blink         = blink,
            Inverse       = inv,
        };
    }
}
