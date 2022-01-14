using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public partial class SendPayload : Form
    {
        public SendPayload()
        {
            InitializeComponent();
            VersionComboBox.Items.AddRange(Constant.Versions);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            string errorMsg = "";
            if ((VersionBox.Text ?? "") == "") errorMsg = "Unknown Version.";
            else if ((IpBox.Text ?? "") == "") errorMsg = "Unknown IP.";
            else if ((PortBox.Text ?? "") == "") errorMsg = "Unknown Port.";

            if (errorMsg.Length > 0)
            {
                ToolStripMsg.ForeColor = Color.Red;
                ToolStripMsg.Text = errorMsg;
            }
            else
            {
                bool isConnected = false;
                string msg = "";
                try { isConnected = PS4Tool.Connect(IpBox.Text, out msg, 1000); }
                catch (Exception) {}
                ToolStripMsg.ForeColor = Color.Red;
                ToolStripMsg.Text = msg;
                if (isConnected) Close();
            }
        }

        private void SendPayloadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string errorMsg = "";
                if ((VersionBox.Text ?? "") == "") errorMsg = "Unknown Version.";
                else if ((IpBox.Text ?? "") == "") errorMsg = "Unknown IP.";
                else if ((PortBox.Text ?? "") == "") errorMsg = "Unknown Port.";
                if (errorMsg.Length > 0)
                {
                    ToolStripMsg.ForeColor = Color.Red;
                    ToolStripMsg.Text = errorMsg;
                    return;
                }

                string patchPath = string.Format(@"{0}\payloads\{1}\", Application.StartupPath, VersionComboBox.SelectedItem);
                if (!File.Exists(patchPath + @"ps4debug.bin"))
                {
                    throw new ArgumentException(string.Format("ps4debug.bin({0}) not found!", patchPath));
                }
                SendPayLoad(IpBox.Text, Convert.ToInt32(PortBox.Text), patchPath + @"ps4debug.bin");
                Thread.Sleep(1000);

                ToolStripMsg.ForeColor = Color.Green;
                ToolStripMsg.Text = "ps4debug.bin injected successfully!";
            }
            catch (Exception exception)
            {
                ToolStripMsg.ForeColor = Color.Red;
                ToolStripMsg.Text = exception.Message;
            }
        }

        private void SendPayLoad(string IP, int port, string payloadPath)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(new IPEndPoint(IPAddress.Parse(IP), port));
            socket.SendFile(payloadPath);
            socket.Close();
        }

        private void SendPayload_Load(object sender, EventArgs e)
        {

            string defaultVer = Constant.Versions[0];
            if (Properties.Settings.Default.PS4FWVersion.Value is string version && (version ?? "") != "") defaultVer = version;
            if (Properties.Settings.Default.PS4IP.Value is string ip && (ip ?? "") != "") IpBox.Text = ip;
            if (Properties.Settings.Default.PS4Port.Value is uint port && port != 0) PortBox.Text = Convert.ToString(port);

            VersionComboBox.SelectedIndex = VersionComboBox.Items.IndexOf(defaultVer);
        }

        private void SendPayload_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if ((VersionBox.Text ?? "") != "") Properties.Settings.Default.PS4FWVersion.Value = VersionBox.Text;
                if ((IpBox.Text ?? "") != "") Properties.Settings.Default.PS4IP.Value = IpBox.Text;
                if ((PortBox.Text ?? "") != "") Properties.Settings.Default.PS4Port.Value = Convert.ToUInt32(PortBox.Text);
                Properties.Settings.Default.Save();
            }
            catch (Exception exception)
            {
                ToolStripMsg.ForeColor = Color.Red;
                ToolStripMsg.Text = exception.Message;
            }
        }

        private void VersionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            VersionBox.Text = (string)VersionComboBox.SelectedItem;
        }

        private void ToolStripMsg_MouseHover(object sender, EventArgs e)
        {
            if ((ToolStripMsg.Text ?? "").Trim().Length == 0) return;

            if (MsgBox.form == null || MsgBox.form.IsDisposed) MsgBox.Show(this, ToolStripMsg.Text);
            else MsgBox.label.Text = ToolStripMsg.Text;
            MsgBox.form.Refresh();
        }

        private void ToolStripMsg_TextChanged(object sender, EventArgs e)
        {
            if (MsgBox.form == null || MsgBox.form.IsDisposed) return;

            MsgBox.label.Text = ToolStripMsg.Text;
            MsgBox.form.Refresh();
        }
    }
}
