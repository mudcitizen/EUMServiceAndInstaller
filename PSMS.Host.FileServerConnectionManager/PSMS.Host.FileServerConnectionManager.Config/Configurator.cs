using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public class Configurator
    {

        String _Slash = Path.DirectorySeparatorChar.ToString();

        public String GetHostFolder()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = false;
            DialogResult dr = DialogResult.None;

            List<DirectoryValidator> dvs = new List<DirectoryValidator>();
            dvs.Add(new RequiredFileDirectoryValidator("syEumreq.dbf"));
            dvs.Add(new DriveTypeDirectoryValidator(DriveType.Fixed));

            Boolean done = false;
            while (done == false)
            {

                dlg.Description = Properties.Settings.Default.Get_Host_Dialog_Description;
                dr = dlg.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    done = true;
                    foreach (DirectoryValidator dv in dvs)
                    {
                        String msg = dv.IsValid(dlg.SelectedPath);
                        if (!String.IsNullOrEmpty(msg))
                        {
                            MessageBox.Show(msg, Properties.Settings.Default.MessageBox_Title, MessageBoxButtons.OK);
                            done = false;
                        }
                    }
                }
            }

            return dlg.SelectedPath;
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
