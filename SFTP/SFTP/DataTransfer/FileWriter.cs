using System.IO;
using System.Collections.Generic;
using SFTP.Models;

namespace SFTP.DataTransfer
{
   public static class FileWriter
    {
        public static void WriteFile(string localDir,string fileName,List<string> data=null)
        {
            
            if (!Directory.Exists(localDir))
            {
                Directory.CreateDirectory(localDir);

            }
            if (!File.Exists(localDir+fileName))
            {

                using (StreamWriter sw = File.CreateText(localDir+fileName))
                {
                    if (data!=null)
                    {
                        if (data.Count > 0)
                        {

                            foreach (var dataRow in data)
                                sw.WriteLine(dataRow.ToString());
                            // string dataToWrite = string.Join("|", data);

                        }
                    }
                }
            }
        }
    }
}
