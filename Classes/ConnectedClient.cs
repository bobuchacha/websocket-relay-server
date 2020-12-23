using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;

namespace OrchidRelayServer.Classes
{
    class ConnectedClient
    {
        public string IPAddress {get; set;}
        public string DeviceUUID { get; set; }
        public string AccountID { get; set; }
        public string AccessToken { get; set; }
        public string ApplicationName { get; set; }
        public bool IsController { get; set; }
        public bool IsConnected { get; set; }
        public bool IsDevelopment { get; set; }
        public DateTime ConnectedAt { get; set; }
        
        public string SessionID { get; set; }

        private bool _addedToTable;
        private WebSocketSharp.WebSocket _ws;

        public ConnectedClient()
        {
            ConnectedAt = DateTime.Now;
        }

        public WebSocketSharp.WebSocket WebSocketInstance()
        {
            return _ws;
        }
        public void WebSocketInstance(WebSocketSharp.WebSocket ws)
        {
            _ws = ws;
        }

        public bool isAdded()
        {
            return _addedToTable;
        }

        public void isAdded(bool set)
        {
            _addedToTable = set;
        }
        
        /// <summary>
        /// create a new ConnectedClient from json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ConnectedClient CreateFromJSON(string json)
        {
            JsonSerializer serializer = new JsonSerializer();
            JObject o = JObject.Parse(json);
            ConnectedClient newClient = (ConnectedClient)serializer.Deserialize(new JTokenReader(o), typeof(ConnectedClient));

            return newClient;

        }
    }
}
