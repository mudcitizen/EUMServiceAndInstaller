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

        Dictionary<String, String> _Dict;

        [TestInitialize]
        public void Setup() {
            const String one = "one";
            const String two = "two";
            const String three = "three";
            _Dict = new Dictionary<String, String>();
            _Dict.Add(one, one.ToUpper());
            _Dict.Add(two, two.ToUpper());
            _Dict.Add(three, three.ToUpper());
        }

        [TestMethod]
        public void TestChange()
        {

            IList<String> configIn = new List<String>();
            foreach (KeyValuePair<String, String> kvp in _Dict) {
                configIn.Add(String.Format("{0} - {1}", kvp.Key, kvp.Key));
            }

            IConfigurationTextUpdater updater = new ConfigurationTextUpdater();
            IEnumerable<String> configOut = updater.Update(configIn, _Dict);

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
                foreach (KeyValuePair<String, String> kvp in _Dict) {
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

        [TestMethod]
        public void TestNoChange2()
        {
            IList<String> configIn = new List<String>();

            foreach(KeyValuePair<String,String> kvp in _Dict)
            {
                // Key is lower case
                Assert.AreEqual(kvp.Key.ToLower(), kvp.Key);
                // Value is Key.ToUpper()
                Assert.AreEqual(kvp.Key.ToUpper(), kvp.Value);
                configIn.Add(String.Format("{0} - {1}", Guid.NewGuid().ToString().ToUpper(), kvp.Key.ToLower()));
            }

            IConfigurationTextUpdater updater = new ConfigurationTextUpdater();
            IEnumerable<String> configOut = updater.Update(configIn, _Dict);

            Assert.AreEqual(configIn.Count(), configOut.Count());

            // No line in "out" should match a line in "in" - ie every line should change
            foreach (String lineOut in configOut)
                Assert.IsTrue(configIn.Where(lineIn => lineIn == lineOut).Count() == 0);

            // Every line in out should change should match a line from in where in has been uppered()
            foreach (String lineOut in configOut)
                Assert.IsTrue(configIn.Where(lineIn => lineIn.ToUpper() == lineOut).Count() == 1);

        }


        [TestMethod]
        public void TestNoChange()
        {
            IList<String> configIn = new List<String>();

            for (int i = 0; i < 10; i++)
            {
                configIn.Add(Guid.NewGuid().ToString());
            }

            IConfigurationTextUpdater updater = new ConfigurationTextUpdater();
            IEnumerable<String> configOut = updater.Update(configIn, _Dict);

            Assert.IsTrue(configOut == null);


        }



    }
}
