using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace AtmImpsServer
{
    public class ServerLoadDefaults
    {
        public static string ip;
        public static string[] ports;
        public static string pwd;
        public static string dsn;
        public static string uid;
        public static string logPath;
        public static string Ip
        {
            set { ip = value; }
            get { return ip; }

        }
        public static string[] Ports
        {
            set { ports = value; }
            get { return ports; }
        }

        public static string Dsn
        {
            set { dsn = value; }
            get { return dsn; }

        }

        public static string Uid
        {
            set { uid = value; }
            get { return uid; }

        }

        public static string Pwd
        {
            set { pwd = value; }
            get { return pwd; }

        }
        public static string LogPath
        {
            set { logPath = value; }
            get { return logPath; }
        }
        public static void getIpPorts()
        {
            DBClass DBC = new DBClass();
            DBC.IPnPorts();
            ip = DBC.ip;
            ports = DBC.ports;
        }
        public static void getregistry()
        {
            Encrypt.EncryptPass crypto = new Encrypt.EncryptPass();
            Pwd = "ccbankatmserver";//crypto.EncryptToString("ccbankatmserver");
            string SoftwareKey = "SOFTWARE\\CC\\CC ATM Server";
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(SoftwareKey))
                {
                    LogPath = Convert.ToString(key.GetValue("LOG"));
                    Dsn = Convert.ToString(key.GetValue("DSN"));
                    Uid = Convert.ToString(key.GetValue("UID"));
                    Pwd = Convert.ToString(key.GetValue("PWD"));
                }
                if (Dsn == null)
                    Dsn = "CCBank";
                if (Uid == null || Uid == "")
                    Uid = "ccbankatmserver";
                if (Pwd == null || Pwd == "")
                    Pwd = "ccbankatmserver";
                if (LogPath == null || LogPath == "")
                    LogPath = "c:\atm_messages.txt";
            }
            catch (Exception ex)
            {
                            
                Dsn = "CCBank";
                Uid = "ccbankatmserver";
                Pwd = "ccbankatmserver"; //crypto.EncryptToString("ccbankatmserver");
                LogPath = "c:\atm_messages.txt";

            }
        }




    }
}
