using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSMS.Host.FileServerConnectionManager.Config.Test
{
    [TestClass]
    public class Experiments
    {
        [TestMethod]
        public void TestMessageBox()
        {
            DialogResult dr = MessageBox.Show("VFP Dataset?", "Caption", MessageBoxButtons.YesNoCancel);
            String txt;
            switch (dr)
            {
                case DialogResult.Yes:
                    {
                        txt = "Yes";
                        break;
                    }
                case DialogResult.No:
                    {
                        txt = "No";
                        break;
                    }
                default:   
                    {
                        txt = "default / cancel";
                        break;
                    }
            }

            Debug.WriteLine(txt);
            Debug.WriteLine("Done");

        }
    }
}
