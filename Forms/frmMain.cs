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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Config.WebsocketServerPort;
            ChangeFormState();
        }

        private void btnViewClients_Click(object sender, EventArgs e)
        {
            Form frmConnectedClients = new Forms.frmConnectedClients();
            frmConnectedClients.Show();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (ServerController.isServerStarted)
            {
                ServerController.Stop();
            }   
            else
            {
                Config.WebsocketServerPort = textBox1.Text;
                Config.SaveConfig();
                ServerController.Start();
            }           
            ChangeFormState();
        }

        private void ChangeFormState()
        { 
        Debug.WriteLine(ServerController.isServerStarted);
            if (ServerController.isServerStarted)
            {
                btnStartStop.Text = "Stop Server";
                btnStartStop.ForeColor = Color.Red;
                textBox1.Enabled = false;
            }
            else
            {
                btnStartStop.Text = "Start Server";
                btnStartStop.ForeColor = Color.Blue;
                textBox1.Enabled = true;
            }
        }
    }
}
