using Access2Justice.DataImportTool.BusinessLogic;
using Microsoft.Identity.Client;
using System;
using System.Windows.Forms;

namespace Access2Justice.DataImportTool
{
    public partial class DataImport : Form
    {
        public DataImport()
        {
            InitializeComponent();
        }

         static AuthenticationResult userResponse = null;        

        private void Login_Click(object sender, EventArgs e)
        {
            if (button1.Text.ToLowerInvariant() == "login")
            {
                Authentication.Authentication authentication = new Authentication.Authentication();
                userResponse = authentication.Login().Result;
                label1.Text = userResponse?.Account?.Username;
                button1.Text = "Logout";
            }
            else
            {
                Authentication.Authentication authentication = new Authentication.Authentication();
                var response = authentication.Logout().Result;
                label1.Text = "";
                button1.Text = "Login";
            }
        }

        private void ImportTopics_Click(object sender, EventArgs e)
        {
            try
            {
                if (userResponse?.AccessToken != null)
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.ShowDialog();
                    openFileDialog1.Filter = "*.xlsx|*.xls";
                    if (string.IsNullOrEmpty(openFileDialog1.FileName))
                    {
                        MessageBox.Show("Please select proper excel file to process the Topics data");
                    }
                    else
                    {
                        TopicBusinessLogic.GetTopics(userResponse.AccessToken, openFileDialog1.FileName).Wait();
                    }
                }
                else
                {
                    MessageBox.Show("Please login, before we process the data");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ImportResources_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
