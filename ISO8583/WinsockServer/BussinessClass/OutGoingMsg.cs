using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmImpsServer
{
    class OutGoingMsg
    {
        //0800822000000000000004000000000000000101010101123456001
        private const string MTI = "0800";
        private const string Net_signin = "001";
        private const string Net_signoff = "002";
        private const string Net_HandShake = "301";
        private const string BitMap = "82200000000000000400000000000000";
        private string stan = null;
        private string dateTime;
        private string DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        private string STAN
        {
            get { return stan; }
            set { stan = value; }
        }
        public void STAN_DateTime_Reader()
        {
            System.IO.StreamReader STANFile = new System.IO.StreamReader("c:\\STAN.txt");
            STAN = STANFile.ReadToEnd();

            DateTime time = System.DateTime.Now;
            string format = "MMddHHmmss";
            dateTime = time.ToString(format);
        }
        public string signIn()
        {
            string SignInMsg = null;
            STAN_DateTime_Reader();
            return SignInMsg = MTI + BitMap + DateTime + STAN + Net_signin;
        }
        public string signOff()
        {
            string SignOffMsg = null;
            STAN_DateTime_Reader();
            return SignOffMsg = MTI + BitMap + DateTime + STAN + Net_signoff;
        }
        public string HandShake()
        {
            string HandShakeMsg = null;
            STAN_DateTime_Reader();
            return HandShakeMsg = MTI + BitMap + DateTime + STAN + Net_HandShake;
        }

    }
}
