namespace Grapholizer.Core
{
  public class Node
  {
    public string Type { get; set; }

    public string Id { get; set; }

    public string Label { get; set; }

    public int? Size { get; set; }

    public string Color { get; set; }

    public string Symbol { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public Edge[] Edges { get; set; }
  }
}
