namespace Grapholizer.Core
{
  public class Node
  {
    public string Id { get; set; }

    public string Label { get; set; }

    public int? Size { get; set; }

    public string Color { get; set; }

    public string Type { get; set; }

    public Edge[] Edges { get; set; }
  }
}
