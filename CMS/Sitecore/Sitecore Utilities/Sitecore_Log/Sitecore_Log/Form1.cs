using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Sitecore_Log
{
    public partial class Form1 : Form
    {
        private string file;
        private string fullLog;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseFile = new OpenFileDialog();
            browseFile.Filter = "Text Files (*.txt)|*.txt";
            browseFile.Title = "Browse TXT file";
            if (browseFile.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                txtBrowse.Text = browseFile.FileName;
                file = browseFile.FileName;
                // create reader & open file
                StreamReader tr = new StreamReader(browseFile.FileName);

                // read a line of text
                fullLog = tr.ReadToEnd();
                textDisplay.Text = fullLog;

                // close the stream
                tr.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening file", "File Error",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void updateLog()
        {
            string currentLog = fullLog;

            if (!text.Checked)
            {
                string temp = Regex.Match(currentLog, "[\\d]{3}.*AUDIT.*[\\s]");
                currentLog = Regex.Replace(currentLog, "[\\d]{3}.*WARN.*[\\s]", "");
            }
            if (!chkAudit.Checked)
            {
                currentLog = Regex.Replace(currentLog, "[\\d]{3}.*AUDIT.*[\\s]", "");
            }
            if (!chkError.Checked)
            {
                currentLog = Regex.Replace(currentLog, "[\\d]{3}.*ERROR.*[\\s]", "");
                currentLog = Regex.Replace(currentLog, ".*Exception:.*", "");
                currentLog = Regex.Replace(currentLog, ".*Message:.*", "");
                currentLog = Regex.Replace(currentLog, ".*Source:.*", "");
            }
            if (!chkInfo.Checked)
            {
                currentLog = Regex.Replace(currentLog, "[\\d]{3}.*INFO.*[\\s]", "");                
            }
            if (!chkWarn.Checked)
            {
                currentLog = Regex.Replace(currentLog, "[\\d]{3}.*WARN.*[\\s]", "");
            }
            currentLog = Regex.Replace(currentLog, "[\\s]{5}", "");
            textDisplay.Text = currentLog;
            textDisplay.Update();
        }

        private void chkError_CheckedChanged(object sender, EventArgs e)
        {
            updateLog();    
        }

        private void chkInfo_CheckedChanged(object sender, EventArgs e)
        {
            updateLog();
        }

        private void chkAudit_CheckedChanged(object sender, EventArgs e)
        {
            updateLog();
        }

        private void chkWarn_CheckedChanged(object sender, EventArgs e)
        {
            updateLog();
        }

        private void text_CheckedChanged(object sender, EventArgs e)
        {
            updateLog();
        }
    }
}
