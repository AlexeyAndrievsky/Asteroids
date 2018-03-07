using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        OpenFileDialog ofd = new OpenFileDialog();
        StringBuilder buffer = new StringBuilder(128);
        int second;
        int minutes;
        string CommandString;
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);
        public Form1()
        {
            InitializeComponent();
            txtpath.ReadOnly = true;
        }

        private void btnplay_Click(object sender, EventArgs e)
        {
            if (ofd.FileName == "")
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ofd.Filter = "MP3 Files|*.mp3";
                    CommandString = "open " + "\"" + ofd.FileName + "\"" + " type MPEGVideo alias Mp3File";
                    mciSendString(CommandString, null, 0, 0);
                    CommandString = "play Mp3File";
                    mciSendString(CommandString, null, 0, 0);
                    timer1.Enabled = true;
                }
            }

            else
            {
                CommandString = "play Mp3File";
                mciSendString(CommandString, null, 0, 0);

                timer1.Enabled = true;
                //timer1.Start();
            }
        }

        private void btnpause_Click(object sender, EventArgs e)
        {
            CommandString = "pause mp3file";
            mciSendString(CommandString, null, 0, 0);
            timer1.Stop();
        }

        private void btnbrowse_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Mp3 files |*.mp3";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtpath.Text = ofd.FileName;
                CommandString = "close Mp3File";
                mciSendString(CommandString, null, 0, 0);
                timer1.Enabled = false;
                CommandString = "open " + "\"" + ofd.FileName + "\"" + " type MPEGVideo alias Mp3File";
                mciSendString(CommandString, null, 0, 0);
                
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CommandString = "Status Mp3File position";
            mciSendString(CommandString, buffer, 128, 0);
            second = int.Parse(buffer.ToString());
            second = second / 1000;
            minutes = second / 60;
            second = second % 60;
            lbltime.Text = minutes.ToString("00") + ":" + second.ToString("00");
        }
    }
}
