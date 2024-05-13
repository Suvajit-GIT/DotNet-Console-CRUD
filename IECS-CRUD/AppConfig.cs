using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace IECS_CRUD
{
    public static class AppConfig
    {
        private static readonly Lazy<JObject> _appSettings = new Lazy<JObject>(() =>
        {
            string configFile = "AppConfig.json";  

            if (!File.Exists(configFile))
            {
                throw new FileNotFoundException($"Configuration file '{configFile}' not found.");
            }

            string json = File.ReadAllText(configFile);
            return JObject.Parse(json);
        });

        public static string FilePath
        {
            get { return _appSettings.Value["FilePath"].ToString(); }
        }
    }
}
