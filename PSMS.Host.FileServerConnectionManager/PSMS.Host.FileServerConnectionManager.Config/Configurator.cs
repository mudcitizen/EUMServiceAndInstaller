using Sms.ConnectionUI.Client;
using Sms.ConnectionUI.VfpDataProvider;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public class Configurator
    {

        String _Slash = Path.DirectorySeparatorChar.ToString();


        public void Configure(String targetDir)
        {

            IConnectionStringProvider csp = new EumInstallClient();
            IConnectionStringValidator csv = new ConnectionStringValidator();


            String errMsg = "Not empty";

            while (!String.IsNullOrEmpty(errMsg))
            {

                String connStr = csp.GetConnectionString();
                errMsg = csv.Validate(connStr);


                if (String.IsNullOrEmpty(errMsg))
                {

                    // update app.config 
                }
                else
                {
                    MessageBox.Show(errMsg);
                }

            }



            // See if the connStr is complete

        }


        public void UpdateConfig(String targetDir, String hostDir)
        {
            targetDir = StripBackslash(targetDir);
            hostDir = StripBackslash(hostDir);
            String textToReplace = @"Data Source={0};";
            String[] configFiles = Directory.GetFiles(targetDir, "*.config");
            foreach (String cf in configFiles)
            {
                String configData = File.ReadAllText(cf);
                if (configData.Contains(textToReplace))
                {
                    String replacementText = String.Format(textToReplace, hostDir);
                    File.WriteAllText(cf, configData.Replace(textToReplace, replacementText));
                }
            }

        }

        String StripBackslash(String dirName)
        {
            while (dirName.EndsWith(@"\"))
            {
                String s = String.Format("{0} - {1}", dirName, dirName.Length);
                dirName = dirName.Remove(dirName.Length - 1, 1);
            }
            return dirName;
        }


    }
}
