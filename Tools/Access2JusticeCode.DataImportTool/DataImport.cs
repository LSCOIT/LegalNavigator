using Access2Justice.Tools.BusinessLogic;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Access2JusticeCode.DataImportTool
{
    public partial class DataImport : Form
    {
        public DataImport()
        {
            InitializeComponent();
        }

         static AuthenticationResult userResponse = null;

        private void Button1_Click(object sender, EventArgs e)
        {
            Authentication.Authentication authentication = new Authentication.Authentication();
            userResponse = authentication.Login().Result;
            label1.Text = userResponse?.Account?.Username; 
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (userResponse?.AccessToken != null)
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.ShowDialog();
                openFileDialog1.Filter = "*.xlsx|*.xls";
                if (string.IsNullOrEmpty(openFileDialog1.FileName))
                { MessageBox.Show("Please select proper excel file to process the Topics data"); }
                else { TopicBusinessLogic.GetTopics(userResponse.AccessToken, openFileDialog1.FileName).Wait(); }
            }
            else
            {
                MessageBox.Show("Please login, before we process the data");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {   
            if (userResponse?.AccessToken != null)
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.ShowDialog();
                openFileDialog1.Filter = "*.xlsx|*.xls";
                if (string.IsNullOrEmpty(openFileDialog1.FileName))
                { MessageBox.Show("Please select proper excel file to process the Resources data"); }
                else { ResourceBusinessLogic.GetResources(userResponse.AccessToken, openFileDialog1.FileName).Wait(); }
            }
            else
            {
                MessageBox.Show("Please login, before we process the data");
            }
        }
    }
}
