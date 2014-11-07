using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;
using System.Xml;

namespace OpenNETCF.Configuration
{
  class ConfigurationRecord
  {
    private enum HaveFactoryEnum
    {
      NotFound = 0,
      Group = 1,
      Section = 2,
    }

    private Hashtable results;
    private Hashtable factories;
    private Hashtable unevaluatedSections;
    private bool factoriesNoInherit;
    private string filename = null;
    private Exception error;
    private readonly ConfigurationRecord parent = null;
    private static object RemovedFactorySingleton = new Object();
    private static object GroupSingleton = new Object();

    private Hashtable EnsureFactories
    {
      get
      {
        // Check if factories has already been initialized
        if (factories == null)
        {
          factories = new Hashtable();
        }
        return factories;
      }
    }

    public ConfigurationRecord()
      : this(null)
    {
    }

    public ConfigurationRecord(ConfigurationRecord Parent)
    {
      results = new Hashtable();
      parent = Parent;
    }

    public bool Load(string Filename)
    {
      Uri uri = new Uri(Filename);

      // Is the file we're trying to load a local file?
      if (uri.Scheme == "file")
      {
        // If so, use the URI's local path
        filename = uri.LocalPath;
      }
      else
      {
        // Else just use the filename as provided
        filename = Filename;
      }

      XmlTextReader xmlTextReader = null;
      try
      {
        // Load the config file into a XmlReader
        xmlTextReader = OpenXmlTextReader(filename);
        if (xmlTextReader != null)
        {
          // Generate section factories from the raw XML
          ScanFactoriesRecursive(xmlTextReader);
          // 
          if (xmlTextReader.Depth == 1)
          {
            ScanSectionsRecursive(xmlTextReader, null);
          }
          bool flag = true;
          return flag;
        }
      }
      catch (ConfigurationException)
      {
        throw;
      }
      catch (Exception e)
      {
        error = TranslateXmlParseOrEvaluateErrors(e);
        throw error;
      }
      finally
      {
        if (xmlTextReader != null)
        {
          xmlTextReader.Close();
        }
      }
      return false;
    }

    public bool LoadXml(string xmlFragment)
    {
      XmlTextReader xmlTextReader = null;
      try
      {
        // Load the config file into a XmlReader
        xmlTextReader = LoadXmlTextReader(xmlFragment);
        if (xmlTextReader != null)
        {
          // Generate section factories from the raw XML
          ScanFactoriesRecursive(xmlTextReader);
          // 
          if (xmlTextReader.Depth == 1)
          {
            ScanSectionsRecursive(xmlTextReader, null);
          }
          bool flag = true;
          return flag;
        }
      }
      catch (ConfigurationException)
      {
        throw;
      }
      catch (Exception e)
      {
        error = TranslateXmlParseOrEvaluateErrors(e);
        throw error;
      }
      finally
      {
        if (xmlTextReader != null)
        {
          xmlTextReader.Close();
        }
      }
      return false;
    }

    public object GetConfig(string configKey, object context)
    {
      if (error != null)
      {
        throw error;
      }

      if (!results.Contains(configKey))
      {
        object config = ResolveConfig(configKey, context);
        lock (results.SyncRoot)
        {
          results[configKey] = config;
        }
        return config;
      }

      // else
      return results[configKey];
    }

    public object ResolveConfig(string configKey, object context)
    {
      if (unevaluatedSections != null && unevaluatedSections.Contains(configKey))
      {
        return Evaluate(configKey, context);
      }

      if (parent != null)
      {
        return parent.GetConfig(configKey, context);
      }

      return null;
    }

    private object Evaluate(string configKey, object context)
    {
      // Get config factory
      IConfigurationSectionHandler factory = GetFactory(configKey);

      // Get the parent result which will be passed to the section handler
      object parentResult = (parent == null) ? null : parent.GetConfig(configKey, context);

      // Evaluate the config section
      string[] strs = configKey.Split(new char[] { '/' });
      XmlTextReader xmlTextReader = null;
      object result = null;
      try
      {
        xmlTextReader = OpenXmlTextReader(filename);
        result = EvaluateRecursive(factory, parentResult, strs, 0, xmlTextReader, context);
      }
      catch (ConfigurationException)
      {
        throw;
      }
      catch (Exception e)
      {
        throw TranslateXmlParseOrEvaluateErrors(e);
      }
      finally
      {
        if (xmlTextReader != null)
        {
          xmlTextReader.Close();
        }
      }

