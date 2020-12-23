using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrchidRelayServer.Classes;
using System.Diagnostics;
/*
* this is a static class to store connected clients information in memory
*/
namespace OrchidRelayServer
{
    class ConnectedClients
    {
        private static List<ConnectedClient> _clients = new List<ConnectedClient> { };
        private static string[] ColumnNames = {"IP Address", "Device UUID", "Account ID", "Access Token", "Application Name", "Controller", "Alive", "Connected At", "Development Device"};
        private static DataTable dt;

        public static void Add(ConnectedClient client)
        {
            _clients.Add(client);
        }
        public static void Remove(ConnectedClient client)
        {
            _clients.Remove(client);
        }

        public static List<ConnectedClient> All()
        {
            return _clients;
        }
        public static List<ConnectedClient> AllByAccountId(string accountId)
        {
            List<ConnectedClient> result = new List<ConnectedClient> { };
            foreach (ConnectedClient client in _clients)
            {
                if (client.AccountID == accountId)
                {
                    result.Add(client);
                }
            }
            return result;
        }
        public static List<ConnectedClient> AllDevByAccountId(string accountId)
        {
            List<ConnectedClient> result = new List<ConnectedClient> { };
            foreach (ConnectedClient client in _clients)
            {
                if (client.AccountID == accountId && client.IsDevelopment == true)
                {
                    result.Add(client);
                }
            }
            return result;
        }


        public static ConnectedClient FindByDeviceUUID(string deviceId)
        {
            foreach(ConnectedClient client in _clients)
            {
                 if (client.DeviceUUID == deviceId)
                {
                    return client;
                }
            }
            return null;
        }

        public static DataTable GetDataTable()
        {
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add(ColumnNames[0], typeof(string));
                dt.Columns.Add(ColumnNames[1], typeof(string));
                dt.Columns.Add(ColumnNames[2], typeof(string));
                dt.Columns.Add(ColumnNames[3], typeof(string));
                dt.Columns.Add(ColumnNames[4], typeof(string));
                dt.Columns.Add(ColumnNames[5], typeof(bool));
                dt.Columns.Add(ColumnNames[6], typeof(bool));
                dt.Columns.Add(ColumnNames[8], typeof(bool));       // development
                dt.Columns.Add(ColumnNames[7], typeof(DateTime));
            }

            dt.Rows.Clear();

            foreach (ConnectedClient client in _clients)
            {
                //if (client.isAdded()) continue;
                //client.isAdded(true);
                
                DataRow row = dt.NewRow();
                row[ColumnNames[0]] = client.IPAddress;
                row[ColumnNames[1]] = client.DeviceUUID;
                row[ColumnNames[2]] = client.AccountID;
                row[ColumnNames[3]] = client.AccessToken;
                row[ColumnNames[4]] = client.ApplicationName;
                row[ColumnNames[5]] = client.IsController;
                row[ColumnNames[7]] = client.ConnectedAt;
                row[ColumnNames[8]] = client.IsDevelopment;
                row[ColumnNames[7]] = client.ConnectedAt;
                dt.Rows.Add(row);
            }


            return dt;
        }
    }

}
