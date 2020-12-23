using OrchidRelayServer.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrchidRelayServer
{
    static class Program
    {
        private static string _interprocessID;
        private static Mutex _appSingletonMaker;
        public static string AppVersion;
        public static Form frmMain;
        public static Form frmConnectedClients;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // get the GUID of the assembly, and initialize the Mutex using that GUID
            _interprocessID = Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value.ToUpper();
            _appSingletonMaker = new Mutex(true, _interprocessID);

            // Application Instance has not initialized yet. We initialize the server
            if (_appSingletonMaker.WaitOne(TimeSpan.Zero, true))
                InitializeApplication();                        // initialize Program
            else
                SendArgumentsToInitializedInstance();           // send arguments to running instance

           
        }

        static void InitializeApplication()
        {
            // load saved configuration
            Config.LoadSavedConfigs();

            // initialize server
            ServerController.InitializeWebsocketServer();

            // delete log file name if we dont keep it
            if (Config.PrependLogFile != "yes" && Config.PrependLogFile != "true")
            {
                File.Delete(Config.LogFileName);
            }

            // listen to arguments when another instance started
            Task.Run(() =>
            {
                using (var server = new NamedPipeServerStream(_interprocessID))
                {
                    using (var reader = new StreamReader(server))
                    {
                        using (var writer = new StreamWriter(server))
                        {

                            // this infinity loop is to receive any thing from the other instance
                            while (true)
                            {
                                server.WaitForConnection();
                                var incomingArgs = reader.ReadLine().Split('\t');
                                server.Disconnect();
     
                            }
                        }
                    }
                }
            });

            Application.EnableVisualStyles();

            // release mutex
            _appSingletonMaker.ReleaseMutex();

            // app version
            AppVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

            // start the web server
            if (Config.WebsocketServerPort != null) ServerController.Start();


            // install tray icon
            InitializeTrayIconEvents();
            TrayIcon.InitializeTrayIcon();

            Application.SetCompatibleTextRenderingDefault(false);
            
        }

        private static void InitializeTrayIconEvents()
        {
            TrayIcon.AddEventHandler(TrayIcon.EventType.ShowMainWindow, (s, e) => {
                frmMain = new Forms.frmMain();
                frmMain.Show();
            });
            TrayIcon.AddEventHandler(TrayIcon.EventType.ShowConnectedClientsWindow, (s, e) => {
                frmConnectedClients = new Forms.frmConnectedClients();
                frmConnectedClients.Show();
            });
            TrayIcon.AddEventHandler(TrayIcon.EventType.BeforeExit, (s, e) =>
            {
                ServerController.Stop();
            });
        }

        static void SendArgumentsToInitializedInstance() { /* do nothing here */ }
    }
}
