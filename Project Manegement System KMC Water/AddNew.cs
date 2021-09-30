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

namespace Project_Manegement_System_KMC_Water
{
    public partial class AddNew : Form
    {
        public AddNew()
        {
            InitializeComponent();
        }

        string Permission = null, MethodOfExecution = null;
        SQLFunctions sqlFunctions = new SQLFunctions();
        MultiFunctions multiFunctions = new MultiFunctions();

        private void AddNew_Load(object sender, EventArgs e)
        {
            Start();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(1280, 720);
        }

        private void btnMethordofExecution_Click(object sender, EventArgs e)
        {
            btnMethordofExecutionD.Checked = btnMethordofExecutionT.Checked = btnMethordofExecutionSP.Checked = false;
            Guna2GradientButton btnMOE = (Guna2GradientButton)sender;
            btnMOE.Checked = true;
            MethodOfExecution = btnMOE.Text;
        }

        private void btnPermission_CheckedChanged(object sender, EventArgs e)
        {
            Guna2GradientButton btnPermission = (Guna2GradientButton)sender;
            if (btnPermission.Checked)
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
                if (txtProjectID.Text != null && txtSection.SelectedIndex != -1 && txtEstimator.SelectedIndex != -1 && txtEstimatedMoney.Text != null
                    && txtEHSub2.Text != null && txtEHSub1.Text != null && txtEHMain.Text.Trim() != null && txtDescription.Text != null
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
            if (txtProjectID.Text != "" && txtSection.SelectedIndex != -1 && txtSection.SelectedIndex != -1 && txtEstimatedMoney.Text != ""
                && txtEHSub2.Text != "" && txtEHSub1.Text != "" && txtEHMain.Text != "" && txtDescription.Text != ""
                && Permission != "" && MethodOfExecution != "")
            {
                string EstimatorNo = SQLRead("select EstimatorNO from Estimator WHERE ZoneNo = N'" + txtSection.Text + "' ;", "int", "EstimatorNO");

                string EHID = SQLRead("SELECT EHID FROM dbo.ExpenditureHead WHERE (EH_Main='" + txtEHMain.Text
                                + "') AND (EH_Sub1='" + txtEHSub1.Text + "') AND (EH_Sub2='" + txtEHSub2.Text + "') "
                                + "AND ((EH_Sub3='" + txtEHSub3.Text + "') OR (EH_Sub3 IS NULL))", "string", "EHID");

                List<string> QueryList = new List<string>
                {
                    "INSERT INTO[dbo].[Project]([ProjectID], [Discription], [EstimatedMoney], [Permission], " +
                    "[StartDate], [MethodOfExecution], [EstimatorNO]) " +
                    "VALUES(" + txtProjectID.Text + ", N'" + txtDescription.Text.Trim() + "', " + txtEstimatedMoney.Text + ", N'"
                    + Permission + "', N'" + UpDownDateYear.Value.ToString() + "-" + UpDownDateMonth.Value.ToString() 
                    + "-" + UpDownDateDay.Value.ToString() + "', N'" + MethodOfExecution + "', "
                    + EstimatorNo + ")",

                    "INSERT INTO [dbo].[ExpenditureHeadProject] ([EHID], [ProjectID]) VALUES (N'" + EHID + "', " + txtProjectID.Text +")",
                                        
                    "INSERT INTO [dbo].[ProgressOfProject] ([ProjectID], [Progress], [Date], [Details]) VALUES ('" + txtProjectID.Text + "','0'," +
                    "'" + UpDownDateYear.Value.ToString() + "-" + UpDownDateMonth.Value.ToString() + "-" + UpDownDateDay.Value.ToString() + "','Not Started')"
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
            if (textBox.Name == txtProjectID.Name)
            {
                textBox.Text = multiFunctions.TypeCheckInput(textBox.Text, "N", 0);
                textBox.Select(textBox.Text.Length, 0);
            }
            else if (textBox.Name == txtEstimatedMoney.Name)
            {
                textBox.Text = multiFunctions.TypeCheckInput(textBox.Text, "N.F", 2);
                textBox.Select(textBox.Text.Length, 0);

                if (textBox.Text != "")
                {
                    if (Convert.ToInt32(textBox.Text) >= 100000)
                        btnPermissionC.Checked = true;
                    else if (Convert.ToInt32(textBox.Text) >= 75000)
                        btnPermissionCom.Checked = true;
                    else
                        btnPermissionCE.Checked = true;
                }
            }
        }

        void Start()
        {
            lblSuccess.Text = null;
            pictureBoxCorrect.Visible = false;
            UpDownDateYear.Value = DateTime.Now.Year;
            UpDownDateMonth.Value = DateTime.Now.Month;
            UpDownDateDay.Value = DateTime.Now.Day;

            try
            {
                txtProjectID.Text = (Convert.ToInt32(sqlFunctions.SQLRead("SELECT MAX(ProjectID) AS LastProjectID FROM dbo.Project", "LastProjectID")[0]) + 1).ToString();
                if (txtProjectID.Text[0].ToString() + txtProjectID.Text[1].ToString() != UpDownDateYear.Value.ToString()[2].ToString() + UpDownDateYear.Value.ToString()[3].ToString())
                {
                    txtProjectID.Text = UpDownDateYear.Value.ToString()[2].ToString() + UpDownDateYear.Value.ToString()[3].ToString() + "00001";
                }
            }
            catch
            {
                txtProjectID.Text = UpDownDateYear.Value.ToString()[2].ToString() + UpDownDateYear.Value.ToString()[3].ToString() + "00001";
            }            
            
            txtSection.Items.Clear();
            List<string> ZoneList = sqlFunctions.SQLRead("SELECT DISTINCT ZoneNo FROM dbo.Estimator WHERE Active='Yes'", "ZoneNo");
            foreach (var Item in ZoneList)
            {
                txtSection.Items.Add(Item);
            }
            txtSection.Items.Add("Other");

            txtEstimator.Items.Clear();
            List<string> EstimatorNameList = sqlFunctions.SQLRead("SELECT EstName FROM dbo.Estimator WHERE Active='Yes'", "EstName");
            foreach (var Item in EstimatorNameList)
            {
                if (Item.Trim() != "") 
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
            txtSection.SelectedIndex = 0;
            txtEstimator.ResetText();
            btnPermissionC.FillColor = btnPermissionCE.FillColor = btnPermissionCom.FillColor = Color.PaleTurquoise;
            btnMethordofExecutionD.FillColor = btnMethordofExecutionT.FillColor = btnMethordofExecutionSP.FillColor = Color.PaleTurquoise;
            Permission = MethodOfExecution = null;
            Start();
        }

        private void btnEHSelect_Click(object sender, EventArgs e)
        {
            this.Opacity = 80;
            SelectExpenditureHead form = new SelectExpenditureHead();
            form.ShowDialog();
            this.Opacity = 100;
            if (form.CloseParentForm)
            {
                SettingsForm settings_form = new SettingsForm();
                settings_form.ParentSelectedField = "EH";
                settings_form.Show();
                this.Close();
            }
            else
            {
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
        }

        private void btnDropDownMenu_SelectionChanged(object sender, EventArgs e)
        {
            Guna2ComboBox btnDropDown = (Guna2ComboBox)sender;
            if (btnDropDown.SelectedItem.ToString() == "Other")
            {
                SettingsForm form = new SettingsForm();
                form.ParentSelectedField = "Est";
                form.Show();
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            multiFunctions.NavigateTo(btnClose.Tag.ToString(), this);
        }

        private void FormNavigationButton_Click(object sender, EventArgs e)
        {
            Guna2CircleButton btnNavigation = (Guna2CircleButton)sender;
            multiFunctions.NavigateTo(btnNavigation.Tag.ToString(), this);
        }
    }
}