      // Remove the configKey from _unevaluatedSections 
      // When all sections are removed throw it away
      if (unevaluatedSections.Count == 0)
      {
        unevaluatedSections = null;
      }

      return result;
    }

    private HaveFactoryEnum HaveFactory(string configKey)
    {
      if (factories != null)
      {
        if (factories.Contains(configKey))
        {
          object o = factories[configKey];

          if (o == RemovedFactorySingleton)
          {
            return HaveFactoryEnum.NotFound;
          }
          if (o == GroupSingleton)
          {
            return HaveFactoryEnum.Group;
          }
          return HaveFactoryEnum.Section;
        }
      }
      if (!factoriesNoInherit && parent != null)
      {
        return parent.HaveFactory(configKey);
      }

      return HaveFactoryEnum.NotFound;
    }

    private IConfigurationSectionHandler GetFactory(string configKey)
    {
      if (factories != null)
      {
        if (factories.Contains(configKey))
        {
          object o = factories[configKey];
          if (o == RemovedFactorySingleton)
          {
            return null;
          }
          IConfigurationSectionHandler factory = o as IConfigurationSectionHandler;
          if (factory != null)
          {
            return factory;
          }

          // If we still have a string, get the type and create the IConfigurationSectionHandler
          string factoryType = (string)o;
          o = null;

          try
          {
            Type t = Type.GetType(factoryType);
            if (t != null)
            {
              if (!typeof(IConfigurationSectionHandler).IsAssignableFrom(t))
              {
                throw new ConfigurationException("Type does not implement IConfigSectionHandler");
              }
              // throws MissingMethodException if there is no valid ctor
              o = Activator.CreateInstance(t);
            }
          }
          catch (Exception e)
          {
            throw new ConfigurationException(e.Message, e);
          }

          if (o == null)
          {
            throw new ConfigurationException(string.Format("Could not create type instance of type {0} for key '{1}'", factoryType, configKey));
          }

          factory = o as IConfigurationSectionHandler;
          if (factory == null)
          {
            throw new ConfigurationException("Type doesn't implement IConfigSectionHandler");
          }

          lock (factories.SyncRoot)
          {
            factories[configKey] = factory;
          }

          return factory;
        }
      }

      if (!factoriesNoInherit && parent != null)
      {
        return parent.GetFactory(configKey);
      }
      else
      {
        return null;
      }
    }

    private object EvaluateRecursive(IConfigurationSectionHandler factory, object config, string[] keys, int iKey, XmlTextReader reader, object context)
    {
      string name = keys[iKey];
      int depth = reader.Depth;

      while (reader.Read() && reader.NodeType != XmlNodeType.Element) ;

      while (reader.Depth == depth + 1)
      {
        if (reader.Name == name)
        {
          if (iKey < keys.Length - 1)
          {
            config = EvaluateRecursive(factory, config, keys, iKey + 1, reader, context);
          }
          else
          {
            // Call configuration section handler
            int line = reader.LineNumber;

            // Try-catch is necessary to protect from exceptions in user config handlers
            try
            {
              ConfigXmlDocument doc = new ConfigXmlDocument();
              doc.LoadSingleElement(filename, reader);
              config = factory.Create(config, context, doc.DocumentElement);
            }
            catch (ConfigurationException)
            {
              // Bubble ConfigurationExceptions
              throw;
            }
            catch (XmlException)
            {
              // Bubble XmlExceptions
              throw;
            }
            catch (Exception ex)
            {
              // Wrap all others as ConfigurationExceptions
              throw new ConfigurationException("Exception in ConfigSectionHandler", ex, filename, line);
            }
          }
          continue;
        }
        StrictSkipToNextElement(reader);
      }
      return config;
    }

    private void ScanFactoriesRecursive(XmlTextReader reader)
    {
      // Skip processor instructions and comments
      reader.MoveToContent();

      if (reader.NodeType != XmlNodeType.Element || reader.Name != "configuration")
      {
        throw BuildConfigError(filename + " doesn't have root configuration", reader);
      }

      CheckForUnrecognizedAttributes(reader);

      // Move to first child of <configuration>
      StrictReadToNextElement(reader);
      if (reader.Depth == 1)
      {
        if (reader.Name == "configSections")
        {
          CheckForUnrecognizedAttributes(reader);
          ScanFactoriesRecursive(reader, null);
        }
      }
    }

