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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrchidRelayServer.Classes.WebSocketServerControllers
{

    class RestaurantManagerService : WebSocketBehavior
    {

        private bool isRegistered = false;
        private ConnectedClient clientInfo;

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

        private bool RegisterClient(string data)
        {
            if (data[0] == '{')
            {
                clientInfo = ConnectedClient.CreateFromJSON(data);
                if (clientInfo != null)
                {
                    clientInfo.ws = Context.WebSocket;
                    return true;
                }
            }
            return false;
        }
        #endregion


        #region Events
        protected override void OnOpen()
        {
            
        }

        protected override void OnClose(CloseEventArgs e)
        {
    
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (!isRegistered) isRegistered = RegisterClient(e.data);
        }


        #endregion

    }
}
