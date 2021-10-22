using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Guna.UI2.WinForms;

namespace Project_Manegement_System___KMC
{
    public partial class AddNew : Form
    {
        public AddNew()
        {
            InitializeComponent();
        }

        string Permission = null, MethodOfExecution = null;
        SQLFunctions sqlFunctions = new SQLFunctions();

        private void AddNew_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void btnMethordofExecution_Click(object sender, EventArgs e)
        {
            btnMethordofExecutionS.FillColor = btnMethordofExecutionD.FillColor =
                btnMethordofExecutionT.FillColor = btnMethordofExecutionSP.FillColor = Color.PaleTurquoise;
            Guna2Button btnPermission = (Guna2Button)sender;
            btnPermission.FillColor = Color.DarkTurquoise;
            MethodOfExecution = btnPermission.Text;
        }

        private void btnPermission_Click(object sender, EventArgs e)
        {
            btnPermissionC.FillColor = btnPermissionCE.FillColor = btnPermissionCom.FillColor = Color.PaleTurquoise;
            Guna2Button btnPermission = (Guna2Button)sender;
            btnPermission.FillColor = Color.DarkTurquoise;
            Permission = btnPermission.Text;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            AddDataToTheDatabase(false);
        }

        private void btnAddProgress_Click(object sender, EventArgs e)
        {
            try
            {
                AddDataToTheDatabase(true);
            }
            finally
            {
                if (txtProjectID.Text != null && txtName.SelectedIndex != -1 && txtEstimator.SelectedIndex != -1 && txtEstimatedMoney.Text != null
                    && txtEHSub2.Text != null && txtEHSub1.Text != null && txtEHMain.Text != null && txtDescription.Text != null
                    && Permission != null && MethodOfExecution != null)
                {
                    Progress form = new Progress();
                    form.txtProjectID.Text = txtProjectID.Text;
                    form.Show();
                    this.Close();
                }                
            }
        }

