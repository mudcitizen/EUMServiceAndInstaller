using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public class ConfigurationTextUpdater : IConfigurationTextUpdater
    {
        public IEnumerable<string> Update(IEnumerable<string> config, IDictionary<String, String> configKeyValuePairs)
        {
            bool changed = false;
            IList<String> configOut = new List<String>();

            foreach (string str in config)
            {
                String configLine = str;

                foreach (KeyValuePair<String, String> kvp in configKeyValuePairs)
                {

                    if (configLine.Contains(kvp.Key))
                    {
                        configLine = configLine.Replace(kvp.Key, kvp.Value);
                        changed = true;
                    }
                }

                configOut.Add(configLine);
            }

            if (!changed)
                configOut = null;

            return configOut;
        }
    }
}
