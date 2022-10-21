using System;
using System.IO;
using Rebex.Net;

namespace SftpClient
{

    class Program
    {
        public static string destinationFolder = "/TempRemote";
        public static string sourceFolder = "TempLocal";
        static void Main(string[] args)
        {
            string sourceFolderPath = string.Concat(Directory.GetCurrentDirectory().Split("bin")[0], sourceFolder);
            Rebex.Licensing.Key = "==AQvZmqlPuRbTpnMeT5srInrjQt3u7t4XzzBgEQCRU8Zg==";
            // create sftpClient and connect
            Sftp sftpClient = new Sftp();

            // to verify the server//s fingerprint:
            //   a) check sftpClient.Fingerprint property after the
            //      //Connect// method call
            //    - or -
            //   b) use sftpClient.FingerprintCheck event handler
            //      to implement a fingerprint checker
            sftpClient.Connect("sftptest.trizettoprovider.com");
            // verify the server//s fingerprint here unless using the event handler

            // authenticate
            sftpClient.Login("T18", "835wxF3r");

            // browse directories, transfer files, etc...
            if (!sftpClient.DirectoryExists(destinationFolder)) 
            {
                sftpClient.CreateDirectory(destinationFolder);
            }
            sftpClient.ChangeDirectory(destinationFolder);

            foreach (var localFile in Directory.GetFiles(sourceFolderPath))
            {
                var localFileName = Path.GetFileName(localFile);
                if (localFileName != null)
                {
                    string tempRemoteFileName = string.Concat("EDI_", DateTime.Now.Ticks.ToString(), ".txt");
                    using (var localFileStream = File.OpenRead(localFile))
                    {
                        //Initiating upload
                        sftpClient.PutFile(localFileStream, tempRemoteFileName);
                        localFileStream.Close();
                    }
                }
            }

            // disconnect
            sftpClient.Disconnect();
        }
    }
}