        void AddDataToTheDatabase(bool AddProgress)
        {
            if (txtProjectID.Text != "" && txtName.SelectedIndex != -1 && txtEstimator.SelectedIndex != -1 && txtEstimatedMoney.Text != ""
                && txtEHSub2.Text != "" && txtEHSub1.Text != "" && txtEHMain.Text != "" && txtDescription.Text != ""
                && Permission != "" && MethodOfExecution != "")
            {
                string EstimatorNo = SQLRead("select EstimatorNO from Estimator WHERE EstName = N'" + txtEstimator.Text + "' ;", "int", "EstimatorNO");

                string EHID = SQLRead("SELECT EHID FROM dbo.ExpenditureHead WHERE (EH_Main='" + txtEHMain.Text
                                + "') AND (EH_Sub1='" + txtEHSub1.Text + "') AND (EH_Sub2='" + txtEHSub2.Text + "') "
                                + "AND ((EH_Sub3='" + txtEHSub3.Text + "') OR (EH_Sub3 IS NULL))", "string", "EHID");

                string PersonName = SQLRead("select PID from Person WHERE Name = N'" + txtName.Text + "' ;", "int", "PID");

                List<string> QueryList = new List<string>
                {
                    "INSERT INTO[dbo].[Project]([ProjectID], [Discription], [EstimatedMoney], [Permission], " +
                    "[StartDate], [MethodOfExecution], [EstimatorNO]) " +
                    "VALUES(" + txtProjectID.Text + ", N'" + txtDescription.Text.Trim() + "', " + txtEstimatedMoney.Text + ", N'"
                    + Permission + "', N'" + txtDate.Text + "', N'" + MethodOfExecution + "', "
                    + EstimatorNo + ")",

                    "INSERT INTO [dbo].[ExpenditureHeadProject] ([EHID], [ProjectID]) VALUES (N'" + EHID + "', " + txtProjectID.Text +")",

                    "INSERT INTO [dbo].[PersonProject] ([ProjectID], [PID]) VALUES (" + txtProjectID.Text  +", N'" + PersonName + "')",

                    "INSERT INTO [dbo].[ProgressOfProject] ([ProjectID], [Progress], [Date], [Details]) VALUES ('" + txtProjectID.Text + "','0','" + txtDate.Text + "','Not Started')"
                };

                for (var i = 0; i < QueryList.Count; i++) 
                {
                    if (ExecuteSQL(QueryList[i]))
                    {
                        pictureBoxCorrect.Visible = true;
                        lblSuccess.Text = txtProjectID.Text + " Added Successfully !";
                    }
                    else
                    {
                        if (!AddProgress)
                        {
                            MessageBox.Show("Incorrect Input Data !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    }
                }                
            }
            else
            {
                MessageBox.Show("Please Enter All Required Fields !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool ExecuteSQL(string query)
        {
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = '"
                + Properties.Settings.Default.DatabaseLocation + "'; Integrated Security = True");
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader SQL = cmd.ExecuteReader();

                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error found!" + e.Message, "Somthing Went Wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }            
        }

        public string SQLRead(string query, string ReturnValueType, string FieldName)
        {
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = '"
                + Properties.Settings.Default.DatabaseLocation + "'; Integrated Security = True");

                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        if (ReturnValueType == "int")
                        {
                            return reader.GetInt32(reader.GetOrdinal(FieldName)).ToString();
                        }
                        else
                        {
                            try
                            {
                                return reader.GetString(reader.GetOrdinal(FieldName)).ToString();
                            }
                            catch
                            {
                                string[] FieldNameList = FieldName.Split( );
                                string returnString = "", Value;
                                foreach (var Name in FieldNameList)
                                {
                                    try
                                    {
                                        Value = reader.GetString(reader.GetOrdinal(Name)).ToString();
                                    }
                                    catch
                                    {
                                        Value = "";
                                    }
                                    returnString += Value + " ";
                                }
                                return returnString;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error found!" + e.Message, "Somthing Went Wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }            
        }

        private void AddNewRecord(object sender, EventArgs e)
        {
            Guna2TextBox textBox = (Guna2TextBox)sender;
            if (textBox.Name == txtEstimatedMoney.Name || textBox.Name == txtProjectID.Name)
            {
                char[] originalText = textBox.Text.ToCharArray();
                foreach (char c in originalText)
                {
                    if (textBox.Name == txtEstimatedMoney.Name && c == '.')
                        continue;
                    else if (!(Char.IsNumber(c)))
                    {
                        textBox.Text = textBox.Text.Remove(textBox.Text.IndexOf(c));
                    }
                }
                textBox.Select(textBox.Text.Length, 0);
            }
        }

        void Start()
        {
            lblSuccess.Text = null;
            pictureBoxCorrect.Visible = false;
            txtDate.ResetText();

            txtProjectID.Text = (Convert.ToInt32(sqlFunctions.SQLRead("SELECT MAX(ProjectID) AS LastProjectID FROM dbo.Project", "LastProjectID")[0]) + 1).ToString();
            if (txtProjectID.Text[0].ToString() + txtProjectID.Text[1].ToString() != txtDate.Value.Year.ToString()[2].ToString() + txtDate.Value.Year.ToString()[3].ToString()) 
            {
                txtProjectID.Text = txtDate.Value.Year.ToString()[2].ToString() + txtDate.Value.Year.ToString()[3].ToString() + "00001";
            }

            txtName.Items.Clear();
            List<string> PersonNameList = sqlFunctions.SQLRead("SELECT Name FROM dbo.Person WHERE Active='Yes'", "Name");
            foreach (var Item in PersonNameList)
            {
                txtName.Items.Add(Item);
            }
            txtName.Items.Add("Other");

            txtEstimator.Items.Clear();
            List<string> EstimatorNameList = sqlFunctions.SQLRead("SELECT EstName FROM dbo.Estimator WHERE Active='Yes'", "EstName");
            foreach (var Item in EstimatorNameList)
            {
                txtEstimator.Items.Add(Item);
            }
            txtEstimator.Items.Add("Other");
        }
                
        void ClearData()
        {
            txtProjectID.Clear();
            txtDescription.Clear();
            txtEHMain.Clear();
            txtEHSub1.Clear();
            txtEHSub2.Clear();
            txtEHSub3.Clear();
            txtEstimatedMoney.Clear();
            txtName.SelectedIndex = 0;
            txtEstimator.ResetText();
            btnPermissionC.FillColor = btnPermissionCE.FillColor = btnPermissionCom.FillColor = Color.PaleTurquoise;
            btnMethordofExecutionS.FillColor = btnMethordofExecutionD.FillColor =
                btnMethordofExecutionT.FillColor = btnMethordofExecutionSP.FillColor = Color.PaleTurquoise;
            Permission = MethodOfExecution = null;
            Start();
        }

        private void btnEHSelect_Click(object sender, EventArgs e)
        {
            SelectExpenditureHead form = new SelectExpenditureHead();
            form.ShowDialog();
            try
            {
                string EH = SQLRead("SELECT EH_Main,EH_Sub1,EH_Sub2,EH_Sub3 FROM dbo.ExpenditureHead WHERE (EHID='"
                + Properties.Settings.Default.SelectedEHID + "')", "string", "EH_Main EH_Sub1 EH_Sub2 EH_Sub3");
                if (EH == null)
                    EH = "";
                string[] EHParts = EH.Split();
                txtEHMain.Text = EHParts[0];
                txtEHSub1.Text = EHParts[1];
                txtEHSub2.Text = EHParts[2];
                txtEHSub3.Text = EHParts[3];
            }
            catch
            {
                txtEHMain.Text = txtEHSub1.Text = txtEHSub2.Text = txtEHSub3.Text = "";
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            DashBoard form = new DashBoard();
            form.Show();
            this.Close();
        }

        private void txtSelectedIndexChanged(object sender, EventArgs e)
        {
            Guna2ComboBox SelectedXomboBox = (Guna2ComboBox)sender;
            if (SelectedXomboBox.Text == "Other")
            {                
                Settings form = new Settings();
                form.btnEditData.Checked = true;
                if (SelectedXomboBox.Name == "txtName")
                    form.btnEditDataPerson.Checked = true;
                else
                    form.btnEditDataEst.Checked = true;
                form.BeforeLoad();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ShowDialog();
                Start();
                MessageBox.Show("Index No, Proposer, StartDate and Estimater fileds reseted becouse apply updates.",
                    "Settings Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter form = new Filter();
            form.Show();
            this.Close();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings form = new Settings();
            form.BeforeLoad();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }

        private void btnEditData_Click(object sender, EventArgs e)
        {
            AddNewTableMode form = new AddNewTableMode();
            form.ShowDialog();

            if (form.NavigateToProgress)
            {
                Progress SubForm = new Progress();
                SubForm.txtProjectID.Text = form.SelectedProjectID;
                SubForm.Show();
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            sqlFunctions.SignOut(this);
        }

        private void btnProgress_Click(object sender, EventArgs e)
        {
            Progress form = new Progress();
            form.Show();
            this.Close();
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
            this.Close();
        }

        private void btnTableMode_Click(object sender, EventArgs e)
        {
            AddNewTableMode form = new AddNewTableMode();
            form.ShowDialog();

            if (form.NavigateToProgress)
            {
                Progress SubForm = new Progress();
                SubForm.txtProjectID.Text = form.SelectedProjectID;
                SubForm.Show();
                this.Close();
            }
        }

        private void btnProgressHistory_Click(object sender, EventArgs e)
        {
            ProgressHistory form = new ProgressHistory();
            form.ShowDialog();
        }
    }
}
