namespace Grapholizer.WebApi.Models.JSON
{
  public class NodeJS
  {
    public string id { get; set; }
    public string label { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int? size { get; set; }
    public string color { get; set; }
    public string type { get; set; }
    public string graphName { get; set; }
    public string nodeType { get; set; }
  }
}