using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public interface IConfigurationTextUpdater
    {
        IEnumerable<String> Update(IEnumerable<String> config, IDictionary<String,String> configKeyValuePairs);
    }
}
