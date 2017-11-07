using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using PSMS.Host.WMI.Contracts;
using PSMS.Host.WMI.Common;
using PSMS.Host.WMI.ClassFactory;
using PSMS.Host.WMI.ServerConnections;
using PSMS.Host.WMI.RequestProcessing;
using System.Threading;
using System.Net;
using PSMS.Host.WMI.OpenFiles;

namespace PSMS.Host.WMI.ConsoleApplication
{
    class Program
    {
        const String _LocalHostIPAdress = "127.0.0.1";
        const String _OutDirectory = @"C:\Temp\";

        static void Main(string[] args)
        {
            /*
            Console.WriteLine("Start");
            VfpDataExtractor vfp = new VfpDataExtractor(Constants.ConnectionStringNames.Vfp, "SELECT * FROM SYEUMREQ", @"c:\TEMP\VFPDATA.XML");
            vfp.Exec();
            ProcessNext();
            Console.WriteLine("Done");
             */
            EmulateService();
        }

        static void ProcessNext()
        {
            DateTime beg = DateTime.Now;
            IRequestProcessorFactory factory = new AFactory();
            Console.WriteLine(String.Format("GetFactory - {0}", DateTime.Now - beg));
            beg = DateTime.Now;
            RequestProcessor processor = new RequestProcessor(factory);
            Console.WriteLine(String.Format("Build processor - {0}", DateTime.Now - beg));
            beg = DateTime.Now;
            processor.ProcessNext();
            Console.WriteLine(String.Format("ProcessNext() - {0}", DateTime.Now - beg));
        }

        static void DontKnow()
        {
            if (!Directory.Exists(_OutDirectory))
                Directory.CreateDirectory(_OutDirectory);
            //
            List<ServerConnection> scs = GetConnections();
            GetServerConnectionHostEntries(scs);
            LocalHostLab();
            DsnProviderTest();
        }

        static List<ServerConnection> GetConnections()
        { 
            ServerConnectionProvider scp = new ServerConnectionProvider();
            List<ServerConnection> scs = ((IEnumerable<ServerConnection>)scp.Get(null)).ToList();
            GenericXmlSerializer<List<ServerConnection>> s = new GenericXmlSerializer<List<ServerConnection>>();
            s.WriteFile(scs,GetFullyQualifiedOutFileName("ServerConnections.xml"));
            return scs;
        }

        static List<ServerConnectionHostEntry> GetServerConnectionHostEntries(List<ServerConnection> scs)
        {
            //                    IPHostEntry he = Dns.GetHostEntry(name);
            List<ServerConnectionHostEntry> items = new List<ServerConnectionHostEntry>();
            foreach (ServerConnection sc in scs)
            {
                IPHostEntry he = null;
                try
                {
                    he = Dns.GetHostEntry(sc.ComputerName);
                    items.Add(new ServerConnectionHostEntry() { Connection = sc, HostName = he.HostName, Aliases = he.Aliases.ToArray(), AddressList = he.AddressList.Select(ipa => ipa.ToString()).ToArray() });
                }
                catch (Exception ex)
                { }
                //items.Add(new ServerConnectionHostEntry() { Connection = sc, HostEntry = he });
            }
            GenericXmlSerializer<ServerConnectionHostEntry[]> s = new GenericXmlSerializer<ServerConnectionHostEntry[]>();
            s.WriteFile(items.ToArray(), GetFullyQualifiedOutFileName("ConnectionHostEntries.xml"));

            return items; 

        }

        static void  LocalHostLab()
        {

            ServerConnection sc = new ServerConnection() { UserName = "Joe", ComputerName = _LocalHostIPAdress };
            IPHostEntry he = Dns.GetHostEntry(sc.ComputerName);

            IPAddress theIpa;
            Boolean isIpa = IPAddress.TryParse(sc.ComputerName, out theIpa);

            Console.Write("Try {0}; IPAddress {1}", isIpa, theIpa);

            ServerConnectionHostEntry sche = new ServerConnectionHostEntry();

            sche.Connection = sc;
            sche.HostName = he.HostName;
            sche.AddressList = he.AddressList.Select(ipa => ipa.ToString()).ToArray();
            GenericXmlSerializer<ServerConnectionHostEntry> s = new GenericXmlSerializer<ServerConnectionHostEntry>();
            s.WriteFile(sche, GetFullyQualifiedOutFileName("HostEntry.xml"));
        }

        static void DsnProviderTest()
        {
            List<String> ipas = new List<string>() { _LocalHostIPAdress };
            DnsProvider p = new DnsProvider();
            Dictionary<String, String> items = p.GetHostNames(ipas);
            Console.WriteLine("** DsnProviderTest ** ; {0}", items.Count);
            foreach (KeyValuePair<String, String> kvp in items)
            {
                Console.WriteLine("Key - {0}; Value - {1}", kvp.Key, kvp.Value);
            }
            Console.WriteLine("");

        }

        static string GetFullyQualifiedOutFileName(String fileName)
        {
            return _OutDirectory+fileName;
        }

        static void EmulateService()
        {
            IRequestProcessorFactory factory = new AFactory();
            RequestProcessor processor = new RequestProcessor(factory);

            String fileName = @"C:\Temp\Stop.txt";
            if (File.Exists(fileName))
                File.Delete(fileName);

            Process p = Process.GetCurrentProcess();
            while (!File.Exists(fileName))
            {
                Console.WriteLine("{0} {1} Create {2} to stop this", DateTime.Now, p.Id, fileName);
                processor.ProcessNext();
                Thread.Sleep(1000);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }        
    }

    public class ServerConnectionHostEntry
    {
        public ServerConnection Connection { get; set; }
        //public IPHostEntry HostEntry { get; set; }
        public String HostName;
        public String[] Aliases;
        public String[] AddressList;



    }
}
