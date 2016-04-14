using Grapholizer.Web.Utility;

namespace Grapholizer.Web
{
  public static class AppSettings
  {
    public static readonly AppSetting<string> ApiBaseUrl = new AppSetting<string>("Grapholizer.Web.ApiBaseUrl");
  }
}