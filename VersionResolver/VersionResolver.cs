using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace VersionResolver
{
    public static class VersionResolver
    {
      private const string VersionsXmlElement = "versions";
      
      private static readonly IDictionary<string, string> _versionsDictionary = new Dictionary<string, string>();

      private static readonly DictionaryWithDefault _versions =
        new DictionaryWithDefault(_versionsDictionary);

      public static DictionaryWithDefault Versions {
        get { return _versions; }
      }

      public static string BuildAssemblyVersion(
        string majorVersionName, 
        string minorVersionName, 
        string buildVersionName, 
        string revisionVersionName)
      {
        return String.Format("{0}.{1}.{2}.{3}", 
           Versions[majorVersionName],
           Versions[minorVersionName], 
           Versions[buildVersionName], 
           Versions[revisionVersionName]
        );
      }

      public static void LoadFiles(params string[] fileNames)
      {
        _versionsDictionary.Clear();
        foreach (var fileName in fileNames.Where(File.Exists))
        {
          XElement root = XDocument.Load(fileName).Root;
          if (root == null)
          {
            throw new InvalidDataException("No root element");
          }
          if (!root.Name.ToString().Equals(VersionsXmlElement))
          {
            throw new InvalidDataException(String.Format("Root element {0} expected", VersionsXmlElement));
          }
          foreach (var versonElement in root.Elements())
          {
            string versionName = versonElement.Name.ToString();
            string versionValue = versonElement.Nodes().OfType<XText>().Single().Value;
            if (!_versionsDictionary.ContainsKey(versionName))
            {
              _versionsDictionary.Add(versionName, versionValue);
            }
            else
            {
              _versionsDictionary[versionName] = versionValue;
            }
          }
        }
      }

      public class DictionaryWithDefault
      {
        private readonly IDictionary<string, string> _dictionary;

        public DictionaryWithDefault(IDictionary<string, string> dictionary)
        {
          _dictionary = dictionary;
        }
        
        public string this[string key, string defaultValue = ""]
        {
          get
          {
            string value;
            return _dictionary.TryGetValue(key, out value) ? value : defaultValue;
          }
        }
      }
    }
}
