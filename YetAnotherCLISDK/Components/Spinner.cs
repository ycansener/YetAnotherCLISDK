using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class Spinner : IDisposable
{
    public enum SpinnerStyle { Dots, Line, Arrow, Clock, Bounce, Pulse }

    private static readonly string[][] Frames =
    [
        ["⠋","⠙","⠹","⠸","⠼","⠴","⠦","⠧","⠇","⠏"],   // Dots
        ["-","\\","|","/"],                                // Line
        ["←","↖","↑","↗","→","↘","↓","↙"],              // Arrow
        ["🕛","🕐","🕑","🕒","🕓","🕔","🕕","🕖","🕗","🕘","🕙","🕚"], // Clock
        ["⠁","⠂","⠄","⡀","⢀","⠠","⠐","⠈"],             // Bounce
        ["▁","▂","▃","▄","▅","▆","▇","█","▇","▆","▅","▄","▃","▂","▁"], // Pulse
    ];

    private readonly string[] _frames;
    private readonly string?  _label;
    private readonly Style    _style;
    private readonly int      _intervalMs;
    private readonly CancellationTokenSource _cts = new();
    private readonly Task _task;
    private bool _disposed;

    public Spinner(SpinnerStyle style = SpinnerStyle.Dots, string? label = null,
                   Style? frameStyle = null, int intervalMs = 80)
    {
        _frames     = Frames[(int)style];
        _label      = label;
        _style      = frameStyle ?? new Style { Foreground = Color.Cyan };
        _intervalMs = intervalMs;

        Console.Write(Ansi.HideCursor());
        _task = Task.Run(Spin);
    }

    private async Task Spin()
    {
        int i = 0;
        while (!_cts.Token.IsCancellationRequested)
        {
            var frame = _style.Apply(_frames[i % _frames.Length]);
            var text  = _label is null ? frame : $"{frame} {_label}";
            Console.Write($"\r{text} ");
            i++;
            try { await Task.Delay(_intervalMs, _cts.Token); }
            catch (OperationCanceledException) { break; }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _cts.Cancel();
        _task.Wait();
        Console.Write(Ansi.ClearLine());
        Console.Write(Ansi.ShowCursor());
    }
}
