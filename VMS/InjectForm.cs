using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VMS
{
    public partial class InjectForm : Form
    {
        public static byte[] dll;
        public static string Process;
        public static bool download;
        public static string url;
        public static string status;
        public static int value = 1;
        public InjectForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            Process = "CSGO";
            download = true;
            url = "YOUR URL";
            Task.Run(Inject.Download);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = status;
            progressBar1.Value = value;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.TopMost = true;
        }
    }
}
