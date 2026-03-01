namespace YetAnotherCLISDK.Core;

public sealed class BorderStyle
{
    public char TopLeft     { get; init; }
    public char TopRight    { get; init; }
    public char BottomLeft  { get; init; }
    public char BottomRight { get; init; }
    public char Horizontal  { get; init; }
    public char Vertical    { get; init; }
    public char TopT        { get; init; }
    public char BottomT     { get; init; }
    public char LeftT       { get; init; }
    public char RightT      { get; init; }
    public char Cross       { get; init; }

    public static readonly BorderStyle None = new()
    {
        TopLeft = ' ', TopRight = ' ', BottomLeft = ' ', BottomRight = ' ',
        Horizontal = ' ', Vertical = ' ',
        TopT = ' ', BottomT = ' ', LeftT = ' ', RightT = ' ', Cross = ' '
    };

    public static readonly BorderStyle Single = new()
    {
        TopLeft = '┌', TopRight = '┐', BottomLeft = '└', BottomRight = '┘',
        Horizontal = '─', Vertical = '│',
        TopT = '┬', BottomT = '┴', LeftT = '├', RightT = '┤', Cross = '┼'
    };

    public static readonly BorderStyle Double = new()
    {
        TopLeft = '╔', TopRight = '╗', BottomLeft = '╚', BottomRight = '╝',
        Horizontal = '═', Vertical = '║',
        TopT = '╦', BottomT = '╩', LeftT = '╠', RightT = '╣', Cross = '╬'
    };

    public static readonly BorderStyle Rounded = new()
    {
        TopLeft = '╭', TopRight = '╮', BottomLeft = '╰', BottomRight = '╯',
        Horizontal = '─', Vertical = '│',
        TopT = '┬', BottomT = '┴', LeftT = '├', RightT = '┤', Cross = '┼'
    };

    public static readonly BorderStyle Heavy = new()
    {
        TopLeft = '┏', TopRight = '┓', BottomLeft = '┗', BottomRight = '┛',
        Horizontal = '━', Vertical = '┃',
        TopT = '┳', BottomT = '┻', LeftT = '┣', RightT = '┫', Cross = '╋'
    };

    public static readonly BorderStyle Ascii = new()
    {
        TopLeft = '+', TopRight = '+', BottomLeft = '+', BottomRight = '+',
        Horizontal = '-', Vertical = '|',
        TopT = '+', BottomT = '+', LeftT = '+', RightT = '+', Cross = '+'
    };

    public static readonly BorderStyle Dotted = new()
    {
        TopLeft = '·', TopRight = '·', BottomLeft = '·', BottomRight = '·',
        Horizontal = '·', Vertical = '·',
        TopT = '·', BottomT = '·', LeftT = '·', RightT = '·', Cross = '·'
    };
}
