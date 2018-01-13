using System;
using System.Threading;
using SFTP.Models;
using SFTP.DbOperations;
using SFTP.DataTransfer;
using System.Collections.Generic;
using System.Timers;
using System.Configuration;
namespace SFTP
{
    public class ThreadStarter
    {
        System.Timers.Timer uploadTimer = new System.Timers.Timer();
        System.Timers.Timer downloadTimer = new System.Timers.Timer();
        private SftpHostModel _host = new SftpHostModel();
        PrimaryDataloader pdObject = new PrimaryDataloader();
        private List<ServiceMap> _servicList = new List<ServiceMap>();
        public ThreadStarter()
        {
            _host = PrimaryDataloader.GetPrimaryData();
            _servicList = PrimaryDataloader.GetServiceMap();
            pdObject.CheckRemoteDir();
            
        }
        public void Start()
        {
            List<string> er = new List<string>();
            try
            {
                while (true)
                {
                    if (ConfigurationManager.AppSettings["uploadEnabled"] == "True")
                    {
                        UploadThread();
                        Thread.Sleep(10000);
                    }
                    if (ConfigurationManager.AppSettings["downloadEnabled"] == "True")
                    {
                        DownloadThread();
                        Thread.Sleep(10000);
                    }

                };
                
                // DownloadThread(); 1600
                //Console.WriteLine("ATM Interface Started Successfully");
                //uploadTimer.Interval = 3000;
                //uploadTimer.Elapsed += new ElapsedEventHandler(UploadThread);
                //uploadTimer.Enabled = true;

                //downloadTimer.Interval = 5000;
                //downloadTimer.Elapsed += new ElapsedEventHandler(DownloadThread);
                //downloadTimer.Enabled = true;


            }
            catch (Exception ex)
            {
                er.Add(ex.ToString());
                FileWriter.WriteFile(@"c:\sftp\log\", "log.txt",er);
            }
           
        }

        public void DownloadThread(object source, ElapsedEventArgs e)
        {
            Downloader downloadObj = new Downloader();
            Console.WriteLine("Download Started  :  " + Convert.ToString(DateTime.Now));
            downloadObj.DownLoadAndProcess(_servicList);
        }
        public void UploadThread(object source, ElapsedEventArgs e)
        {
            Uploader uploadObj = new Uploader();
            Console.WriteLine("Upload Started  :  " + Convert.ToString(DateTime.Now));
            uploadObj.GenerateUploadData(_servicList);           
        }
        public void UploadThread()
        {
            Uploader uploadObj = new Uploader();
           
            uploadObj.GenerateUploadData(_servicList);
        }
        public void DownloadThread()
        {
            Downloader downloadObj = new Downloader();
            
            downloadObj.DownLoadAndProcess(_servicList);
        }
    }
}
