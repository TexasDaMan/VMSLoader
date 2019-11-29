using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using ManualMapInjection.Injection;

namespace VMS
{
    internal class Inject
    {
        private void inj()
        {
            if (InjectForm.Process == "CSGO")
            {
                CSINJ();
            }
        }

        public static void Download()
        {
            InjectForm.status = "Downloading";
            if (Process.GetProcessesByName("csgo").Length == 0)
            {
                HttpWebRequest.DefaultWebProxy = new WebProxy();
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                client.DownloadDataCompleted += DownloadDataCompleted;
                client.DownloadDataAsync(new Uri(InjectForm.url));
            }
            else
            {
                MessageBox.Show("You must have CSGO Closed");
            }
        }

        public static void next()
        {
            CSINJ();
        }

        public static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            InjectForm.status = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
            InjectForm.value = int.Parse(Math.Truncate(percentage).ToString());
        }

        public static void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            InjectForm.dll = e.Result;
            InjectForm.status = "Download Finished";
            next();
        }

        public static void CSINJ()
        {
            while (System.Diagnostics.Process.GetProcessesByName(InjectForm.Process).Length == 0) //if csgo isnt started
            {
                InjectForm.status = "Scanning for CSGO";
                InjectForm.value = 1;
                Thread.Sleep(500); //sleeps for .5 seconds
            }

            bool Clientdll_Found = false; //initialize client_found with false
            bool Enginedll_Found = false; //initialize engine_found with false
            bool Serverdll_Found = false;

            do
            {
                InjectForm.status = "Scanning for Panorama";
                Process[] CheckModules = System.Diagnostics.Process.GetProcessesByName(InjectForm.Process);

                foreach (ProcessModule m in CheckModules[0].Modules)
                {
                    if (m.ModuleName == "client_panorama.dll") //this is to check if client_panorama.dll is loaded
                    {
                        Clientdll_Found = true;
                        InjectForm.value = 25;
                    }
                }
            } while (Clientdll_Found == false); //loop while not loaded

            do
            {
                InjectForm.status = "Scanning for Engine";
                Process[] CheckModules = System.Diagnostics.Process.GetProcessesByName(InjectForm.Process);

                foreach (ProcessModule m in CheckModules[0].Modules)
                {
                    if (m.ModuleName == "engine.dll") //this is to check if engine.dll is loaded
                    {
                        InjectForm.value = 50;
                        Enginedll_Found = true;
                    }
                }
            } while (Enginedll_Found == false); //loop while not loaded
            do
            {
                InjectForm.status = "Scanning for Server Browser ";
                Process[] CheckModules = System.Diagnostics.Process.GetProcessesByName(InjectForm.Process);

                foreach (ProcessModule m in CheckModules[0].Modules)
                {
                    if (m.ModuleName == "serverbrowser.dll") //this is to check if engine.dll is loaded
                    {
                        InjectForm.value = 75;
                        Serverdll_Found = true;
                    }
                }
            } while (Serverdll_Found == false);

            if (Clientdll_Found == true && Enginedll_Found == true && Serverdll_Found == true) //if its loaded
            {
                Thread.Sleep(3000);
                var name = "csgo";
                var target = Process.GetProcessesByName(name).FirstOrDefault();

                var injector = new ManualMapInjector(target) { AsyncInjection = true };

                injector.Inject(InjectForm.dll).ToInt64();

            }
            Application.Exit();
        }
    }
}