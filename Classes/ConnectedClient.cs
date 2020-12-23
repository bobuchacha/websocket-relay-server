using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DateTime ConnectedAt { get; set; }
        public WebSocketSharp.WebSocket ws;

        public ConnectedClient()
        {
            ConnectedAt = DateTime.Now;
        }

        /// <summary>
        /// create a new ConnectedClient from json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ConnectedClient CreateFromJSON(string json)
        {
            ConnectedClient newClient = JsonSerializer.Deserialize<ConnectedClient>(json);

            return newClient;
        }
    }
}
