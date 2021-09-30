using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Data.SqlClient;
using System.Data.OleDb;
using Guna.UI2.WinForms;
using System.IO;

namespace Project_Manegement_System_KMC_Water
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.TransparencyKey = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(110)))), ((int)(((byte)(255)))));
        }
        int ConnectionOfflineCount = 0;

        public static bool CheckInternetConnection()
        {            
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("https://onedrive.live.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }            
        }

        void IsOnline()
        {/*
            bool InternetConn = CheckInternetConnection();
            if (!InternetConn)
            {                
                lblConn.Text = "You are offline";
                if (ConnectionOfflineCount % 15 == 0)
                {
                    MessageBox.Show("Can't Connect to database !", "Error Connection !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }                    
                btnLogin.Enabled = false;
                ConnectionOfflineCount += 1;
            }
            else
            {
                ConnectionOfflineCount = 0;
                lblConn.Text = "Connected";
                btnLogin.Enabled = true;
            }*/
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;

            SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = '" + Properties.Settings.Default.DatabaseLocation + "'; Integrated Security = True");
            //Properties.Settings.Default.DatabaseConnection = conn;

            string query = "select Type from login WHERE username='" + txtUserName.Text + "' AND password='" + txtPassword.Text + "' ;";

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader readerSQL = cmd.ExecuteReader();
                if (readerSQL.Read())
                {
                    Properties.Settings.Default.LoginType = readerSQL.GetString(readerSQL.GetOrdinal("Type")).ToString().Trim();

                    if (readerSQL.GetString(readerSQL.GetOrdinal("Type")).ToString().Trim() == "Admin")
                    {
                        Properties.Settings.Default.User = "Admin";
                        DashBoard form = new DashBoard();
                        form.Show();
                    }
                    else
                    {
                        Properties.Settings.Default.User = "User";
                        //DashBoard form = new DashBoard();
                        //DataCommunication form = new DataCommunication();
                        //form.Show();
                    }
                    Properties.Settings.Default.Save();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password !", "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Can't Access to the Database, Please try again later.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UseWaitCursor = false;
            conn.Close();
        }

        private void TimerCheckConn_Tick(object sender, EventArgs e)
        {
            //IsOnline();
        }
                
        private void Login_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void lblContinue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Properties.Settings.Default.LoginType = "G";
            WaterProduction form = new WaterProduction();
            form.Show();
            this.Hide();
        }

        private void btnabout_DoubleClick(object sender, EventArgs e)
        {
            lblContinue.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblLogin_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MDF (*.mdf)|*.mdf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter DatabasePath = new StreamWriter("Path.txt");
                DatabasePath.WriteLine(ofd.FileName);
                DatabasePath.Close();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}