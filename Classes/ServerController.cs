using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Net;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Diagnostics;

namespace OrchidRelayServer.Classes
{
    class ServerController
    {

        public static Boolean isServerStarted = false;

        static WebSocketServer WSSServer;
        private static HttpServer httpServer;

        public static void LogInfo(string str)
        {
            //WSSServer.Log.Info(str);
            httpServer.Log.Info(str);
        }
        public static void LogDebug(string str)
        {
            //WSSServer.Log.Debug(str);
            httpServer.Log.Debug(str);
        }
        public static void LogWarn(string str)
        {
            //WSSServer.Log.Warn(str);
            httpServer.Log.Warn(str);
        }
        public static void LogError(string str)
        {
            //WSSServer.Log.Error(str);
            httpServer.Log.Error(str);
        }

        public static void InitializeWebsocketServer()
        {

            //WSSServer = new WebSocketServer(IPAddress.Any, Convert.ToInt16(Config.WebsocketServerPort));
            //WSSServer.Log.File = Config.LogFileName;
            //WSSServer.Log.Level = LogLevel.Warn;

            //WSSServer.AddWebSocketService<WebSocketServerControllers.RestaurantManagerService>("/rms");                       // default, legacy module
            //WSSServer.AddWebSocketService<WebSocketServerControllers.UtilityService>("/utility");                       // default, legacy module
            //WSSServer.KeepClean = false;

            // merging to v2 - new interface
            httpServer = new HttpServer(IPAddress.Any, Convert.ToInt16(Config.WebsocketServerPort));
            httpServer.AddWebSocketService<WebSocketServerControllers.RestaurantManagerService>("/rms");            // compatibility to legacy standalone print server
            httpServer.AddWebSocketService<WebSocketServerControllers.UtilityService>("/utility");                  // backward compatibility websocket utility server

            httpServer.Log.File = "log.txt";
            httpServer.Log.Level = LogLevel.Debug;
            httpServer.Log.Debug("Web Server started");
            httpServer.KeepClean = false;

            httpServer.OnGet += HttpControllers.GET.Handle;
            //httpServer.OnPost += HttpControllers.POST.Handle;

        }

        public static void Start()
        {
            
            isServerStarted = true;
            //WSSServer.Start();
            httpServer.Start();
            LogDebug("Server started. Listening to port " + Config.WebsocketServerPort);
            if (httpServer.IsListening) TrayIcon.NotifyUser("Server started", "Websocket server has been starting and listening to port " + Config.WebsocketServerPort);
        }
        public static void Stop()
        {
            isServerStarted = false;
            try
            {
                // WSSServer.Stop();
                httpServer.Stop();

            }catch(Exception e)
            {
                LogError(e.ToString());
                Debug.Write(e);
            }

            if (!httpServer.IsListening) TrayIcon.NotifyUser("Server stopped", "Websocket server has been stopped");

        }


    }
}