    private void ScanFactoriesRecursive(XmlTextReader reader, string configKey)
    {

      int depth = reader.Depth;
      StrictReadToNextElement(reader);
      while (reader.Depth == depth + 1)
      {
        switch (reader.Name)
        {
          case "sectionGroup":
            {
              // Get the name of the current sectionGroup
              string tagName = null;
              if (reader.HasAttributes)
              {
                while (reader.MoveToNextAttribute())
                {
                  if (reader.Name != "name")
                    ThrowUnrecognizedAttribute(reader);
                  tagName = reader.Value;
                }
                reader.MoveToElement();
              }
              // sectionGroup name attribute must have a value
              CheckRequiredAttribute(tagName, "name", reader);
              // Check the validity of the section name
              VerifySectionName(tagName, reader);

              // Add the current sectionGroup to the hashtable and process it
              string tagKey = TagKey(configKey, tagName);
              if (HaveFactoryEnum.Section == HaveFactory(tagName))
              {
                throw BuildConfigError("Tag name already defined", reader);
              }
              EnsureFactories[tagKey] = GroupSingleton;
              ScanFactoriesRecursive(reader, tagKey);
              continue;
            }
          case "section":
            {
              string tagName = null;
              string typeName = null;

              if (reader.HasAttributes)
              {
                while (reader.MoveToNextAttribute())
                {
                  switch (reader.Name)
                  {
                    case "name":
                      tagName = reader.Value;
                      break;
                    case "type":
                      typeName = reader.Value;
                      break;
                    case "allowLocation":
                    case "allowDefinition":
                      break;
                    default:
                      ThrowUnrecognizedAttribute(reader);
                      break;
                  }
                }
                reader.MoveToElement();
              }
              CheckRequiredAttribute(tagName, "name", reader);
              CheckRequiredAttribute(typeName, "type", reader);
              VerifySectionName(tagName, reader);
              string tagKey = TagKey(configKey, tagName);
              if (HaveFactory(tagKey) != HaveFactoryEnum.NotFound)
              {
                throw BuildConfigError("Tag name already defined", reader);
              }
              EnsureFactories[tagKey] = typeName;
              break;
            }
          case "remove":
            {
              string tagName = null;
              // Schema defines that <remove /> elements must have a name attribute
              // so check to see if we have one and retrieve its value
              if (reader.HasAttributes)
              {
                while (reader.MoveToNextAttribute())
                {
                  if (reader.Name != "name")
                    ThrowUnrecognizedAttribute(reader);
                  tagName = reader.Value;
                }
                reader.MoveToElement();
              }
              // Enforce schema definition of remove element
              if (tagName == null)
                this.ThrowRequiredAttribute(reader, "name");
              // Does the remove element have a valid name?
              this.VerifySectionName(tagName, reader);

              // If so, add it to the hashtable
              string tagKey = ConfigurationRecord.TagKey(configKey, tagName);
              if (HaveFactory(tagKey) != HaveFactoryEnum.Section)
              {
                throw BuildConfigError("Could not remove section handler", reader);
              }
              EnsureFactories[tagName] = RemovedFactorySingleton;
              break;
            }
          case "clear":
            {
              // Config schema definition states that clear element has no attributes
              CheckForUnrecognizedAttributes(reader);
              // Reset factories hashtable
              factories = null;
              factoriesNoInherit = true;
              break;
            }
          default:
            ThrowUnrecognizedElement(reader);
            break;
        }

        this.StrictReadToNextElement(reader);
        // Unrecognized children are not allowed
        if (reader.Depth > depth + 1)
        {
          ThrowUnrecognizedElement(reader);
        }
      }
    }

    private static string TagKey(string configKey, string tagName)
    {
      return (configKey != null) ? String.Concat(configKey, "/", tagName) : tagName;
    }

    private void VerifySectionName(string tagName, XmlTextReader reader)
    {
      if (tagName.StartsWith("config"))
      {
        BuildConfigError("Tag name cannot begin with config", reader);
      }
      if (tagName == "location")
      {
        BuildConfigError("Tag name cannot be location", reader);
      }
    }

