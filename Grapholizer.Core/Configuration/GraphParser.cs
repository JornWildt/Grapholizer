using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Grapholizer.Core.Utility;


namespace Grapholizer.Core.Configuration
{
  public class GraphParser : XmlFileWatchParser<GraphParser.GraphDefinitionSet>
  {
    private static object SerializerLock = new object();


    public class GraphDefinitionSet : Dictionary<string, GraphDefinition>
    {
    }


    public static GraphParser Instance = new GraphParser();


    public static GraphDefinition GetGraphDefinition(string name)
    {
      GraphDefinitionSet graphs = Instance.GetCache();
      return graphs[name];
    }


    public GraphParser()
    {
      SetAppSettingsKeys("GraphDir", "SchemaPath", null, null);
    }


    protected override GraphDefinitionSet Parse()
    {
      Logger.DebugFormat("Parse graphs - scanning directory '{0}'.", SourceDir);

      GraphDefinitionSet graphDefinitions = new GraphDefinitionSet();
      XmlSerializer serializer = new XmlSerializer(typeof(GraphDefinition));

      if (Directory.Exists(SourceDir))
      {
        foreach (string graphFilename in Directory.EnumerateFiles(SourceDir, "*.graph.config", SearchOption.AllDirectories))
        {
          Logger.DebugFormat("Parse graph '{0}'.", graphFilename);
          try
          {
            GraphDefinition graph = Deserialize<GraphDefinition>(graphFilename, serializer, SerializerLock);
            string name = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(graphFilename));
            graphDefinitions[name] = graph;
          }
          catch (Exception ex)
          {
            Logger.Warn(ex);
          }
        }
      }

      return graphDefinitions;
    }
  }
}
