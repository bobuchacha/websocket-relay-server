using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrchidRelayServer.Classes;
/*
 * this is a static class to store connected clients information in memory
 */
namespace OrchidRelayServer
{
    class ConnectedClients
    {
        private static List<ConnectedClient> _clients = new List<ConnectedClient> { };
        private static string[] ColumnNames = {"IP Address", "Device UUID", "Account ID", "Access Token", "Application Name", "Is Controller", "Is Connected", "Connected At"};

        public static void Add(ConnectedClient client)
        {
            _clients.Add(client);
        }
        public static void Remove(ConnectedClient client)
        {
            _clients.Remove(client);
        }

        public static DataTable GetDataTable()
        {
           
            DataTable dt = new DataTable();
            dt.Columns.Add(ColumnNames[0], typeof(string));
            dt.Columns.Add(ColumnNames[1], typeof(string));
            dt.Columns.Add(ColumnNames[2], typeof(string));
            dt.Columns.Add(ColumnNames[3], typeof(string));
            dt.Columns.Add(ColumnNames[4], typeof(string));
            dt.Columns.Add(ColumnNames[5], typeof(bool));
            dt.Columns.Add(ColumnNames[6], typeof(bool));
            dt.Columns.Add(ColumnNames[7], typeof(DateTime));

            foreach (ConnectedClient client in _clients)
            {
                DataRow row = dt.NewRow();
                row[ColumnNames[0]] = client.IPAddress;
                row[ColumnNames[1]] = client.DeviceUUID;
                row[ColumnNames[2]] = client.AccountID;
                row[ColumnNames[3]] = client.AccessToken;
                row[ColumnNames[4]] = client.ApplicationName;
                row[ColumnNames[5]] = client.IsController;
                row[ColumnNames[6]] = client.IsConnected;                
                row[ColumnNames[7]] = client.ConnectedAt;                
                dt.Rows.Add(row);
            }
            return dt;
        }
    }

}
