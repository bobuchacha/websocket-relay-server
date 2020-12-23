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
            gridMain.DataSource = ConnectedClients.GetDataTable();
            gridMain.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void gridMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        ~frmConnectedClients()
        {

            Debug.Write("AS");
        }
    }
}
