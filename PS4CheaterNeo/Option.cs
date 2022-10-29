using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public partial class Option : Form
    {
        public Option()
        {
            InitializeComponent();
            Opacity = Properties.Settings.Default.UIOpacity.Value;

            optionTreeView1.InitSettings(Properties.Settings.Default);
        }
    }
}
