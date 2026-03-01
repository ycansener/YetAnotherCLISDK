# YetAnotherCLISDK

A .NET 10 SDK for building beautiful, colourful command-line interfaces.  
Supports rich text markup, borders, tables, lists, diagrams, progress bars, spinners, and interactive selection menus — all with a fluent, chainable API.

---

## Table of Contents

- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Markup Language](#markup-language)
- [Colors](#colors)
- [Styles](#styles)
- [Cli — Writing Text](#cli--writing-text)
- [Rule — Horizontal Divider](#rule--horizontal-divider)
- [Panel — Bordered Box](#panel--bordered-box)
- [Table](#table)
- [BulletList](#bulletlist)
- [OrderedList](#orderedlist)
- [TreeView](#treeview)
- [BarChart](#barchart)
- [ProgressBar](#progressbar)
- [Spinner](#spinner)
- [SelectionMenu](#selectionmenu)
- [MultiSelectMenu](#multiselectmenu)
- [IRenderable — Custom Components](#irenderable--custom-components)

---

## Getting Started

Add a reference to the `YetAnotherCLISDK` class library project, then call `Cli.Initialize()` at the top of your program. This enables UTF-8 output and ANSI escape codes on Windows.

```csharp
using YetAnotherCLISDK;
using YetAnotherCLISDK.Components;
using YetAnotherCLISDK.Core;

Cli.Initialize();

Cli.WriteLine("[bold green]Hello, World![/]");
```

---

## Project Structure

```
YetAnotherCLISDK.slnx
├── YetAnotherCLISDK/            ← Class library (the SDK)
│   ├── Core/
│   │   ├── Ansi.cs              ANSI escape code constants
│   │   ├── Color.cs             RGB Color struct + named colours
│   │   ├── Style.cs             Composable text style
│   │   ├── Markup.cs            Rich markup parser
│   │   ├── BorderStyle.cs       Border character presets
│   │   └── Alignment.cs         Left / Center / Right enum
│   ├── Components/
│   │   ├── Panel.cs
│   │   ├── Table.cs
│   │   ├── Lists.cs             BulletList + OrderedList
│   │   ├── Rule.cs
│   │   ├── ProgressBar.cs
│   │   ├── Spinner.cs
│   │   ├── SelectionMenu.cs
│   │   ├── MultiSelectMenu.cs
│   │   ├── BarChart.cs
│   │   └── TreeView.cs
│   ├── IRenderable.cs
│   └── Cli.cs
└── YetAnotherCLISDK.Demo/       ← Console app demo
    └── Program.cs
```

---

## Markup Language

All text accepted by `Cli.Write`, `Cli.WriteLine`, and component constructors supports an inline markup syntax.

### Syntax

```
[tag]text[/]
```

Tags can be combined with spaces. The closing tag `[/]` (or `[/tagname]`) pops the current style, restoring the previous one. Styles stack — nested tags inherit the outer style.

### Text decorations

| Tag | Effect |
|---|---|
| `[bold]` | **Bold** |
| `[italic]` | *Italic* |
| `[underline]` or `[u]` | Underline |
| `[strikethrough]` or `[strike]` or `[s]` | ~~Strikethrough~~ |
| `[dim]` | Dimmed |
| `[blink]` | Blinking |
| `[inverse]` or `[reverse]` | Inverted fg/bg |

### Foreground colours

Use any named colour, a hex value, or an RGB function:

```csharp
Cli.WriteLine("[red]Red text[/]");
Cli.WriteLine("[#ff6b6b]Hex colour[/]");
Cli.WriteLine("[rgb(100,200,100)]RGB colour[/]");
```

### Background colours

Prefix the colour with `on`:

```csharp
Cli.WriteLine("[white on blue]White on blue background[/]");
Cli.WriteLine("[black on gold]Black on gold background[/]");
```

### Combining tags

```csharp
Cli.WriteLine("[bold red]Bold red[/]");
Cli.WriteLine("[bold italic underline cyan]All at once[/]");
```

### Nested tags

```csharp
Cli.WriteLine("[bold]This is bold and [red]this is bold+red[/] back to just bold[/]");
```

### Escaping brackets

Use `[[` to output a literal `[`:

```csharp
Cli.WriteLine("Use [[bold]] to make text bold.");
// Output: Use [bold] to make text bold.
```

---

## Colors

`Color` is an RGB struct with 25 predefined named colours and factory methods.

### Predefined colours

```
Black      DarkRed    DarkGreen  DarkYellow  DarkBlue   DarkMagenta
DarkCyan   Gray       DarkGray   Red         Green      Yellow
Blue       Magenta    Cyan       White       Orange     Pink
Purple     Gold       Teal       Lime        Coral      SkyBlue
MediumPurple
```

### Creating colours

```csharp
using YetAnotherCLISDK.Core;

Color red    = Color.Red;                    // predefined
Color custom = new Color(128, 0, 255);       // from RGB bytes
Color hex    = Color.FromHex("#9b59b6");     // from hex string
Color hex2   = Color.FromHex("9b59b6");      // # prefix optional
```

### Using in markup

Named colours are used directly in markup tags:

```csharp
Cli.WriteLine("[orange]Orange text[/]");
Cli.WriteLine("[mediumpurple on darkgray]Styled[/]");
```

---

## Styles

`Style` is an immutable record that combines colours and text decorations. Use it when you need to apply styles programmatically rather than via markup strings.

```csharp
using YetAnotherCLISDK.Core;

var style = new Style
{
    Foreground  = Color.Cyan,
    Background  = Color.DarkBlue,
    Bold        = true,
    Italic      = false,
    Underline   = true,
    Strikethrough = false,
    Dim         = false,
    Blink       = false,
    Inverse     = false,
};

// Apply to a plain string → returns ANSI-escaped string
string styled = style.Apply("Hello!");
Console.WriteLine(styled);
```

`Style.Default` is a no-op style (no colour, no decoration).

---

## Cli — Writing Text

`Cli` is the main static entry point.

### Initialise

Must be called once at program startup:

```csharp
Cli.Initialize();
```

Enables UTF-8 encoding and ANSI virtual terminal processing on Windows.

### Write and WriteLine

```csharp
Cli.Write("[cyan]No newline[/]");
Cli.WriteLine("[bold]With newline[/]");
Cli.WriteLine(); // blank line
```

### Status shortcuts

```csharp
Cli.WriteSuccess("Deployment complete.");
// ✓ Deployment complete.   (bold green)

Cli.WriteError("Connection refused.");
// ✗ Connection refused.    (bold red)

Cli.WriteWarning("Disk space low.");
// ⚠ Disk space low.        (bold yellow)

Cli.WriteInfo("Session expires in 5 min.");
// ℹ Session expires in 5 min.  (bold cyan)
```

### Render a component

```csharp
Cli.Render(new Panel("Hello!"));
```

Any `IRenderable` can be passed to `Cli.Render()`.

### Terminal dimensions

```csharp
int w = Cli.Width;   // Console.WindowWidth  (min 20)
int h = Cli.Height;  // Console.WindowHeight (min 10)
```

---

## Rule — Horizontal Divider

Renders a full-width horizontal line, optionally with a centred title.

```csharp
// Plain rule
Cli.Render(new Rule());

// Rule with title
Cli.Render(new Rule("[bold]Section Title[/]"));

// Custom character and colour
Cli.Render(new Rule("Results")
    .WithChar('═')
    .WithStyle(new Style { Foreground = Color.Cyan }));

// Fixed width
Cli.Render(new Rule("Fixed").WithWidth(40));
```

**Output:**
```
──────────────────── Section Title ─────────────────────
```

### API

| Method | Description |
|---|---|
| `new Rule(string? title)` | Optional centred title (markup supported) |
| `.WithChar(char)` | Fill character (default `─`) |
| `.WithStyle(Style)` | Style for the line and title |
| `.WithWidth(int)` | Fixed width; defaults to terminal width |

---

## Panel — Bordered Box

Draws a box around content with an optional title. Supports multi-line content and markup.

```csharp
// Minimal
Cli.Render(new Panel("Hello inside a panel!"));

// With title and border style
Cli.Render(new Panel("This is [bold cyan]important[/] information.")
    .WithTitle("[bold]Notice[/]")
    .WithBorder(BorderStyle.Rounded));

// Coloured border with padding
Cli.Render(new Panel("Content here.")
    .WithTitle("[bold yellow]Warning[/]")
    .WithBorder(BorderStyle.Double)
    .WithBorderStyle(new Style { Foreground = Color.Yellow })
    .WithPadding(2, 1));

// Multi-line content
Cli.Render(new Panel("Line one\nLine two\n[green]Line three[/]")
    .WithTitle("Multi-line")
    .WithBorder(BorderStyle.Heavy));

// Fixed width
Cli.Render(new Panel("Narrow panel.").WithWidth(30));
```

**Output (Rounded):**
```
╭─────────── Notice ────────────╮
│ This is important information. │
╰────────────────────────────────╯
```

### Border presets

| Preset | Characters |
|---|---|
| `BorderStyle.Single` | `┌ ─ ┐ │ └ ┘` |
| `BorderStyle.Double` | `╔ ═ ╗ ║ ╚ ╝` |
| `BorderStyle.Rounded` | `╭ ─ ╮ │ ╰ ╯` *(default)* |
| `BorderStyle.Heavy` | `┏ ━ ┓ ┃ ┗ ┛` |
| `BorderStyle.Ascii` | `+ - + | + +` |
| `BorderStyle.Dotted` | `· · · · · ·` |

### API

| Method | Description |
|---|---|
| `new Panel(string content)` | Content (markup + `\n` for newlines) |
| `.WithTitle(string)` | Centred title in top border (markup supported) |
| `.WithBorder(BorderStyle)` | Border character set |
| `.WithBorderStyle(Style)` | Colour/style of the border lines |
| `.WithTitleStyle(Style)` | Style of the title text (default: bold) |
| `.WithPadding(int x, int y)` | Horizontal and vertical inner padding |
| `.WithWidth(int)` | Force a fixed total width |

---

## Table

Renders tabular data with auto-sized columns, alignment, an optional title row, and full border/style control.

```csharp
// Basic table
Cli.Render(new Table()
    .AddColumn("Name")
    .AddColumn("Language")
    .AddColumn("Stars", Alignment.Right)
    .AddRow("Spectre.Console", "C#",     "8,200")
    .AddRow("Rich",            "Python", "47,000")
    .AddRow("Chalk",           "JS",     "21,000"));

// With title, rounded border, coloured cells
Cli.Render(new Table()
    .WithTitle("[bold]🏆 Leaderboard[/]")
    .WithBorder(BorderStyle.Rounded)
    .AddColumn("Rank",   Alignment.Center)
    .AddColumn("Player", Alignment.Left)
    .AddColumn("Score",  Alignment.Right)
    .AddRow("[gold]1[/]", "[cyan]Alice[/]",   "[yellow]9,850[/]")
    .AddRow("[silver]2[/]", "[cyan]Bob[/]",   "[yellow]7,420[/]")
    .AddRow("[coral]3[/]", "[cyan]Charlie[/]", "[yellow]5,100[/]"));

// Custom border colour
Cli.Render(new Table()
    .WithBorder(BorderStyle.Double)
    .WithBorderStyle(new Style { Foreground = Color.Cyan })
    .AddColumn("Key").AddColumn("Value")
    .AddRow("Version", "1.0.0")
    .AddRow("Author",  "Jane Doe"));
```

**Output:**
```
╭──────────── 🏆 Leaderboard ─────────────╮
│ Rank │ Player  │  Score │
├──────┼─────────┼────────┤
│  1   │ Alice   │  9,850 │
│  2   │ Bob     │  7,420 │
│  3   │ Charlie │  5,100 │
╰──────┴─────────┴────────╯
```

### API

| Method | Description |
|---|---|
| `new Table()` | Create an empty table |
| `.AddColumn(string header, Alignment)` | Add a column; default alignment is `Left` |
| `.AddRow(params string[] cells)` | Add a data row (markup supported in each cell) |
| `.WithTitle(string)` | Optional title row spanning all columns |
| `.WithBorder(BorderStyle)` | Border preset (default: `Single`) |
| `.WithBorderStyle(Style)` | Colour/style of border lines |
| `.WithHeaderStyle(Style)` | Style of header text (default: bold) |

### Alignment values

```csharp
Alignment.Left    // default
Alignment.Center
Alignment.Right
```

---

## BulletList

An unordered list with optional nesting and markup in items.

```csharp
// Simple list
Cli.Render(new BulletList()
    .AddItem("First item")
    .AddItem("Second item")
    .AddItem("Third item"));

// With title and colours
Cli.Render(new BulletList("[bold]Features[/]")
    .AddItem("[green]Fast[/]")
    .AddItem("[cyan]Colourful[/]")
    .AddItem("[yellow]Easy to use[/]"));

// Nested lists
Cli.Render(new BulletList("[bold]Tech Stack[/]")
    .AddItem("[cyan]Backend[/]", new BulletList()
        .AddItem("[blue].NET 10[/]")
        .AddItem("[green]PostgreSQL[/]"))
    .AddItem("[cyan]Frontend[/]", new BulletList()
        .AddItem("[yellow]React[/]")
        .AddItem("[blue]TypeScript[/]")));

// Custom bullet character
Cli.Render(new BulletList()
    .WithBullet("→")
    .AddItem("Step one")
    .AddItem("Step two"));
```

**Output:**
```
Features
 ● Fast
 ● Colourful
 ● Easy to use
```

### API

| Method | Description |
|---|---|
| `new BulletList(string? title)` | Optional bold title |
| `.AddItem(string label, BulletList? nested)` | Add item, with optional nested list |
| `.WithBullet(string)` | Bullet character (default `●`) |
| `.WithIndent(int)` | Spaces per nesting level (default `2`) |

---

## OrderedList

A numbered list with automatic numbering and markup in items.

```csharp
// Basic numbered list
Cli.Render(new OrderedList()
    .AddItem("Clone the repository")
    .AddItem("Run dotnet restore")
    .AddItem("Run dotnet run"));

// With title and styled items
Cli.Render(new OrderedList("[bold]Release Steps[/]")
    .AddItem("Bump [yellow]version number[/] in csproj")
    .AddItem("Run [cyan]dotnet pack[/]")
    .AddItem("Push to [magenta]NuGet[/]"));

// Custom number style
Cli.Render(new OrderedList("Steps")
    .WithNumberStyle(new Style { Foreground = Color.Magenta, Bold = true })
    .AddItem("First")
    .AddItem("Second")
    .AddItem("Third"));
```

**Output:**
```
Release Steps
 1. Bump version number in csproj
 2. Run dotnet pack
 3. Push to NuGet
```

### API

| Method | Description |
|---|---|
| `new OrderedList(string? title)` | Optional bold title |
| `.AddItem(string label)` | Add a numbered item (markup supported) |
| `.WithNumberStyle(Style)` | Style for the number prefix (default: bold yellow) |

---

## TreeView

Renders a tree structure with guide connectors.

```csharp
// File system tree
Cli.Render(new TreeView("src/")
    .AddNode("Controllers/", t => t
        .AddNode("HomeController.cs")
        .AddNode("ApiController.cs"))
    .AddNode("Models/", t => t
        .AddNode("User.cs")
        .AddNode("Product.cs"))
    .AddNode("Program.cs")
    .AddNode("appsettings.json"));

// With markup
Cli.Render(new TreeView("[bold cyan]MyProject/[/]")
    .AddNode("[yellow]src/[/]", t => t
        .AddNode("[green]App.cs[/]")
        .AddNode("[green]Utils.cs[/]"))
    .AddNode("[yellow]tests/[/]", t => t
        .AddNode("App.Tests.cs"))
    .AddNode("[gray]README.md[/]"));
```

**Output:**
```
src/
├── Controllers/
│   ├── HomeController.cs
│   └── ApiController.cs
├── Models/
│   ├── User.cs
│   └── Product.cs
├── Program.cs
└── appsettings.json
```

### API

| Method | Description |
|---|---|
| `new TreeView(string rootLabel)` | Root node label (markup supported) |
| `.AddNode(string label, Action<TreeView>? configure)` | Add a child node; optionally configure its children |

---

## BarChart

Renders a horizontal bar chart, scaled to the terminal width.

```csharp
// Basic chart
Cli.Render(new BarChart()
    .AddBar("Jan", 42)
    .AddBar("Feb", 78)
    .AddBar("Mar", 55));

// With title and per-bar colours
Cli.Render(new BarChart()
    .WithTitle("[bold]📊 Monthly Sales[/]")
    .AddBar("January",  42_000, Color.Blue)
    .AddBar("February", 78_500, Color.Cyan)
    .AddBar("March",    55_200, Color.Green)
    .AddBar("April",    91_100, Color.Yellow)
    .AddBar("May",      67_800, Color.Orange));

// Fixed bar width and default colour
Cli.Render(new BarChart()
    .WithBarWidth(30)
    .WithDefaultColor(Color.Magenta)
    .AddBar("Alpha", 60)
    .AddBar("Beta",  80)
    .AddBar("Gamma", 45));
```

**Output:**
```
📊 Monthly Sales
  January  │████████████░░░░░░░░░░░░░░░░░░ 42000
  February │████████████████████████░░░░░░ 78500
  March    │█████████████████░░░░░░░░░░░░░ 55200
```

### API

| Method | Description |
|---|---|
| `new BarChart()` | Create an empty chart |
| `.AddBar(string label, double value, Color? color)` | Add a bar; colour is optional |
| `.WithTitle(string)` | Optional title above the chart |
| `.WithBarWidth(int)` | Fixed bar width; defaults to auto (terminal width minus labels) |
| `.WithDefaultColor(Color)` | Default bar colour when none specified (default: `Cyan`) |

---

## ProgressBar

An in-place progress bar that re-renders on the same line. Implements `IDisposable` — wrapping in `using` prints a final newline on completion.

```csharp
// Basic usage
using var pb = new ProgressBar(100);
for (int i = 0; i <= 100; i++)
{
    pb.SetValue(i);
    Thread.Sleep(30);
}
// Dispose prints the final newline

// With label
using (var pb = new ProgressBar(200).WithLabel("Downloading"))
{
    for (int i = 0; i <= 200; i++)
    {
        pb.Advance(1);   // increment by amount
        Thread.Sleep(10);
    }
}

// Custom characters and width
using var pb = new ProgressBar(50)
    .WithLabel("Installing")
    .WithWidth(30)
    .WithFillChar('▓')
    .WithEmptyChar('░');

for (int i = 0; i <= 50; i++)
{
    pb.SetValue(i);
    Thread.Sleep(40);
}
```

**Output:**
```
Downloading [████████████████░░░░░░░░░░░░░░░░░░░░░░]  45%
```

### API

| Method | Description |
|---|---|
| `new ProgressBar(double total)` | Total value (e.g. 100) |
| `.WithLabel(string)` | Label printed before the bar (markup supported) |
| `.WithWidth(int)` | Bar character width (default `40`) |
| `.WithFillChar(char)` | Filled portion character (default `█`) |
| `.WithEmptyChar(char)` | Empty portion character (default `░`) |
| `.SetValue(double)` | Set absolute value and redraw |
| `.Advance(double)` | Increment by amount and redraw |
| `.Dispose()` | Print newline (called automatically by `using`) |

---

## Spinner

An animated spinner running on a background thread. Implements `IDisposable` — disposing stops the animation and clears the line.

```csharp
// Basic usage — spins while work is done
using (var spinner = new Spinner())
{
    DoSomethingLong();
}

// With label
using (var spinner = new Spinner(label: "Fetching data..."))
{
    await FetchDataAsync();
}

// Specific style
using (var spinner = new Spinner(Spinner.SpinnerStyle.Dots, "Loading"))
{
    Thread.Sleep(3000);
}

// Custom style and speed
using var spinner = new Spinner(
    style:       Spinner.SpinnerStyle.Arrow,
    label:       "Processing",
    frameStyle:  new Style { Foreground = Color.Magenta },
    intervalMs:  60);

Thread.Sleep(2000);
```

### Spinner styles

| Style | Frames |
|---|---|
| `Dots` *(default)* | `⠋ ⠙ ⠹ ⠸ ⠼ ⠴ ⠦ ⠧ ⠇ ⠏` |
| `Line` | `- \ \| /` |
| `Arrow` | `← ↖ ↑ ↗ → ↘ ↓ ↙` |
| `Clock` | `🕛 🕐 🕑 … 🕚` |
| `Bounce` | `⠁ ⠂ ⠄ ⡀ ⢀ ⠠ ⠐ ⠈` |
| `Pulse` | `▁ ▂ ▃ ▄ ▅ ▆ ▇ █ ▇ ▆ …` |

### API

| Parameter / Method | Description |
|---|---|
| `new Spinner(SpinnerStyle, string? label, Style? frameStyle, int intervalMs)` | All parameters optional |
| `.Dispose()` | Stop animation, clear line, show cursor |

---

## SelectionMenu

An interactive single-select prompt. Arrow keys navigate, **Enter** confirms, **Escape** cancels (returns `null`).

```csharp
// Basic menu
var choice = new SelectionMenu("Pick an environment:")
    .AddOption("Development")
    .AddOption("Staging")
    .AddOption("Production")
    .Show();

if (choice is not null)
    Cli.WriteSuccess($"Deploying to: {choice}");

// With markup in options
var lang = new SelectionMenu("Favourite language?")
    .AddOption("[blue]C#[/]")
    .AddOption("[yellow]JavaScript[/]")
    .AddOption("[green]Python[/]")
    .AddOption("[orange]Rust[/]")
    .Show();

// Custom pointer
var plan = new SelectionMenu("Choose a plan:")
    .WithPointer("▶")
    .AddOption("Free")
    .AddOption("Pro")
    .AddOption("Enterprise")
    .Show();
```

**Runtime appearance:**
```
Pick an environment:
  ❯ Development        ← highlighted, bold cyan
    Staging
    Production
```

### API

| Method | Description |
|---|---|
| `new SelectionMenu(string prompt)` | Prompt text (markup supported) |
| `.AddOption(string label, Color? highlight)` | Add an option (markup supported) |
| `.WithPointer(string)` | Pointer character (default `❯`) |
| `.Show()` | Display and return selected plain text, or `null` if escaped |

---

## MultiSelectMenu

An interactive multi-select prompt. Arrow keys navigate, **Space** toggles, **Enter** confirms, **Escape** cancels (returns empty array).

```csharp
// Basic multi-select
var selected = new MultiSelectMenu("Select features to enable:")
    .AddOption("Dark mode")
    .AddOption("Notifications")
    .AddOption("Auto-updates")
    .AddOption("Analytics")
    .Show();

foreach (var s in selected)
    Cli.WriteSuccess($"Enabled: {s}");

// Custom checkbox characters
var tools = new MultiSelectMenu("Which tools do you use?")
    .WithCheckedChar("✔")
    .WithUncheckedChar("✘")
    .AddOption("Docker")
    .AddOption("Kubernetes")
    .AddOption("Terraform")
    .AddOption("Ansible")
    .Show();

Cli.WriteInfo($"{tools.Length} tools selected.");
```

**Runtime appearance:**
```
Select features to enable:
  ❯ ◉ Dark mode          ← cursor + checked
    ○ Notifications
    ◉ Auto-updates        ← checked
    ○ Analytics
```

### API

| Method | Description |
|---|---|
| `new MultiSelectMenu(string prompt)` | Prompt text (markup supported) |
| `.AddOption(string label)` | Add an option |
| `.WithCheckedChar(string)` | Checked indicator (default `◉`) |
| `.WithUncheckedChar(string)` | Unchecked indicator (default `○`) |
| `.Show()` | Display and return `string[]` of selected plain-text labels |

---

## IRenderable — Custom Components

Any component that implements `IRenderable` can be passed to `Cli.Render()`.

```csharp
public interface IRenderable
{
    IEnumerable<string> GetLines();
}
```

### Example: custom banner component

```csharp
using YetAnotherCLISDK;
using YetAnotherCLISDK.Core;

public class Banner : IRenderable
{
    private readonly string _text;
    private readonly Color  _color;

    public Banner(string text, Color color) => (_text, _color) = (text, color);

    public IEnumerable<string> GetLines()
    {
        var style = new Style { Foreground = _color, Bold = true };
        var line  = new string('─', _text.Length + 4);

        yield return style.Apply($"┌{line}┐");
        yield return style.Apply($"│  {_text}  │");
        yield return style.Apply($"└{line}┘");
    }
}

// Usage
Cli.Render(new Banner("Hello, World!", Color.Cyan));
```

---

## Quick Reference

```csharp
Cli.Initialize();                            // call once at startup

// Text
Cli.Write("[bold red]text[/]");              // no newline
Cli.WriteLine("[italic cyan]text[/]");       // with newline
Cli.WriteLine();                             // blank line
Cli.WriteSuccess("msg");                     // ✓ bold green
Cli.WriteError("msg");                       // ✗ bold red
Cli.WriteWarning("msg");                     // ⚠ bold yellow
Cli.WriteInfo("msg");                        // ℹ bold cyan

// Components (all via Cli.Render)
Cli.Render(new Rule("Title"));
Cli.Render(new Panel("content").WithTitle("T").WithBorder(BorderStyle.Rounded));
Cli.Render(new Table().AddColumn("A").AddColumn("B").AddRow("1","2"));
Cli.Render(new BulletList().AddItem("x").AddItem("y"));
Cli.Render(new OrderedList().AddItem("first").AddItem("second"));
Cli.Render(new TreeView("root").AddNode("child1").AddNode("child2"));
Cli.Render(new BarChart().AddBar("A", 50).AddBar("B", 80));

// Progress (IDisposable)
using var pb = new ProgressBar(100).WithLabel("Loading");
pb.SetValue(50);

// Spinner (IDisposable)
using var sp = new Spinner(Spinner.SpinnerStyle.Dots, "Working...");
Thread.Sleep(2000);

// Interactive
string?  pick  = new SelectionMenu("Choose:").AddOption("A").AddOption("B").Show();
string[] picks = new MultiSelectMenu("Pick:").AddOption("X").AddOption("Y").Show();
```