    private void ScanSectionsRecursive(XmlTextReader reader, string configKey)
    {
      int depth = reader.Depth;

      // only move to child nodes on first level (we've already passed the first <configSections>)
      if (configKey == null)
      {
        depth = 0;
      }
      else
      {
        StrictReadToNextElement(reader);
      }

      while (reader.Depth == depth + 1)
      {
        string tagName = reader.Name;
        string tagKey = TagKey(configKey, tagName);

        HaveFactoryEnum haveFactory = HaveFactory(tagKey);

        if (haveFactory == HaveFactoryEnum.Group)
        {
          ScanSectionsRecursive(reader, tagKey);
          continue;
        }
        else if (haveFactory == HaveFactoryEnum.NotFound)
        {
          if (tagKey != "location")
          {
          }
          else if (tagKey == "configSections")
          {
            throw BuildConfigError("ClientConfig: too many ConfigSection elements", reader);
          }
          else
          {
            throw BuildConfigError("Unrecognized ConfigurationSection: " + tagName, reader);
          }
        }
        else
        {
          if (unevaluatedSections == null)
          {
            unevaluatedSections = new Hashtable();
          }
          unevaluatedSections[tagKey] = null;
        }
        StrictSkipToNextElement(reader);
      }
    }

    private static XmlTextReader OpenXmlTextReader(string configFileName)
    {
      string localFileName;
      Uri uri = new Uri(configFileName);

      bool isFile = uri.Scheme == "file";
      if (isFile)
      {
        localFileName = uri.LocalPath;
      }
      else
      {
        localFileName = uri.ToString();
      }
      XmlTextReader reader = null;
      try
      {
        if (isFile)
        {
          if (!File.Exists(uri.LocalPath))
          {
            return null;
          }
          reader = new XmlTextReader(localFileName);
        }
        else
        {
          try
          {
            Stream stream = File.OpenRead(configFileName);
            reader = new XmlTextReader(stream);
          }
          catch
          {
            return null;
          }
        }
        reader.MoveToContent();
        return reader;
      }
      catch (Exception)
      {
        throw new ConfigurationException("ErrorloadingXMLfile", localFileName, 0);
      }
    }

    private static XmlTextReader LoadXmlTextReader(string xmlFragment)
    {
      XmlTextReader reader = null;
      try
      {
        reader = new XmlTextReader(xmlFragment, XmlNodeType.Element, null);
        reader.MoveToContent();
        return reader;
      }
      catch (Exception)
      {
        throw new ConfigurationException("ErrorloadingXMLFragment", xmlFragment, 0);
      }
    }

    private ConfigurationException BuildConfigError(string message, XmlTextReader reader)
    {
      return new ConfigurationException(message, filename, reader.LineNumber);
    }

    private ConfigurationException BuildConfigError(string message, Exception inner, XmlTextReader reader)
    {
      return new ConfigurationException(message, inner, filename, reader.LineNumber);
    }

    private void StrictReadToNextElement(XmlTextReader reader)
    {
      while (reader.Read())
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          return;
        }
        CheckIgnorableNodeType(reader);
      }
    }

    private void StrictSkipToNextElement(XmlTextReader reader)
    {
      reader.Skip();
      while (!reader.EOF && reader.NodeType != XmlNodeType.Element)
      {
        CheckIgnorableNodeType(reader);
        reader.Read();
      }
    }

    private void CheckIgnorableNodeType(XmlTextReader reader)
    {
      if (reader.NodeType != XmlNodeType.Comment && reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.Whitespace && reader.NodeType != XmlNodeType.SignificantWhitespace)
      {
        ThrowUnrecognizedElement(reader);
      }
    }

    private void ThrowUnrecognizedAttribute(XmlTextReader reader)
    {
      throw BuildConfigError("Unrecognized configuration attribute:" + reader.Value, reader);
    }

    private void CheckForUnrecognizedAttributes(XmlTextReader reader)
    {
      if (reader.HasAttributes)
      {
        reader.MoveToNextAttribute();
        ThrowUnrecognizedAttribute(reader);
      }
    }

    private void ThrowRequiredAttribute(XmlTextReader reader, string attrib)
    {
      throw BuildConfigError("Missing required attribute", reader);
    }

    private void ThrowUnrecognizedElement(XmlTextReader reader)
    {
      throw BuildConfigError("ConfigBase unrecognized element", reader);
    }

    private void CheckRequiredAttribute(object o, string attrName, XmlTextReader reader)
    {
      if (o == null)
      {
        ThrowRequiredAttribute(reader, "name");
      }
    }

    private ConfigurationException TranslateXmlParseOrEvaluateErrors(Exception e)
    {
      XmlException e2 = e as XmlException;
      if (e2 != null)
      {
        return new ConfigurationException(e2.Message, e, filename, e2.LineNumber);
      }
      else
      {
        return new ConfigurationException("Error loading XML file", e, filename, 0);
      }
    }
  }
}
