using YetAnotherCLISDK;
using YetAnotherCLISDK.Components;
using YetAnotherCLISDK.Core;

Cli.Initialize();

// ── Header ──────────────────────────────────────────────────────────────────
Cli.WriteLine("[bold cyan]  ██╗   ██╗███████╗████████╗[/]  [bold white]YetAnotherCLISDK[/]");
Cli.WriteLine("[bold cyan]  ╚██╗ ██╔╝██╔════╝╚══██╔══╝[/]  [gray]The colourful terminal SDK[/]");
Cli.WriteLine("[bold cyan]   ╚████╔╝ █████╗     ██║   [/]");
Cli.WriteLine("[bold cyan]    ╚██╔╝  ██╔══╝     ██║   [/]");
Cli.WriteLine("[bold cyan]     ██║   ███████╗   ██║   [/]");
Cli.WriteLine("[bold cyan]     ╚═╝   ╚══════╝   ╚═╝   [/]");
Cli.WriteLine();

// ── Colourful Text ───────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Colourful Text[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();
Cli.WriteLine("[bold]Bold[/]  [italic]Italic[/]  [underline]Underline[/]  [strikethrough]Strikethrough[/]  [dim]Dim[/]  [blink]Blink[/]");
Cli.WriteLine();
Cli.Write("[red]Red[/] [green]Green[/] [blue]Blue[/] [yellow]Yellow[/] [magenta]Magenta[/] [cyan]Cyan[/] ");
Cli.Write("[orange]Orange[/] [pink]Pink[/] [purple]Purple[/] [gold]Gold[/] [teal]Teal[/] [lime]Lime[/]");
Cli.WriteLine();
Cli.WriteLine("[#ff6b6b]Hex #ff6b6b[/]  [rgb(100,200,100)]RGB(100,200,100)[/]  [bold white on blue] Bg Colour [/]  [bold black on gold] Gold Bg [/]");
Cli.WriteLine();

// ── Status Messages ──────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Status Messages[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();
Cli.WriteSuccess("Operation completed successfully!");
Cli.WriteError("Something went wrong. Please try again.");
Cli.WriteWarning("Disk space is running low.");
Cli.WriteInfo("Your session expires in 10 minutes.");
Cli.WriteLine();

// ── Panels ──────────────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Panels & Borders[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();

Cli.Render(new Panel("Single border panel with [bold cyan]styled[/] content.\nSupports multiple lines and markup!")
    .WithTitle("[bold]Single[/]")
    .WithBorder(BorderStyle.Single));
Cli.WriteLine();

Cli.Render(new Panel("Double border with extra vertical padding.")
    .WithTitle("[bold yellow]Double Border[/]")
    .WithBorder(BorderStyle.Double)
    .WithBorderStyle(new Style { Foreground = Color.Yellow })
    .WithPadding(2, 1));
Cli.WriteLine();

Cli.Render(new Panel("[green]Rounded corners[/] look great for info boxes.")
    .WithTitle("[bold green]Rounded[/]")
    .WithBorder(BorderStyle.Rounded)
    .WithBorderStyle(new Style { Foreground = Color.Green }));
Cli.WriteLine();

Cli.Render(new Panel("[red]Heavy border[/] draws attention.")
    .WithTitle("[bold red]Heavy[/]")
    .WithBorder(BorderStyle.Heavy)
    .WithBorderStyle(new Style { Foreground = Color.Red }));
Cli.WriteLine();

// ── Tables ──────────────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Tables[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();

Cli.Render(new Table()
    .WithTitle("[bold]🏆 Leaderboard[/]")
    .WithBorder(BorderStyle.Rounded)
    .AddColumn("Rank",   Alignment.Center)
    .AddColumn("Player", Alignment.Left)
    .AddColumn("Score",  Alignment.Right)
    .AddColumn("Status", Alignment.Center)
    .AddRow("[gold]🥇 1[/]", "[bold cyan]Alice[/]",   "[bold yellow]9,850[/]",  "[green]Active[/]")
    .AddRow("[silver]🥈 2[/]", "[bold cyan]Bob[/]",     "[yellow]7,420[/]",      "[green]Active[/]")
    .AddRow("[coral]🥉 3[/]", "[bold cyan]Charlie[/]", "[yellow]6,110[/]",      "[gray]Away[/]")
    .AddRow("[gray]4[/]",   "[cyan]Diana[/]",        "[yellow]4,890[/]",      "[red]Offline[/]"));
Cli.WriteLine();

// ── Lists ────────────────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Lists[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();

Cli.Render(new BulletList("[bold]Tech Stack[/]")
    .AddItem("[cyan]Backend[/]", new BulletList()
        .AddItem("[blue].NET 10[/]")
        .AddItem("[green]PostgreSQL[/]")
        .AddItem("[orange]Redis[/]"))
    .AddItem("[cyan]Frontend[/]", new BulletList()
        .AddItem("[yellow]React[/]")
        .AddItem("[blue]TypeScript[/]"))
    .AddItem("[cyan]DevOps[/]", new BulletList()
        .AddItem("[blue]Docker[/]")
        .AddItem("[orange]Kubernetes[/]")));
Cli.WriteLine();

