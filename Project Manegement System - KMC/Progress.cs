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
using System.Windows.Forms.DataVisualization.Charting;

namespace Project_Manegement_System___KMC
{
    public partial class Progress : Form
    {
        public Progress()
        {
            InitializeComponent();
        }
        int CurrentProgress = 0;
        SQLFunctions sqlFunctions = new SQLFunctions();
        AddNew sqlExecuteFunctions = new AddNew();
        List<string> MonthEnglish = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        List<string> MonthNomber = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "1", "12" };

        private void btnAddData_Click(object sender, EventArgs e)
        {
            AddNew form = new AddNew();
            form.Show();
            this.Hide();
        }

        private void Search(object sender, EventArgs e)
        {
            SearchDetails();
        }

        void SearchDetails()
        {
            txtOther.Clear();
            txtDate.Clear();
            txtProgressDate.Clear();
            ProgressBarProgress.Value = 0;

            bool AvailabilityOfProjectID = false;
            List<string> ProjectIDList = SQLRead("SELECT ProjectID FROM dbo.Project", "ProjectID");
            foreach (string ProjectID in ProjectIDList)
            {
                if (txtProjectID.Text.Trim() == ProjectID.Trim())
                {
                    AvailabilityOfProjectID = true;
                    break;
                }
            }

            if (txtProjectID.Text != "" && AvailabilityOfProjectID)
            {
                List<string> ProjectData = SQLRead("SELECT * FROM dbo.Project WHERE (ProjectID='" + txtProjectID.Text + "')",
                    "Discription EstimatedMoney Permission StartDate MethodOfExecution EstimatorNO");

                txtDate.Text = ProjectData[3];
                txtDescription.Text = ProjectData[0];
                txtEstimatedMoney.Text = ProjectData[1];
                txtPermission.Text = ProjectData[2];                
                txtMOE.Text = ProjectData[4];

                txtZoneNo.Text = SQLRead("select ZoneNo from Estimator WHERE EstimatorNO = '" + ProjectData[5] + "' ;", "ZoneNo")[0];

                string EHID = SQLRead("SELECT EHID FROM dbo.ExpenditureHeadProject WHERE ProjectID='" + txtProjectID.Text + "'", "EHID")[0];

                List<string> ExpenditureHead = SQLRead("SELECT * FROM dbo.ExpenditureHead WHERE EHID='" + EHID + "'",
                    "EH_Main EH_Sub1 EH_Sub2 EH_Sub3");

                txtEH.Text = (ExpenditureHead[0].Trim() + " - " + ExpenditureHead[1].Trim() + " - " + ExpenditureHead[2].Trim() + " - " + ExpenditureHead[3]).Trim();
                if (txtEH.Text[txtEH.Text.Count() - 1] == '-' || txtEH.Text[txtEH.Text.Count() - 2] == '-')
                {
                    txtEH.Text = txtEH.Text.Remove(txtEH.Text.Length - 2);
                }

                try
                {
                    string LastProgress = SQLRead("SELECT MAX(Progress) AS LastProgress FROM dbo.ProgressOfProject WHERE ProjectID='" + txtProjectID.Text + "'", "LastProgress")[0];
                    List<string> ProjectProgressData = SQLRead("SELECT * FROM dbo.ProgressOfProject WHERE ProjectID='" + txtProjectID.Text + "' AND Progress='" + LastProgress + "'",
                        "Progress Date Details");

                    bool IsNoProgressData = true;
                    if (ProjectProgressData != null)
                    {
                        for (var i = 0; i < ProjectProgressData.Count; i++)
                        {
                            if (ProjectProgressData[i] != "")
                                IsNoProgressData = false;
                        }
                    }

                    if (IsNoProgressData)
                    {
                        CurrentProgress = 0;
                        txtProgressDate.Text = txtDate.Text;
                    }
                    else
                    {
                        CurrentProgress = Convert.ToInt32(ProjectProgressData[ProjectProgressData.Count - 3]);
                        txtProgressDate.Text = ProjectProgressData[ProjectProgressData.Count - 2];
                        txtOther.Text = ProjectProgressData[ProjectProgressData.Count - 1];
                    }
                }
                catch
                {
                    CurrentProgress = 0;
                    txtProgressDate.Text = txtDate.Text;
                }
            }
            else
            {
                if (txtProjectID.Text != "")
                {
                    MessageBox.Show("Index No is Unavailabile, Try again", "Error Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public List<string> SQLRead(string query, string FieldNames)
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
                    List<string> ReturnList = new List<string> { };
                    while (reader.Read())
                    {
                        string Value;
                        string[] FieldNameList = FieldNames.Split();
                        foreach (var Name in FieldNameList)
                        {
                            try
                            {
                                try
                                {
                                    Value = reader.GetString(reader.GetOrdinal(Name)).ToString();
                                }
                                catch
                                {
                                    try
                                    {
                                        Value = reader.GetDouble(reader.GetOrdinal(Name)).ToString();
                                    }
                                    catch
                                    {
                                        Value = reader.GetInt32(reader.GetOrdinal(Name)).ToString();
                                    }
                                }
                            }
                            catch
                            {
                                Value = "";
                            }
                            ReturnList.Add(Value);
                        }
                    }
                    return ReturnList;
                }
                return null;
            }
            catch 
            {
                return null;
            }
        }

        private void AddNewRecord(object sender, EventArgs e)
        {
            char[] originalText = txtProjectID.Text.ToCharArray();
            foreach (char c in originalText)
            {
                if (!(Char.IsNumber(c)))
                {
                    txtProjectID.Text = txtProjectID.Text.Remove(txtProjectID.Text.IndexOf(c));
                }
            }
            txtProjectID.Select(txtProjectID.Text.Length, 0);
        }

        private void BtnUpdateProgress_Click(object sender, EventArgs e)
        {
            UpdateProgress form = new UpdateProgress();
            form.txtProjectID.Text = txtProjectID.Text;
            form.ShowDialog();
            txtProgressDate.Text = form.txtProgressDate.Text;
            txtOther.Text = form.txtOther.Text;
            CurrentProgress = form.TrackBarProgress.Value;
            UpdateChartProgressHistory();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (txtDescription.Text == "")
            {
                btnRemoveProject.Enabled = btnEdit.Enabled = BtnUpdateProgress.Enabled = false;
            }
            else
            {
                btnRemoveProject.Enabled = btnEdit.Enabled = BtnUpdateProgress.Enabled = true;
                UpdateChartProgressHistory();
            }
        }

        public void UpdateChartProgressHistory()
        {
            ChartProgressHistory.Series[0].Points.Clear();

            List<string> ProgressListstring = sqlFunctions.SQLRead("SELECT DISTINCT Progress FROM ProgressOfProject " +
              "WHERE ProjectID='" + txtProjectID.Text + "'", "Progress");

            List<int> ProgressListInt = new List<int> { };
            List<string> UpdatedDateList = new List<string> { };
            
            for (int i = 0; i < ProgressListstring.Count; i++)
            {
                ProgressListInt.Add(Convert.ToInt32(ProgressListstring[i]));
            }

            ProgressListInt.Sort();

            if (sqlFunctions.SQLRead("SELECT MIN(Progress) AS MinProgress FROM ProgressOfProject " +
                "WHERE ProjectID='" + txtProjectID.Text + "'", "MinProgress")[0].Trim() != "0")
            {
                string[] UpdatedDate = txtDate.Text.Split(',');
                ChartProgressHistory.Series[0].Points.AddXY(UpdatedDate[2].Trim() + "," + UpdatedDate[1].Trim(), 0);
            }

            for (int i = 0; i < ProgressListInt.Count; i++)
            {
                List<string> UpdatedDateOfProjectList = sqlFunctions.SQLRead("SELECT Date FROM ProgressOfProject " +
                    "WHERE ProjectID='" + txtProjectID.Text + "' AND Progress='" + ProgressListInt[i].ToString() + "'", "Date");

                for (int j = 0; j < UpdatedDateOfProjectList.Count; j++) 
                {
                    UpdatedDateOfProjectList[j] = UpdatedDateOfProjectList[j].Replace(' ', ',');
                    string[] UpdatedDate = UpdatedDateOfProjectList[j].Split(',');

                    for (int k = 0; k < MonthEnglish.Count; k++) 
                    {
                        UpdatedDate[2] = UpdatedDate[2].Replace(MonthEnglish[k], MonthNomber[k]);
                    }
                    UpdatedDateOfProjectList[j] = UpdatedDate[5].Trim() + "/" + UpdatedDate[2].Trim() + "/" + UpdatedDate[3].Trim();
                }

                if (UpdatedDateOfProjectList.Count == 1)
                {
                    ChartProgressHistory.Series[0].Points.AddXY(UpdatedDateOfProjectList[0], ProgressListInt[i]);
                }
                else if (UpdatedDateOfProjectList.Count > 1)
                {
                    foreach (string item in UpdatedDateOfProjectList)
                    {
                        ChartProgressHistory.Series[0].Points.AddXY(item, ProgressListInt[i]);
                    }
                }
            }
        }

        private void Progress_Load(object sender, EventArgs e)
        {
            btnRemoveProject.Enabled = btnEdit.Enabled = BtnUpdateProgress.Enabled = false;
            CurrentProgress = 0;
            ProgressBarProgress.Value = 0;
            if (txtProjectID.Text != "")
            {
                SearchDetails();
            }
            ChartProgressHistory.Series[0].Points.AddXY(0, 0);
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            ProgressHistory form = new ProgressHistory();
            form.txtProjectID.Text = txtProjectID.Text;
            form.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearDetails();
        }

        private void TimerSetCurrentProgress_Tick(object sender, EventArgs e)
        {
            if (ProgressBarProgress.Value < CurrentProgress)
            {
                ProgressBarProgress.Value += 1;
            }
        }

        private void ProgressBarProgress_ValueChanged(object sender, EventArgs e)
        {
            if (ProgressBarProgress.Value == 0)
                ProgressBarProgress.FillColor = Color.Gainsboro;
            else
                ProgressBarProgress.FillColor = Color.White;
        }

        void ClearDetails()
        {
            txtProjectID.Clear();
            txtZoneNo.Clear();
            txtProgressDate.Clear();
            ProgressBarProgress.Value = CurrentProgress = 0;
            txtPermission.Clear();
            txtOther.Clear();
            txtMOE.Clear();
            txtEstimatedMoney.Clear();
            txtEH.Clear();
            txtDescription.Clear();
            txtDate.Clear();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            DashBoard form = new DashBoard();
            form.Show();
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectProject form = new SelectProject();
            form.ShowDialog();
            if (!(form.AddNew) && form.SelectedProjectID != null)
            {
                txtProjectID.Text = form.SelectedProjectID;
                SearchDetails();
            }
            else if (form.AddNew)
            {
                this.Close();
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter form = new Filter();
            form.Show();
            this.Hide();
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
            EditData form = new EditData();
            form.Show();
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            sqlFunctions.SignOut(this);
        }

        private void btnProgressHistory_Click(object sender, EventArgs e)
        {
            ProgressHistory form = new ProgressHistory();
            form.ShowDialog();
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
            this.Close();
        }

        private void btnRemoveProject_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete " + txtProjectID.Text, "Information",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (MessageBox.Show("You can't restore " + txtProjectID.Text + " again !", "Information",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    sqlExecuteFunctions.ExecuteSQL("DELETE FROM [dbo].[ExpenditureHeadProject] " +
                        "WHERE ([ProjectID] = " + txtProjectID.Text + ")");

                    sqlExecuteFunctions.ExecuteSQL("DELETE FROM [dbo].[ProgressOfProject] " +
                        "WHERE ([ProjectID] = " + txtProjectID.Text + ")");

                    sqlExecuteFunctions.ExecuteSQL("DELETE FROM [dbo].[PersonProject] " +
                        "WHERE ([ProjectID] = " + txtProjectID.Text + ")");

                    sqlExecuteFunctions.ExecuteSQL("DELETE FROM [dbo].[Project] " +
                        "WHERE ([ProjectID] = " + txtProjectID.Text + ")");

                    ClearDetails();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditDataCustomize form = new EditDataCustomize();
            form.ProjectID = txtProjectID.Text;
            string[] EHParts = txtEH.Text.Split('-');
            form.Person = sqlFunctions.SQLRead("SELECT Person.Name FROM Person,PersonProject WHERE PersonProject.PID=Person.PID AND PersonProject.ProjectID='" + txtProjectID.Text + "'", "Name")[0];
            if (EHParts[0] == "Not Assign")
                form.txtEH.Text = "";
            else
            {
                if (EHParts.Length > 3)
                {
                    form.txtEH.Text = EHParts[0].Trim() + " - " + EHParts[1].Trim() + " - " + EHParts[2].Trim() + " - " + EHParts[3].Trim();
                    form.EHSub3 = EHParts[3].Trim();
                }
                else
                {
                    form.txtEH.Text = EHParts[0].Trim() + " - " + EHParts[1].Trim() + " - " + EHParts[2].Trim();
                    form.EHSub3 = "";
                }

                form.EHMain = EHParts[0].Trim();
                form.EHSub1 = EHParts[1].Trim();
                form.EHSub2 = EHParts[2].Trim();
            }
            form.MOE = txtMOE.Text;
            form.Permission = txtPermission.Text;
            form.StartDate = txtDate.Text;
            form.EstimatedMoney = txtEstimatedMoney.Text;
            form.Estimator = sqlFunctions.SQLRead("SELECT Estimator.EstName FROM Estimator,Project WHERE Project.EstimatorNo=Estimator.Estimatorno AND Project.ProjectID='" + txtProjectID.Text + "'", "EstName")[0];
            form.Description = txtDescription.Text.Trim();
            form.ShowDialog();
            SearchDetails();
        }
    }
}
