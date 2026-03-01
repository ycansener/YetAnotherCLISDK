using YetAnotherCLISDK.Core;

namespace YetAnotherCLISDK.Components;

public sealed class TreeView : IRenderable
{
    private sealed class Node
    {
        public string Label { get; }
        public List<Node> Children { get; } = [];
        public Node(string label) => Label = label;
    }

    private readonly Node _root;
    private Style _guideStyle  = new() { Foreground = Color.Gray };
    private Style _labelStyle  = Style.Default;
    private Style _rootStyle   = new() { Bold = true };

    public TreeView(string rootLabel) => _root = new Node(rootLabel);

    public TreeView AddNode(string label, Action<TreeView>? configure = null)
    {
        var child = new Node(label);
        _root.Children.Add(child);
        if (configure is not null)
        {
            var sub = new TreeView(label);
            configure(sub);
            foreach (var n in sub._root.Children)
                child.Children.Add(n);
        }
        return this;
    }

    public IEnumerable<string> GetLines()
    {
        var lines = new List<string>();
        lines.Add(_rootStyle.Apply(Markup.Parse(_root.Label)));
        RenderChildren(_root.Children, "", lines);
        return lines;
    }

    private void RenderChildren(List<Node> nodes, string prefix, List<string> lines)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            bool last  = i == nodes.Count - 1;
            var  node  = nodes[i];
            var  connector = _guideStyle.Apply(last ? "└── " : "├── ");
            var  label     = _labelStyle.Apply(Markup.Parse(node.Label));

            lines.Add($"{prefix}{connector}{label}");

            if (node.Children.Count > 0)
            {
                var childPrefix = prefix + _guideStyle.Apply(last ? "    " : "│   ");
                RenderChildren(node.Children, childPrefix, lines);
            }
        }
    }
}
