using OrchidRelayServer.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrchidRelayServer.Forms
{
    public partial class frmConnectedClients : Form
    {
        public frmConnectedClients()
        {
            InitializeComponent();
        }

        private void frmConnectedClients_Load(object sender, EventArgs e)
        {
            gridMain.EditMode = DataGridViewEditMode.EditProgrammatically;
            gridMain.DataSource = ConnectedClients.GetDataTable();
            refreshTimer.Start();
        }

        private void gridMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        ~frmConnectedClients()
        {
            refreshTimer.Stop();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            ConnectedClients.GetDataTable();    // update DataTable bound to the grid view
        }
    }
}