Cli.Render(new OrderedList("[bold]Getting Started[/]")
    .AddItem("[cyan]Install[/] the SDK via NuGet")
    .AddItem("Call [yellow]Cli.Initialize()[/] at startup")
    .AddItem("Use [green]markup strings[/] for styling")
    .AddItem("Render [magenta]components[/] with Cli.Render()"));
Cli.WriteLine();

// ── Tree View ────────────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Tree View[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();

Cli.Render(new TreeView("[bold cyan]YetAnotherCLISDK/[/]")
    .AddNode("[yellow]Core/[/]", t => t
        .AddNode("Ansi.cs")
        .AddNode("Color.cs")
        .AddNode("Style.cs")
        .AddNode("Markup.cs")
        .AddNode("BorderStyle.cs"))
    .AddNode("[yellow]Components/[/]", t => t
        .AddNode("Panel.cs")
        .AddNode("Table.cs")
        .AddNode("Lists.cs")
        .AddNode("BarChart.cs")
        .AddNode("TreeView.cs")
        .AddNode("ProgressBar.cs")
        .AddNode("Spinner.cs")
        .AddNode("SelectionMenu.cs")
        .AddNode("MultiSelectMenu.cs"))
    .AddNode("Cli.cs")
    .AddNode("IRenderable.cs")
    .AddNode("[green]Program.cs[/]"));
Cli.WriteLine();

// ── Bar Chart ────────────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Bar Chart[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();

Cli.Render(new BarChart()
    .WithTitle("[bold]📊 Monthly Downloads[/]")
    .AddBar("Jan", 12400, Color.Blue)
    .AddBar("Feb", 18600, Color.Cyan)
    .AddBar("Mar", 15200, Color.Green)
    .AddBar("Apr", 24100, Color.Yellow)
    .AddBar("May", 31500, Color.Orange)
    .AddBar("Jun", 28900, Color.Magenta));
Cli.WriteLine();

// ── Progress Bar ─────────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Progress Bar[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();

using (var pb = new ProgressBar(100).WithLabel("[cyan]Downloading[/] sdk.zip"))
{
    for (int i = 0; i <= 100; i += 2)
    {
        pb.SetValue(i);
        Thread.Sleep(18);
    }
}

using (var pb = new ProgressBar(60).WithLabel("[magenta]Installing[/]   packages").WithFillChar('▓').WithEmptyChar('░'))
{
    for (int i = 0; i <= 60; i++)
    {
        pb.SetValue(i);
        Thread.Sleep(20);
    }
}
Cli.WriteLine();

// ── Spinner ──────────────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Spinners[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();

foreach (var (style, label) in new[]
{
    (Spinner.SpinnerStyle.Dots,   "Dots   "),
    (Spinner.SpinnerStyle.Line,   "Line   "),
    (Spinner.SpinnerStyle.Arrow,  "Arrow  "),
    (Spinner.SpinnerStyle.Bounce, "Bounce "),
    (Spinner.SpinnerStyle.Pulse,  "Pulse  "),
})
{
    using var s = new Spinner(style, label);
    Thread.Sleep(1500);
}

Cli.WriteSuccess("All spinners completed!");
Cli.WriteLine();

// ── Interactive Menus ────────────────────────────────────────────────────────
Cli.Render(new Rule("[bold]Interactive Menus[/]").WithStyle(new Style { Foreground = Color.Blue }));
Cli.WriteLine();

var language = new SelectionMenu("What is your favourite programming language?")
    .AddOption("[blue].NET / C#[/]")
    .AddOption("[yellow]JavaScript[/]")
    .AddOption("[blue]Python[/]")
    .AddOption("[orange]Rust[/]")
    .AddOption("[cyan]Go[/]")
    .Show();

if (language is not null)
    Cli.WriteSuccess($"Great choice: {language}!");

Cli.WriteLine();

var features = new MultiSelectMenu("Which SDK features interest you most? [gray](Space = toggle, Enter = confirm)[/]")
    .AddOption("Colourful text & markup")
    .AddOption("Panels & borders")
    .AddOption("Tables")
    .AddOption("Lists & tree views")
    .AddOption("Progress bars & spinners")
    .AddOption("Interactive selection menus")
    .AddOption("Bar charts")
    .Show();

if (features.Length > 0)
{
    Cli.WriteLine();
    Cli.WriteInfo($"You selected {features.Length} feature(s):");
    var featureList = new BulletList().WithBullet("→");
    foreach (var f in features) featureList.AddItem($"[cyan]{f}[/]");
    Cli.Render(featureList);
}

Cli.WriteLine();
Cli.Render(new Rule().WithStyle(new Style { Foreground = Color.Gray }));
Cli.WriteLine("[bold cyan]  Thanks for using YetAnotherCLISDK![/]  [gray]Happy coding 🚀[/]");
Cli.WriteLine();

// ── Centered Layout ──────────────────────────────────────────────────────────
Console.ReadKey(intercept: true); // press any key to see centered layout demo
Cli.Render(new CenteredLayout(
    new Panel(
        "[bold]Name:[/]    ________________________\n" +
        "[bold]Email:[/]   ________________________\n" +
        "[bold]Password:[/] _______________________")
    .WithTitle("[bold cyan]  Login  [/]")
    .WithBorder(BorderStyle.Rounded)
    .WithBorderStyle(new Style { Foreground = Color.Cyan })
    .WithPadding(2, 1))
.WithClearScreen());
