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
        public SendPayload(Main mainForm)
        {
            this.Font = mainForm.Font;
            InitializeComponent();
            ApplyUI(mainForm.langJson);
            VersionComboBox.Items.AddRange(Constant.Versions);
        }

        public void ApplyUI(LanguageJson langJson)
        {
            try
            {
                if (langJson != null)
                {
                    SendPayloadBtn.Text = langJson.SendPayloadForm.SendPayloadBtn;
                    OKBtn.Text          = langJson.SendPayloadForm.OKBtn;
                }
            }
            catch (Exception ex)
            {
                InputBox.MsgBox("Apply UI language Exception", "", ex.Message, 100);
            }
            try
            {
                Opacity = Properties.Settings.Default.UIOpacity.Value;
                ForeColor = Properties.Settings.Default.UiForeColor.Value; //Color.White;
                BackColor = Properties.Settings.Default.UiBackColor.Value; //Color.FromArgb(36, 36, 36);
                statusStrip1.BackColor = Properties.Settings.Default.SendPayloadStatusStrip1BackColor.Value; //Color.Silver;

                ToolStripMsg.ForeColor    = ForeColor;
                ToolStripMsg.BackColor    = Color.Transparent;
                VersionComboBox.ForeColor = ForeColor;
                VersionComboBox.BackColor = BackColor;
                IpBox.ForeColor           = ForeColor;
                IpBox.BackColor           = BackColor;
                PortBox.ForeColor         = ForeColor;
                PortBox.BackColor         = BackColor;
                SendPayloadBtn.ForeColor  = ForeColor;
                SendPayloadBtn.BackColor  = BackColor;
                OKBtn.ForeColor           = ForeColor;
                OKBtn.BackColor           = BackColor;
                VersionBox.ForeColor      = ForeColor;
                VersionBox.BackColor      = BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":ApplyUI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
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
                return;
            }
            bool isConnected = false;
            string msg = "";
            try { isConnected = PS4Tool.Connect(IpBox.Text, out msg, 1000, true); }
            catch (Exception) { }
            ToolStripMsg.ForeColor = Color.Red;
            ToolStripMsg.Text = msg;
            if (isConnected) Close();
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

                string payloadsPath = string.Format(@"{0}\payloads\", Application.StartupPath);
                string patchPath = payloadsPath;
                string patchName = "ps5debug.elf";
                if (!File.Exists(patchPath + patchName))
                {
                    patchName = "ps4debug.bin";
                    if (!File.Exists(patchPath + patchName)) patchPath = string.Format(@"{0}\payloads\{1}\", Application.StartupPath, VersionComboBox.SelectedItem);
                    if (!File.Exists(patchPath + patchName)) throw new ArgumentException(string.Format("payload not found! ({0})\n" +
                        "You must manually place the downloaded ps4debug.bin or ps5debug.elf in the folder \n\\Path\\to\\PS4CheaterNeo\\payloads\\", payloadsPath));
                }
                SendPayLoad(IpBox.Text, Convert.ToInt32(PortBox.Text), patchPath + patchName);
                Thread.Sleep(1000);

                ToolStripMsg.ForeColor = Color.Green;
                ToolStripMsg.Text = patchName + " injected successfully!";
            }
            catch (Exception ex)
            {
                ToolStripMsg.ForeColor = Color.Red;
                ToolStripMsg.Text = "SendPayloadBtn_Click Exception";
                InputBox.MsgBox("SendPayloadBtn_Click Exception", "", ex.Message, 100);
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
            if ((Properties.Settings.Default.PS4FWVersion.Value ?? "") != "") defaultVer = Properties.Settings.Default.PS4FWVersion.Value;
            if ((Properties.Settings.Default.PS4IP.Value ?? "") != "") IpBox.Text = Properties.Settings.Default.PS4IP.Value;
            if (Properties.Settings.Default.PS4Port.Value != 0) PortBox.Text = Convert.ToString(Properties.Settings.Default.PS4Port.Value);

            VersionBox.Text = defaultVer;
            VersionComboBox.SelectedIndex = VersionComboBox.Items.IndexOf(defaultVer);
        }

        private void SendPayload_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if ((VersionBox.Text ?? "") != "") Properties.Settings.Default.PS4FWVersion.Value = VersionBox.Text;
                if ((IpBox.Text ?? "") != "") Properties.Settings.Default.PS4IP.Value = IpBox.Text;
                if ((PortBox.Text ?? "") != "") Properties.Settings.Default.PS4Port.Value = Convert.ToUInt16(PortBox.Text);
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                if (ToolStripMsg.ForeColor != Color.Red) e.Cancel = true;
                ToolStripMsg.ForeColor = Color.Red;
                ToolStripMsg.Text = ex.Message;
            }
        }

        private void VersionComboBox_SelectedIndexChanged(object sender, EventArgs e) => VersionBox.Text = (string)VersionComboBox.SelectedItem;
    }
}
