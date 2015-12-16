using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PSMS.Host.WMI.LogicalDisks;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public class DriveTypeDirectoryValidator : DirectoryValidator
    {

        DriveType _DriveType;

        public DriveTypeDirectoryValidator(DriveType driveType)
        {
            _DriveType = driveType;
        }
        
        public override string IsValid(string directory)
        {

            String msg = String.Empty;
            LogicalDiskProvider ldp = new LogicalDiskProvider();
            IEnumerable<LogicalDisk> lds = (IEnumerable<LogicalDisk>)ldp.Get(null);
            int at = directory.IndexOf(Path.VolumeSeparatorChar);
            String drive = directory.ToUpper().Substring(0, at+1);
            LogicalDisk selectedDisk;
            try
            {
                selectedDisk = lds.Where(ld => ld.DeviceId.StartsWith(drive)).First();
                if (selectedDisk.DriveType != _DriveType)
                    msg = String.Format(Properties.Settings.Default.Wrong_DriveType, _DriveType);
            }
            catch (Exception ex)
            {
                msg = Properties.Settings.Default.Invalid_Drive;
            }
            
            return msg;


        }
    }
}
