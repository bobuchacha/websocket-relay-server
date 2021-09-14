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

    class RestaurantManagerService : WebSocketBehavior
    {

        private bool _isRegistered = false;
        private ConnectedClient _clientInfo;

        #region Construction
        /// <summary>
        /// construction. when a client connected, this class will be constructed for that client only
        /// </summary>
        public RestaurantManagerService() : this(null) {}
        public RestaurantManagerService(string prefix) {}
        #endregion


        #region Private methods
        /// <summary>
        /// Get MachineID during connection set by QueryString mid
        /// </summary>
        /// <returns></returns>
        private string getQueryString(string key)
        {
            return Context.QueryString[key];
        }

        private bool RegisterClient(string json)
        {
            try
            {
                // check if a ConnectedClient with DeviceUUID is registered. This probably get doubled if connection get fatal
   
                if (json[0] != '{') throw (new Exception("Register Information doesn't seem to be JSON format."));

                // deserialize json
                JsonSerializer serializer = new JsonSerializer();
                JObject o = JObject.Parse(json);
                ConnectedClient clientInfo = (ConnectedClient)serializer.Deserialize(new JTokenReader(o), typeof(ConnectedClient));

                // check the client info
                if (clientInfo == null) throw (new Exception("Can not recognize register data received."));
                if (clientInfo.DeviceUUID == null) throw (new Exception("Device UUID is empty"));
                if (clientInfo.AccountID == null) throw (new Exception("Account ID is not set"));
                if (clientInfo.AccessToken == null) throw (new Exception("Access Token is not set"));
                if (clientInfo.ApplicationName == null) throw (new Exception("Application Name is not set"));

                // set session id and additional props to client info before saving it
                clientInfo.SessionID = ID;
                clientInfo.IPAddress = Context.UserEndPoint.Address.ToString();
                clientInfo.IsConnected = true;
                clientInfo.WebSocketInstance(Context.WebSocket);

                _clientInfo = clientInfo;

                // before adding instance to cache, we need to find if there is any existing instance with same
                // deviceUUID in memory. This is because server sometimes get fatal
                // and server does not close client correctly.
                ConnectedClient duplicatedClient = ConnectedClients.FindByDeviceUUID(clientInfo.DeviceUUID);

                if (duplicatedClient != null)
                {
                    duplicatedClient.WebSocketInstance().Close();
                    ConnectedClients.Remove(duplicatedClient);
                }

                // add instance to cache memory
                ConnectedClients.Add(_clientInfo);

                // send response to client
                Context.WebSocket.Send("Device registered as " + clientInfo.DeviceUUID + ". Account ID " + clientInfo.AccountID);

                return true;

            }
            catch(Exception e)
            {
                Debug.WriteLine("Error: Can not register client. " + e.Message);
                return false;
            }
        }

        private void RelayMessage(string json)
        {
            JsonSerializer serializer = new JsonSerializer();
            JObject o = JObject.Parse(json);
            JToken from = o.GetValue("from");
            JToken to = o.GetValue("to");
            JToken messageId = o.GetValue("messageId");
            
            try
            {
                if (from == null) throw new Exception("Message does not contain origin device uuid.");
                if (to == null) throw new Exception("Message does not contain destination device uuid.");
                if (messageId == null) throw new Exception("Message does not contain unique id.");

                ConnectedClient recipientDevice = ConnectedClients.FindByDeviceUUID(to.ToString());
                if (recipientDevice == null) throw new Exception("Destination Device not connected.");

                recipientDevice.WebSocketInstance().Send(json);
            }
            catch(Exception e)
            {
                _clientInfo.WebSocketInstance().Send("{\"success\":false, \"error\": \"" + e.Message + "\"}");
            }


        }
        #endregion


        #region Events
        protected override void OnOpen()
        {
            Context.WebSocket.EmitOnPing = true;

            SetInterval(() => Context.WebSocket.Ping(), TimeSpan.FromSeconds(30));

            Context.WebSocket.Send("Welcome to Restaurant Manager Remote Management Service vesion " + Program.AppVersion);
            Debug.WriteLine("New Client connected. Session ID: " + ID);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Debug.WriteLine("Client disconnected. Session ID: " + ID);
            ConnectedClients.Remove(_clientInfo);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsPing)
            {
                Context.WebSocket.Send(e.Data);
                return;
            }

            if (!_isRegistered)
            {
                Debug.WriteLine("Received message. Client not registered. Registering now");
                _isRegistered = RegisterClient(e.Data);
                return;
            }

            RelayMessage(e.Data);

        }


        #endregion


        public static async Task SetInterval(Action action, TimeSpan timeout)
        {
            await Task.Delay(timeout).ConfigureAwait(false);

            action();

            SetInterval(action, timeout);
        }
    }
}
