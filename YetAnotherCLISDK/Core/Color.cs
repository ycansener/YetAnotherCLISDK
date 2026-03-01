namespace YetAnotherCLISDK.Core;

public readonly struct Color
{
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }

    public Color(byte r, byte g, byte b) => (R, G, B) = (r, g, b);

    public static Color FromHex(string hex)
    {
        hex = hex.TrimStart('#');
        return new Color(
            Convert.ToByte(hex[..2], 16),
            Convert.ToByte(hex[2..4], 16),
            Convert.ToByte(hex[4..6], 16));
    }

    public string ToFg() => Ansi.Fg(R, G, B);
    public string ToBg() => Ansi.Bg(R, G, B);

    // Predefined colors
    public static readonly Color Black        = new(0,   0,   0);
    public static readonly Color DarkRed      = new(139, 0,   0);
    public static readonly Color DarkGreen    = new(0,   100, 0);
    public static readonly Color DarkYellow   = new(128, 128, 0);
    public static readonly Color DarkBlue     = new(0,   0,   139);
    public static readonly Color DarkMagenta  = new(139, 0,   139);
    public static readonly Color DarkCyan     = new(0,   139, 139);
    public static readonly Color Gray         = new(128, 128, 128);
    public static readonly Color DarkGray     = new(64,  64,  64);
    public static readonly Color Red          = new(205, 49,  49);
    public static readonly Color Green        = new(13,  188, 121);
    public static readonly Color Yellow       = new(229, 229, 16);
    public static readonly Color Blue         = new(36,  114, 200);
    public static readonly Color Magenta      = new(188, 63,  188);
    public static readonly Color Cyan         = new(17,  168, 205);
    public static readonly Color White        = new(229, 229, 229);
    public static readonly Color Orange       = new(255, 165, 0);
    public static readonly Color Pink         = new(255, 105, 180);
    public static readonly Color Purple       = new(128, 0,   128);
    public static readonly Color Gold         = new(255, 215, 0);
    public static readonly Color Teal         = new(0,   128, 128);
    public static readonly Color Lime         = new(0,   255, 0);
    public static readonly Color Coral        = new(255, 127, 80);
    public static readonly Color SkyBlue      = new(135, 206, 235);
    public static readonly Color MediumPurple = new(147, 112, 219);

    private static readonly Dictionary<string, Color> _named = new(StringComparer.OrdinalIgnoreCase)
    {
        ["black"]        = Black,
        ["darkred"]      = DarkRed,
        ["darkgreen"]    = DarkGreen,
        ["darkyellow"]   = DarkYellow,
        ["darkblue"]     = DarkBlue,
        ["darkmagenta"]  = DarkMagenta,
        ["darkcyan"]     = DarkCyan,
        ["gray"]         = Gray,
        ["grey"]         = Gray,
        ["darkgray"]     = DarkGray,
        ["darkgrey"]     = DarkGray,
        ["red"]          = Red,
        ["green"]        = Green,
        ["yellow"]       = Yellow,
        ["blue"]         = Blue,
        ["magenta"]      = Magenta,
        ["cyan"]         = Cyan,
        ["white"]        = White,
        ["orange"]       = Orange,
        ["pink"]         = Pink,
        ["purple"]       = Purple,
        ["gold"]         = Gold,
        ["teal"]         = Teal,
        ["lime"]         = Lime,
        ["coral"]        = Coral,
        ["skyblue"]      = SkyBlue,
        ["mediumpurple"] = MediumPurple,
    };

    public static bool TryParse(string name, out Color color) =>
        _named.TryGetValue(name, out color);
}
