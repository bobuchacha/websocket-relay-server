using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace OrchidRelayServer.Classes.WebSocketServerControllers
{

    class UtilityService : WebSocketBehavior
    {

    
        #region Construction
        /// <summary>
        /// construction. when a client connected, this class will be constructed for that client only
        /// </summary>
        public UtilityService() : this(null) { }
        public UtilityService(string prefix) { }
        #endregion




        #region Events
        protected override void OnOpen()
        {
            Context.WebSocket.Send("Welcome to Orchid WebSocket Server Utility version " + Program.AppVersion);
        }

        protected override void OnClose(CloseEventArgs e)
        {

        }

        protected override void OnMessage(MessageEventArgs e)
        {
            try
            {
                string JSON;

                JsonSerializer serializer = new JsonSerializer();
                JObject o = JObject.Parse(e.Data);
                JToken command = o.GetValue("command");
                JToken param = o.GetValue("param");

                if (command == null) throw new Exception("Invalid request format");

                switch (command.ToString())
                {
                    case "getConnectedDevices":
                    case "get-connected-devices":
                        JSON = JsonConvert.SerializeObject(ConnectedClients.AllByAccountId(param.ToString()));
                        Success(JSON);
                        break;

                    case "getConnectedDevDevices":
                    case "get-connected-dev-devices":
                        JSON = JsonConvert.SerializeObject(ConnectedClients.AllDevByAccountId(param.ToString()));
                        Success(JSON);
                        break;

                    default:
                        throw new Exception("Invalid Request " + command.ToString());
                }

            }catch(Exception err)
            {
                Context.WebSocket.Send("{\"success\":false, \"error\": \"" + err.Message + "\"}");
            }
        }


        #endregion

        private void Success(string Payload)
        {
            Payload = Payload.Replace('"', '\"');
            Context.WebSocket.Send("{\"success\":true, \"payload\": " + Payload + "}");
        }
    }
}
