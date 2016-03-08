namespace Grapholizer.WebApi.Models.JSON
{
  public class EdgeJS
  {
    public string id { get; set; }
    public string source { get; set; }
    public string target { get; set; }
    public int? size { get; set; }
    public string color { get; set; }
    public string type { get; set; }
  }
}