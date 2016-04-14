using Grapholizer.Web.Utility;

namespace Grapholizer.Web
{
  public static class AppSettings
  {
    public static readonly AppSetting<string> ApiBaseUrl = new AppSetting<string>("Grapholizer.Web.ApiBaseUrl");
    public static readonly AppSetting<string> WebBaseUrl = new AppSetting<string>("Grapholizer.Web.WebBaseUrl");
  }
}