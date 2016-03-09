using System;
using System.Collections.Generic;


namespace Grapholizer.Core.Layouts
{
  public class RandomLayout : ILayout
  {
    public void Layout(Dictionary<string, Node> nodes, Node root)
    {
      Random randomizer = new Random();

      foreach (var item in nodes)
      {
        item.Value.X = randomizer.Next(100);
        item.Value.Y = randomizer.Next(100);
      }
    }
  }
}
