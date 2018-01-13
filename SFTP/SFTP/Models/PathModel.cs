
namespace SFTP.Models
{
    public class PathModel
    {
        public string TopUpPath { get; set; }
        public string TopUpArchivePath { get; set; }
        public string TopupAckPath { get; set; }
        public string TopupAckArchivePath { get; set; }       

        public string StopAccountPath { get; set; }
        public string StopAccountArchivePath { get; set; }
        public string StopAccountAckPath { get; set; }
        public string StopAccountkArchivePath { get; set; }

        public string TransactionsPath { get; set; }
        public string TransactionsArchivePath { get; set; }
        public string TransactionsAckPath { get; set; }
        public string TransactionsArchiveAckPath { get; set; }

        public string FileNamePrefix { get; set; }

    }
}
