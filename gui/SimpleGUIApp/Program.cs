using System;
using System.Threading;
using System.Windows.Forms;

namespace SimpleGUIApp
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            
            // Kjør manuelle tester i en separat tråd
            Thread testThread = new Thread(() =>
            {
                SimpleGUIApp.Tests.ManualTests.RunTests();
            });
            testThread.Start();
            testThread.Join();  // La testene kjøre ferdig før GUI starter

            // Start hovedprogrammet (GUI-applikasjonen)
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
