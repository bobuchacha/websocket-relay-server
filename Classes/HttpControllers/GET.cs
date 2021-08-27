using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace OrchidRelayServer.Classes.HttpControllers
{
    class GET
    {
        public static void Handle(object sender, HttpRequestEventArgs e)
        {
            switch (e.Request.Url.LocalPath.ToLower())
            {
                case "/get-connected-devices":
                    ResponseConnectedDevices(e, false);
                    break;

                case "/get-connected-dev-devices":
                    ResponseConnectedDevices(e, true);
                    break;

                case "/get-version":
                    ResponsePrintServerVersion(e);
                    break;

                case "/get-features":
                    ResponseAppFeatures(e);
                    break;

                default:
                    e.Response.StatusCode = 404;
                    break;
            }
        }

        private static void ResponseAppFeatures(HttpRequestEventArgs e)
        {
            String features = "WebSocket\n" +
                "Http\n";


            e.Response.ContentType = "text/plain";
            Utility.ResponseWrite(e.Response, features);
        }

        private static void ResponsePrintServerVersion(HttpRequestEventArgs e)
        {
            e.Response.ContentType = "text/plain";
            Utility.ResponseWrite(e.Response, Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }


        /**
         * Responses printer list in plain text. separate by lines
         **/
        protected static void ResponseConnectedDevices(HttpRequestEventArgs e, Boolean showDevDevices = false)
        {
            string szReturn = "";
            string accountID = e.Request.QueryString.Get("@id");

            e.Response.ContentType = "application/json";

            if (!showDevDevices)
                szReturn = JsonConvert.SerializeObject(ConnectedClients.AllByAccountId(accountID));
            else
                szReturn = JsonConvert.SerializeObject(ConnectedClients.AllDevByAccountId(accountID));

            Success(e, szReturn);            

        }

        private static void Success(HttpRequestEventArgs e, string Payload)
        {
            Payload = Payload.Replace('"', '\"');
            string finalResponse = "{\"success\":true, \"payload\": " + Payload + "}";
            Utility.ResponseWrite(e.Response, finalResponse);
        }
    }
}
