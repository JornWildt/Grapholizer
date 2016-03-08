using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using log4net;


namespace Grapholizer.Core.Utility
{
  public abstract class XmlFileWatchParser<CacheType>
    where CacheType : class
  {
    [System.Serializable]
    public class ParserException : Exception
    {
      public ParserException(string msg)
        : base(msg)
      {
      }
      protected ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
      {
      }
    }


    protected static readonly ILog Logger = LogManager.GetLogger((typeof(XmlFileWatchParser<CacheType>)));
    protected static FileSystemWatcher Watcher;
    protected List<string> ParseErrors = new List<string>();


    public static string SourceDirKey { get; protected set; }
    public static string SchemaDirKey { get; protected set; }
    public static string DefaultSourceDir { get; protected set; }
    public static string DefaultSchemaDir { get; protected set; }

    public static Func<string, string> MapPathToBaseDir = (s) => s;

    protected static void SetAppSettingsKeys(string sourceDirKey, string schemaDirKey, string defaultSourceDir, string defaultSchemaDir = null)
    {
      SourceDirKey = sourceDirKey;
      SchemaDirKey = schemaDirKey;
      DefaultSourceDir = defaultSourceDir;
      DefaultSchemaDir = defaultSchemaDir;

      TryCreateOrSetFileWatcherPath();
    }


    private static string _sourceDir;
    public static string SourceDir
    {
      get
      {
        if (_sourceDir == null)
        {
          _sourceDir = MapPathToBaseDir(ConfigurationManager.AppSettings[SourceDirKey] ?? DefaultSourceDir);
        }
        return _sourceDir;
      }
      set
      {
        if (_sourceDir != value)
        {
          TryCreateOrSetFileWatcherPath();
          Cache = null;
          _sourceDir = value;
        }
      }
    }


    private static string _schemaDir;
    public static string SchemaDir
    {
      get
      {
        if (_schemaDir == null)
        {
          _schemaDir = MapPathToBaseDir(ConfigurationManager.AppSettings[SchemaDirKey] ?? DefaultSchemaDir);
        }
        return _schemaDir;
      }
      set
      {
        if (_schemaDir != value)
        {
          TryCreateOrSetFileWatcherPath();
          Cache = null;
          _schemaDir = value;
        }
      }
    }

    private static CacheType Cache;

    protected ValidationEventHandler OnValidationError = (s, arg) => { throw arg.Exception; };

    protected virtual string NotInCacheErrorMsg
    {
      get { return "Could not find key '{0}' in cache"; }
    }


    static XmlFileWatchParser()
    {
      Watcher = new FileSystemWatcher();
      Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
      Watcher.Changed += new FileSystemEventHandler(OnChanged);
      Watcher.Created += new FileSystemEventHandler(OnChanged);
      Watcher.Deleted += new FileSystemEventHandler(OnChanged);
      Watcher.Renamed += new RenamedEventHandler(OnRenamed);
      Watcher.Filter = "*.*";
    }


    private static void TryCreateOrSetFileWatcherPath()
    {
      if (SourceDir == null)
      {
        Watcher.EnableRaisingEvents = false;
      }
      else
      {
        try
        {
          if (Watcher.Path != SourceDir || !Watcher.EnableRaisingEvents)
          {
            if (!Directory.Exists(SourceDir))
            {
              Directory.CreateDirectory(SourceDir);
            }
            Logger.DebugFormat("Assigning path '{0}' to FileWatchParser {1}.", SourceDir, typeof(XmlFileWatchParser<CacheType>));
            Watcher.Path = SourceDir;
            Watcher.EnableRaisingEvents = true;
          }
        }
        catch (Exception e)
        {
          Watcher.EnableRaisingEvents = false;
          Logger.Error(string.Format("Could not create directory: {0} or set it as FileSystemWatcher path", SourceDir), e);
        }
      }
    }


    static void OnRenamed(object sender, RenamedEventArgs e)
    {
      Cache = null;
    }


    static void OnChanged(object sender, FileSystemEventArgs e)
    {
      Cache = null;
    }


    public CacheType GetCache()
    {
      CacheType localCache = Cache;

      if (localCache == null)
        localCache = Cache = Parse();

      if (ParseErrors != null && ParseErrors.Count > 0)
      {
        StringBuilder errors = new StringBuilder();
        foreach (string error in ParseErrors)
        {
          errors.AppendLine(error);
        }
        throw new ParserException(errors.ToString()); // FIXME: parser error
      }

      return localCache;
    }


    public static void ClearCache()
    {
      Cache = null;
    }


    protected abstract CacheType Parse();


    protected T ExecuteWithErrorHandling<T>(string filename, Func<T> f)
    {
      try
      {
        return f();
      }
      catch (InvalidOperationException ex)
      {
        string msg = ex.Message;
        Exception innerEx = ex.InnerException;
        while (innerEx != null)
        {
          msg += " (" + innerEx.Message + ")";
          innerEx = innerEx.InnerException;
        }
        msg = string.Format("Reading file '{0}' failed: {1}.", filename, msg);
        throw new ParserException(msg);
      }
    }


    protected XmlReaderSettings CreateXmlReaderSettings(string schemaName)
    {
      XmlReaderSettings settings = new XmlReaderSettings();
      if (schemaName != null)
      {
        settings.ValidationType = ValidationType.Schema;
        try
        {
          settings.Schemas.Add(ParseSchema(SchemaDir, schemaName, OnValidationError));
        }
        catch (XmlSchemaException e) { throw new ParserException(e.SourceUri + " is invalid. Items will not be loaded."); }
        catch (FileNotFoundException e) { throw new ParserException(e.Message); }
        settings.ValidationEventHandler += OnValidationError;
      }
      return settings;
    }


    protected XmlSchema ParseSchema(string schemaPath, string schemaName, ValidationEventHandler onValidationError)
    {
      using (XmlTextReader r = new XmlTextReader(Path.Combine(schemaPath, schemaName)))
      {
        return XmlSchema.Read(r, onValidationError);
      }
    }


    protected T Deserialize<T>(string filename, XmlSerializer serializer, object serializerLock, XmlReaderSettings settings = null)
    {
      return ExecuteWithErrorHandling(filename, () =>
      {
        lock (serializerLock)
        {
          using (XmlReader reader = XmlReader.Create(filename, settings))
          {
            T deserialized = (T)serializer.Deserialize(reader);
            return deserialized;
          }
        }
      });
    }


    protected XmlReader CreateXmlReader(string filename, XmlReaderSettings settings)
    {
      if (settings == null)
        return XmlReader.Create(filename);
      else
        return XmlReader.Create(filename, settings);
    }
  }
}
