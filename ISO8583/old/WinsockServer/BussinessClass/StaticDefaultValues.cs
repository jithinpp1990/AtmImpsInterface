using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace AtmImpsServer
{
    public static class StaticDefaultValues
    {

        public static string dsn;

        public static string Dsn
        {
            set { dsn = value; }
            get { return dsn; }

        }

        public static string uid;

        public static string Uid
        {
            set { uid = value; }
            get { return uid; }

        }

        public static string pwd;

        public static string Pwd
        {
            set { pwd = value; }
            get { return pwd; }

        }


        public static void getregistry()
        {
            string SoftwareKey = "SOFTWARE\\CC\\CC ATM Server";
            try
            {
                using (RegistryKey rkRegistryKeyOne = Registry.LocalMachine.OpenSubKey(SoftwareKey))
                {

                    Dsn = Convert.ToString(rkRegistryKeyOne.GetValue("DSN"));
                    Uid = Convert.ToString(rkRegistryKeyOne.GetValue("UID"));
                    Pwd = Convert.ToString(rkRegistryKeyOne.GetValue("PWD"));
                }
                if (Dsn == null)
                    Dsn = "CCBank";
                if (Uid == null)
                    Uid = "ccbankatmserver";
                if (Pwd == null)
                    Pwd = "ccbankatmserver";
            }
            catch (Exception ex)
            {

                Dsn = "CCBank";
                Uid = "ccbankatmserver";
                Pwd = "ccbankatmserver";

            }
        }


    }
}
