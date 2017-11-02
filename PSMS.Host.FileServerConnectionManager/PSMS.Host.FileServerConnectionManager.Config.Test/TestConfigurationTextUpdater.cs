using System;
using System.Collections.Generic;
using System.Linq; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Diagnostics;

namespace PSMS.Host.FileServerConnectionManager.Config.Test
{
    [TestClass]
    public class TestConfigurationTextUpdater
    {


        [TestMethod]
        public void TestChange()
        {
            IConfigurationTextUpdater updater = new ConfigurationTextUpdater();

            IList<String> terms = new List<String>();
            const String one = "one";
            const String two = "two";
            const String three = "three";
            IDictionary<String, String> dict = new Dictionary<String,String>();
            dict.Add(one, one.ToUpper());
            dict.Add(two, two.ToUpper());
            dict.Add(three, three.ToUpper());

            IList<String> configIn = new List<String>();
            foreach (KeyValuePair<String, String> kvp in dict) {
                configIn.Add(String.Format("{0} - {1}", kvp.Key, kvp.Key));
            }

            IEnumerable<String> configOut = updater.Update(configIn, dict);
            Assert.IsFalse(configOut == null);

            StringBuilder sb = new StringBuilder();
            foreach (String lineOut in configOut) {

                sb.AppendLine(lineOut);
                String str = lineOut;

                // Should not equal any line in configIn
                Assert.AreEqual(configIn.Where(lineIn => lineIn.Contains(lineOut)).Count(), 0);

                // There should be a lineIn that has both Key and Value for a given KVP
                Assert.AreEqual(configIn.Where(lineIn => lineIn.Contains(lineOut)).Count(), 0);

                int linesWithValueTwice = 0;

                // There should be 1 KVP whose Value occurs twice in the line
                foreach (KeyValuePair<String, String> kvp in dict) {
                    int valueOccurs = 0;
                    // Key should not be there....
                    Assert.IsTrue(lineOut.IndexOf(kvp.Key) < 0);
                    // Value should be there once or twice.  In only 1 line should it occur twice
                    while (str.IndexOf(kvp.Value) >= 0)
                    {
                        valueOccurs++;
                        str = str.Substring(str.IndexOf(kvp.Value) + 1);
                    }
                    Assert.IsTrue(valueOccurs == 0 || valueOccurs == 2);

                    if (valueOccurs == 2)
                        linesWithValueTwice++;
                }

                Assert.AreEqual(linesWithValueTwice, 1);
            }

            String s = sb.ToString();
            Debug.WriteLine(s);



        }


    }
}
