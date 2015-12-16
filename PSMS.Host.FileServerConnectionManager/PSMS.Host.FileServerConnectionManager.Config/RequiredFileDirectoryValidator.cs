using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public class RequiredFileDirectoryValidator : DirectoryValidator
    {
        String _FileName;
        //"syEumreq.dbf"

        public RequiredFileDirectoryValidator(String fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(fileName);
            _FileName = fileName;
        }
        
        public override string IsValid(string directory)
        {
            String fileName = Path.Combine(directory, _FileName);
            if (File.Exists(fileName))
                return String.Empty;
            else
                return String.Format(Properties.Settings.Default.Invalid_Directory, _FileName);

        }
    }
}
