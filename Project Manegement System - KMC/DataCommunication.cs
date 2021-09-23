using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace Project_Manegement_System___KMC
{
    public partial class DataCommunication : Form
    {
        public DataCommunication()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        Progress ProgressForm = new Progress();
        AddNew sqlExecutFunctions = new AddNew();
        List<string> NewProgressDetails = new List<string> { };

        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard form = new DashBoard();
            form.Show();
            this.Hide();
        }

        private void btnAddData(object sender, EventArgs e)
        {
            AddNew form = new AddNew();
            form.Show();
            this.Hide();
        }

        private void btnProgress_Click(object sender, EventArgs e)
        {
            Progress form = new Progress();
            form.Show();
            this.Hide();
        }

        private void btnFilters(object sender, EventArgs e)
        {
            Filter form = new Filter();
            form.Show();
            this.Hide();
        }

        private void btnEditData_Click(object sender, EventArgs e)
        {
            EditData form = new EditData();
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

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
            this.Close();
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {            
            if (Properties.Settings.Default.LoginType == "User")
            {
                ImportXMLToGrid(false);
                DataSet UserSettingsDataSet = new DataSet();
                UserSettingsDataSet.ReadXml("Settings.Xml");
                UserSettingsDataSet.Tables["UserSettings"].Clear();
                UserSettingsDataSet.Tables["UserSettings"].Rows.Add(DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year);
                UserSettingsDataSet.Tables["UserSettings"].WriteXml("Settings.Xml");
                lblLastUpdateMsg.Text = "Last Updated: " + UserSettingsDataSet.Tables["UserSettings"].Rows[UserSettingsDataSet.Tables["UserSettings"].Rows.Count - 1].ItemArray[0].ToString().Trim();
                lblLastUpdateMsg.ForeColor = Color.Teal;
                ImgLastUpdateShortMsg.Image = Properties.Resources.Data;
            }
            else
            {
                if (ImportXMLToGrid(true)) 
                {
                    MainPagedControl.SelectedPage = page2;
                    btnBack.Tag = MainPagedControl.SelectedPage.Name;

                    List<string> RowsToDelete = new List<string> { };

                    for (int i = 0; i < TableResults.Rows.Count - 1; i++)
                    {
                        if (sqlFunctions.SQLRead("SELECT Discription FROM Project WHERE ProjectID='" + TableResults.Rows[i].Cells[0].Value.ToString() + "'", "Discription") != null)
                        {
                            RowsToDelete.Add(TableResults.Rows[i].Cells[0].Value.ToString());
                        }
                    }
                    DeleteRowsOfDataGridView(TableResults);

                    for (int i = 0; i < TableDetailsOfUpdatedProgress.Rows.Count - 1; i++)
                    {
                        if (sqlFunctions.SQLRead("SELECT Details FROM ProgressOfProject WHERE " +
                            "ProjectID='" + TableDetailsOfUpdatedProgress.Rows[i].Cells[0].Value.ToString() + "' AND " +
                            "Progress='" + TableDetailsOfUpdatedProgress.Rows[i].Cells[1].Value.ToString() + "' AND " +
                            "Date='" + TableDetailsOfUpdatedProgress.Rows[i].Cells[2].Value.ToString() + "'", "Details") != null)
                        {
                            RowsToDelete.Add(TableDetailsOfUpdatedProgress.Rows[i].Cells[0].Value.ToString());
                        }
                    }
                    DeleteRowsOfDataGridView(TableDetailsOfUpdatedProgress);

                    void DeleteRowsOfDataGridView(Guna2DataGridView gridView)
                    {
                        for (int i = 0; i < RowsToDelete.Count; i++)
                        {
                            for (int j = 0; j < gridView.Rows.Count - 1; j++)
                            {
                                if (RowsToDelete[i] == gridView.Rows[j].Cells[0].Value.ToString())
                                {
                                    gridView.Rows.RemoveAt(j);
                                }
                            }
                        }
                        RowsToDelete.Clear();
                    }

                    DataSet DataSetOFImportedFiles = new DataSet();
                    DataSetOFImportedFiles.ReadXml("Settings.Xml");
                    try
                    {
                        if (DataSetOFImportedFiles.Tables["ImportFileDetails"].Rows.Count >= 100)
                        {
                            for (int i = 0; i < 50; i++)
                            {
                                DataSetOFImportedFiles.Tables["ImportFileDetails"].Rows.RemoveAt(i);
                            }
                        }
                    }
                    catch
                    {
                        DataSetOFImportedFiles.Tables.Add("ImportFileDetails");
                        DataSetOFImportedFiles.Tables["ImportFileDetails"].Columns.Add("EstNo");
                        DataSetOFImportedFiles.Tables["ImportFileDetails"].Columns.Add("Date");
                    }
                    finally
                    {
                        DataSetOFImportedFiles.Tables["ImportFileDetails"].Rows.Add(
                        sqlFunctions.SQLRead("SELECT EstimatorNO FROM Estimator WHERE EstName='" + txtEstimator.Text.Trim() + "' AND ZoneNo='" + txtEstZone.Text.Trim() + "'", "EstimatorNO")[0].Trim(),
                        txtExportedDate.Text);
                        DataSetOFImportedFiles.WriteXml("Settings.Xml");
                    }
                }
            }
        }

        private bool ImportXMLToGrid(bool LoginAsAdmin)
        {
            Guna2DataGridView ImportedDataGrid = new Guna2DataGridView();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                DataSet ImportedDataSet = new DataSet();
                ImportedDataSet.ReadXml(ofd.FileName);

                string[] FileNameParts = ofd.FileName.Split()[0].Replace("\\", "#").Trim().Split('#');

                if (LoginAsAdmin)
                {
                    if (FileNameParts[FileNameParts.Length - 1].Trim() != "Admin") 
                    {
                        TableResults.Columns.Clear();
                        TableDetailsOfUpdatedProgress.Columns.Clear();
                        TableResults.DataSource = ImportedDataSet.Tables["Project"];
                        TableDetailsOfUpdatedProgress.DataSource = ImportedDataSet.Tables["ProgressOfProject"];
                        TableDetailsOfUpdatedProgress.Sort(TableDetailsOfUpdatedProgress.Columns[0], ListSortDirection.Ascending);

                        // updated progress by estmator
                        List<int> RowIndexNumbersToRemove = new List<int> { };
                        string ProjectID = "";
                        int MaxProgress = 0;
                        for (int i = 0; i < TableDetailsOfUpdatedProgress.Rows.Count - 1; i++)
                        {
                            if (ProjectID != TableDetailsOfUpdatedProgress.Rows[i].Cells[0].Value.ToString().Trim())
                            {
                                ProjectID = TableDetailsOfUpdatedProgress.Rows[i].Cells[0].Value.ToString().Trim();
                                MaxProgress = Convert.ToInt32(TableDetailsOfUpdatedProgress.Rows[i].Cells[1].Value.ToString().Trim());
                            }
                            else
                            {
                                if (MaxProgress <= Convert.ToInt32(TableDetailsOfUpdatedProgress.Rows[i].Cells[1].Value.ToString().Trim()))
                                {
                                    RowIndexNumbersToRemove.Add(i - 1);
                                    MaxProgress = Convert.ToInt32(TableDetailsOfUpdatedProgress.Rows[i].Cells[1].Value.ToString().Trim());
                                }
                            }
                        }
                        for (int i = 0; i < RowIndexNumbersToRemove.Count; i++)
                        {
                            TableDetailsOfUpdatedProgress.Rows.RemoveAt(RowIndexNumbersToRemove[i]);
                            for (int j = 0; j < RowIndexNumbersToRemove.Count; j++)
                            {
                                RowIndexNumbersToRemove[j] = RowIndexNumbersToRemove[j] - 1;
                            }
                        }

                        TableDetailsOfUpdatedProgress.Columns[0].HeaderText = TableResults.Columns[0].HeaderText = "Index No";
                        TableResults.Columns[2].HeaderText = "Description";
                        TableResults.Columns[2].HeaderText = "Estimated Money";
                        TableResults.Columns[5].HeaderText = "Start Date";
                        TableResults.Columns[4].HeaderText = "Method Of Execution";
                        TableResults.Columns[6].HeaderText = "Estimator";
                        TableResults.Columns[7].HeaderText = "Expenditure Head";
                        TableResults.Columns[7].HeaderText = "Proposer";

                        txtEstimator.Text = ImportedDataSet.Tables["ExportorDetails"].Rows[0].ItemArray[0].ToString();
                        txtEstZone.Text = ImportedDataSet.Tables["ExportorDetails"].Rows[0].ItemArray[1].ToString();
                        txtExportedDate.Text = ImportedDataSet.Tables["ExportorDetails"].Rows[0].ItemArray[2].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Selected File Is Incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    if (FileNameParts[FileNameParts.Length - 1].Trim() == "Admin")
                    {
                        string Query;
                        foreach (string TableName in new List<string> { "Project", "Estimator", "ExpenditureHead", "ExpenditureHeadProject", "Person", "PersonProject", "ProgressOfProject", "Login" })
                        {
                            for (int i = 0; i < ImportedDataSet.Tables[TableName].Rows.Count; i++)
                            {
                                if (sqlFunctions.SQLRead("SELECT " + ImportedDataSet.Tables[TableName].Columns[1].ColumnName + " FROM " + TableName + " WHERE " + ImportedDataSet.Tables[TableName].Columns[0].ColumnName + "='" + ImportedDataSet.Tables[TableName].Rows[i].ItemArray[0] + "'", ImportedDataSet.Tables[TableName].Columns[1].ColumnName) == null)
                                {
                                    Query = "INSERT INTO [dbo].[" + TableName + "] VALUES (";
                                    for (int j = 0; j < ImportedDataSet.Tables[TableName].Columns.Count; j++)
                                    {
                                        Query += "N'" + ImportedDataSet.Tables[TableName].Rows[i].ItemArray[j] + "', ";
                                    }
                                    Query = Query.Remove(Query.Length - 2) + ")";
                                    sqlExecutFunctions.ExecuteSQL(Query);
                                }
                            }
                        }
                        MessageBox.Show("Database Upto Dated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Selected File Is Incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                return true;
            }
            return false;
        }


        private void btnExportData_Click(object sender, EventArgs e)
        {
            ExportGridToXML();
        }

        private void ExportGridToXML()
        {
            DataSet ExportedDataSet = new DataSet();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML|*.xml";

            void EnterDataToDataSet(List<string> TableNames, DataSet dtaSet)
            {
                Guna2DataGridView ExportedDataGrid = new Guna2DataGridView();

                foreach (string ExportedTableName in TableNames)
                {
                    ExportedDataGrid.Columns.Clear();
                    switch (ExportedTableName)
                    {
                        case "Project":
                            {
                                ExportedDataGrid.Columns.Add("ProjectID", "ProjectID");
                                ExportedDataGrid.Columns.Add("Discription", "Discription");
                                ExportedDataGrid.Columns.Add("EstimatedMoney", "EstimatedMoney");
                                ExportedDataGrid.Columns.Add("Permission", "Permission");
                                ExportedDataGrid.Columns.Add("StartDate", "StartDate");
                                ExportedDataGrid.Columns.Add("MoethodOfExecution", "MoethodOfExecution");
                                ExportedDataGrid.Columns.Add("EstimatorNo", "EstimatorNo");
                                break;
                            }
                        case "Estimator":
                            {
                                ExportedDataGrid.Columns.Add("EstimatorNo", "EstimatorNo");
                                ExportedDataGrid.Columns.Add("EstName", "EstName");
                                ExportedDataGrid.Columns.Add("ZoneNo", "ZoneNo");
                                ExportedDataGrid.Columns.Add("Active", "Active");
                                break;
                            }
                        case "ExpenditureHead":
                            {
                                ExportedDataGrid.Columns.Add("EHID", "EHID");
                                ExportedDataGrid.Columns.Add("EH_Main", "EH_Main");
                                ExportedDataGrid.Columns.Add("EH_Sub1", "EH_Sub1");
                                ExportedDataGrid.Columns.Add("EH_Sub2", "EH_Sub2");
                                ExportedDataGrid.Columns.Add("EH_Sub3", "EH_Sub3");
                                ExportedDataGrid.Columns.Add("ExpenditureHeadDiscription", "ExpenditureHeadDiscription");
                                ExportedDataGrid.Columns.Add("Active", "Active");
                                break;
                            }
                        case "ExpenditureHeadProject":
                            {
                                ExportedDataGrid.Columns.Add("EHID", "EHID");
                                ExportedDataGrid.Columns.Add("ProjectID", "ProjectID");
                                break;
                            }
                        case "Person":
                            {
                                ExportedDataGrid.Columns.Add("PID", "PID");
                                ExportedDataGrid.Columns.Add("Name", "Name");
                                ExportedDataGrid.Columns.Add("Job Title", "Job Title");
                                ExportedDataGrid.Columns.Add("Active", "Active");
                                break;
                            }
                        case "PersonProject":
                            {
                                ExportedDataGrid.Columns.Add("ProjectID", "ProjectID");
                                ExportedDataGrid.Columns.Add("PID", "PID");
                                break;
                            }
                        case "ProgressOfProject":
                            {
                                ExportedDataGrid.Columns.Add("ProjectID", "ProjectID");
                                ExportedDataGrid.Columns.Add("Progress", "Progress");
                                ExportedDataGrid.Columns.Add("Date", "Date");
                                ExportedDataGrid.Columns.Add("Details", "Details");
                                break;
                            }
                        case "Login":
                            {
                                ExportedDataGrid.Columns.Add("Username", "Username");
                                ExportedDataGrid.Columns.Add("Password", "Password");
                                ExportedDataGrid.Columns.Add("Type", "Type");
                                break;
                            }
                    }

                    sqlFunctions.RetrieveDataTable("SELECT * FROM " + ExportedTableName, ExportedDataGrid);
                    DataTable dataTable = ToDataTable(ExportedDataGrid);
                    dataTable.TableName = ExportedTableName;
                    dtaSet.Tables.Add(dataTable);
                }
            }

            if (Properties.Settings.Default.User == "Admin")
            {
                EnterDataToDataSet(new List<string> { "Project", "Estimator", "ExpenditureHead", "ExpenditureHeadProject", "Person", "PersonProject", "ProgressOfProject", "Login" }, ExportedDataSet);
                sfd.FileName = "Admin ExportedData " + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
            }
            else
            {
                EditData EditDataForm = new EditData();
                EditDataForm.RefreshTable(EditDataForm.SelectAllQuery);
                DataTable dataTable = ToDataTable(EditDataForm.TableResults);
                dataTable.TableName = "Project";
                ExportedDataSet.Tables.Add(dataTable);

                EnterDataToDataSet(new List<string> { "ProgressOfProject" }, ExportedDataSet);

                Guna2DataGridView ExportedDataGrid = new Guna2DataGridView();
                ExportedDataGrid.Columns.Clear();
                ExportedDataGrid.Columns.Add("Username", "Username");
                ExportedDataGrid.Columns.Add("Zone", "Zone");
                ExportedDataGrid.Columns.Add("Date", "Date");

                string EstNo = sqlFunctions.SQLRead("SELECT EstimatorNO FROM Login WHERE Username='" + Properties.Settings.Default.User + "'", "EstimatorNO")[0];
                int n = ExportedDataGrid.Rows.Add();

                List<string> ExportorDetails = new List<string> { 
                    sqlFunctions.SQLRead("SELECT EstName FROM Estimator WHERE EstimatorNo='" + EstNo + "'", "EstName")[0],
                    sqlFunctions.SQLRead("SELECT ZoneNo FROM Estimator WHERE EstimatorNO='" + EstNo + "'","ZoneNo")[0],
                    DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() };
                
                for (var i = 0; i < ExportedDataGrid.ColumnCount; i++)
                {
                    ExportedDataGrid.Rows[n].Cells[i].Value = ExportorDetails[i];
                }

                dataTable = ToDataTable(ExportedDataGrid);
                dataTable.TableName = "ExportorDetails";
                ExportedDataSet.Tables.Add(dataTable);

                sfd.FileName = sqlFunctions.SQLRead("SELECT EstName FROM Estimator WHERE EstimatorNo='" + EstNo + "'", "EstName")[0] + " ExportedData " + 
                    DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
            }

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportedDataSet.WriteXml(sfd.FileName);
                    MessageBox.Show(sfd.FileName + " Exported Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private DataTable ToDataTable(Guna2DataGridView dataGridView)
        {
            var dt = new DataTable();
            int columnCount = 0;
            List<int> columnNumbers = new List<int>();

            for (int i = 0; i < dataGridView.ColumnCount; i++) 
            {
                if (dataGridView.Columns[i].Visible)
                {
                    dt.Columns.Add(dataGridView.Columns[i].Name);
                    columnNumbers.Add(columnCount);
                }
                columnCount++;
            }

            var cell = new object[columnNumbers.Count];
            for (int j = 0; j < dataGridView.RowCount - 1; j++)
            {
                int i = 0;
                foreach (int a in columnNumbers)
                {
                    cell[i] = dataGridView.Rows[j].Cells[a].Value;
                    i++;
                }
                dt.Rows.Add(cell);
            }
            return dt;
        }

        private void DataCommunication_Load(object sender, EventArgs e)
        {
            btnBack.Tag = MainPagedControl.SelectedPage.Name;
            if (Properties.Settings.Default.LoginType == "User")
            {
                btnImpaortedFilesDetails.Visible = btnBookNew.Visible = btnDashboard.Enabled = btnFilter.Enabled = btnEditData.Enabled = btnSettings.Enabled = false;
                DataSet UserSettingsDataSet = new DataSet();                
                try
                {
                    UserSettingsDataSet.ReadXml("Settings.Xml");
                    lblLastUpdateMsg.Text += " " + UserSettingsDataSet.Tables["UserSettings"].Rows[UserSettingsDataSet.Tables["UserSettings"].Rows.Count - 1].ItemArray[0].ToString().Trim();
                    if (UserSettingsDataSet.Tables["UserSettings"].Rows[UserSettingsDataSet.Tables["UserSettings"].Rows.Count - 1].ItemArray[0].ToString().Trim() != 
                        DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year) 
                    {
                        lblLastUpdateMsg.ForeColor = Color.LightCoral;
                        ImgLastUpdateShortMsg.Image = Properties.Resources.Warning;
                    }
                }
                catch
                {
                    lblLastUpdateMsg.Text += " " + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
                    UserSettingsDataSet.Tables.Add("UserSettings");
                    UserSettingsDataSet.Tables["UserSettings"].Columns.Add("LastUpdatedDate");
                    UserSettingsDataSet.Tables["UserSettings"].Rows.Add(DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year);
                    UserSettingsDataSet.WriteXml("Settings.Xml");
                }
            }
            else
            {
                ImgLastUpdateShortMsg.Visible = lblLastUpdateMsg.Visible = false;

                foreach (string EstName in sqlFunctions.SQLRead("SELECT EstName FROM Estimator WHERE Active='Yes'", "EstName"))
                {
                    txtBookingDetailsEstimator.Items.Add(EstName.Trim());
                }
            }
        }

        private void AdminPageControl_PageChanged(object sender, Manina.Windows.Forms.PageChangedEventArgs e)
        {
            if (MainPagedControl.SelectedPage == page2)
            {
                btnBack.Tag = AdminPageControl.SelectedPage.Name;
            }
            if (AdminPageControl.SelectedPage == page4)
            {
                if (TableDetailsOfUpdatedProgress.Rows.Count - 1 > 0)
                {
                    NewProgressDetails.Clear();
                    NewProgressDetails.Add(TableDetailsOfUpdatedProgress.Rows[0].Cells[1].Value.ToString());
                    NewProgressDetails.Add(TableDetailsOfUpdatedProgress.Rows[0].Cells[2].Value.ToString());
                    NewProgressDetails.Add(TableDetailsOfUpdatedProgress.Rows[0].Cells[3].Value.ToString());
                    CurrentProgressDetails(TableDetailsOfUpdatedProgress.Rows[0].Cells[0].Value.ToString(), false);
                }
                else
                {
                    txtProgress.Value = 0;
                    txtDetails.Clear();
                    txtLastUpdatedDate.Clear();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (btnBack.Tag.ToString() == page2.Name)
                MainPagedControl.SelectedPage = pageImportExportAdmin;
            else if (btnBack.Tag.ToString() == page3.Name)
            {
                MainPagedControl.SelectedPage = page2;
                btnBack.Tag = MainPagedControl.SelectedPage.Name;
            }
            else if (btnBack.Tag.ToString() == page4.Name)
            {
                AdminPageControl.SelectedPage = page3;
                btnBack.Tag = MainPagedControl.SelectedPage.Name;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int Count = 0;
            if (AdminPageControl.SelectedPage == page3)
            {
                for (int i = 0; i < TableResults.RowCount - 1; i++)
                {
                    if (sqlFunctions.SQLRead("SELECT Discription FROM Project WHERE ProjectID='"+ TableResults.Rows[i].Cells[0].Value.ToString().Trim() + "'", "Discription") == null)
                    {
                        sqlExecutFunctions.ExecuteSQL("INSERT INTO [dbo].[Project]([ProjectID], [Discription], [EstimatedMoney], [Permission], " +
                        "[StartDate], [MethodOfExecution], [EstimatorNO]) " +
                        "VALUES(" + TableResults.Rows[i].Cells[0].Value.ToString().Trim() + ", N'" + TableResults.Rows[i].Cells[1].Value.ToString().Trim() + "', " +
                        TableResults.Rows[i].Cells[2].Value.ToString().Trim() + ", N'" + TableResults.Rows[i].Cells[3].Value.ToString().Trim() + "'," +
                        " N'" + TableResults.Rows[i].Cells[5].Value.ToString().Trim() + "', N'" + TableResults.Rows[i].Cells[4].Value.ToString().Trim() + "', "
                        + sqlFunctions.SQLRead("SELECT EstimatorNO FROM Estimator WHERE EstName='" + TableResults.Rows[i].Cells[6].Value.ToString().Trim() + "'", "EstimatorNO")[0].Trim() + ")");

                        sqlExecutFunctions.ExecuteSQL("INSERT INTO [dbo].[ExpenditureHeadProject] ([EHID], [ProjectID]) " +
                            "VALUES (N'" + TableResults.Rows[i].Cells[7].Value.ToString().Trim() + "', " + TableResults.Rows[i].Cells[0].Value.ToString().Trim() + ")");

                        sqlExecutFunctions.ExecuteSQL("INSERT INTO [dbo].[PersonProject] ([ProjectID], [PID]) " +
                            "VALUES (" + TableResults.Rows[i].Cells[0].Value.ToString().Trim() + ", N'" + TableResults.Rows[i].Cells[8].Value.ToString().Trim() + "')");

                        sqlExecutFunctions.ExecuteSQL("INSERT INTO [dbo].[ProgressOfProject] ([ProjectID], [Progress], [Date], [Details]) " +
                            "VALUES ('" + TableResults.Rows[i].Cells[0].Value.ToString().Trim() + "','0','" + TableResults.Rows[i].Cells[5].Value.ToString().Trim() + "','Not Started')");

                        Count += 1;
                    }
                }
                MessageBox.Show(Count.ToString() + " Project Details Successfully Imported to Database. " 
                    + (TableResults.RowCount - 1 - Count).ToString() + " Project Details Faild to Import Database!",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AdminPageControl.SelectedPage = page4;
            }
            else if (AdminPageControl.SelectedPage == page4)
            {
                if (TableDetailsOfUpdatedProgress.Rows.Count - 1 > 0)
                {
                    for (int i = 0; i < TableDetailsOfUpdatedProgress.RowCount - 1; i++)
                    {
                        if (sqlFunctions.SQLRead("SELECT Discription FROM Project WHERE ProjectID='" + TableDetailsOfUpdatedProgress.Rows[i].Cells[0].Value.ToString().Trim() + "'", "Discription") != null)
                        {
                            sqlExecutFunctions.ExecuteSQL("INSERT INTO [dbo].[ProgressOfProject] ([ProjectID], [Progress], [Date], [Details]) " +
                                "VALUES ('" + TableDetailsOfUpdatedProgress.Rows[i].Cells[0].Value.ToString().Trim() + "'," +
                                "'" + TableDetailsOfUpdatedProgress.Rows[i].Cells[1].Value.ToString().Trim() + "'," +
                                "'" + TableDetailsOfUpdatedProgress.Rows[i].Cells[2].Value.ToString().Trim() + "'," +
                                "'" + TableDetailsOfUpdatedProgress.Rows[i].Cells[3].Value.ToString().Trim() + "')");

                            Count += 1;
                        }
                    }
                    CurrentProgressDetails(txtProjectID.Text, true);
                    MessageBox.Show(Count.ToString() + " Rows Successfully Imported to Database. "
                        + (TableResults.RowCount - 1 - Count).ToString() + " Rows Faild to Import Database!",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }
        }

        void CurrentProgressDetails(string ProjectID, bool Refreash)
        {
            try
            {
                string LastProgress = sqlFunctions.SQLRead("SELECT MAX(Progress) AS LastProgress FROM dbo.ProgressOfProject WHERE ProjectID='" + ProjectID + "'", "LastProgress")[0];
                List<string> ProjectProgressData = sqlFunctions.SQLRead("SELECT * FROM dbo.ProgressOfProject WHERE ProjectID='" + ProjectID + "' AND Progress='" + LastProgress + "'",
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
                    NewProject();
                }
                else
                {
                    if (ProjectID != txtProjectID.Text || Refreash) 
                    {
                        txtProjectID.Text = ProjectID;
                        txtProgress.Value = Convert.ToInt32(ProjectProgressData[ProjectProgressData.Count - 3].Trim());
                        txtLastUpdatedDate.Text = ProjectProgressData[ProjectProgressData.Count - 2].Trim();
                        txtDetails.Text = ProjectProgressData[ProjectProgressData.Count - 1].Trim();
                        
                        //UpdateChartProgressHistory();
                        ProgressForm.txtProjectID.Text = txtProjectID.Text;
                        ProgressForm.txtDate.Text = sqlFunctions.SQLRead("SELECT StartDate FROM Project WHERE ProjectID='" + ProjectID + "'", "StartDate")[0];
                        ProgressForm.UpdateChartProgressHistory();
                        ChartProgressHistory.Series.Clear();
                        ChartProgressHistory.Series.Add(ProgressForm.ChartProgressHistory.Series[0]);

                        if (NewProgressDetails[0] == txtProgress.Value.ToString() &&
                            NewProgressDetails[1] == txtLastUpdatedDate.Text &&
                            NewProgressDetails[2] == txtDetails.Text) 
                        {
                            btnSelectedProgressImport.Checked = true;
                        }
                        else
                        {
                            btnSelectedProgressImport.Checked = false;
                        }
                    }                                       
                }
            }
            catch
            {
                NewProject();
            }

            void NewProject()
            {
                txtProjectID.Text = ProjectID;
                txtProgress.Value = 0;
                txtDetails.Clear();
                if (sqlFunctions.SQLRead("SELECT StartDate FROM Project WHERE ProjectID='" + ProjectID + "'", "StartDate") != null)
                {
                    txtLastUpdatedDate.Text = sqlFunctions.SQLRead("SELECT StartDate FROM Project WHERE ProjectID='" + ProjectID + "'", "StartDate")[0];
                }
                else
                {
                    txtLastUpdatedDate.Clear();
                }
            }            
        }

        private void TableDetailsOfUpdatedProgress_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            NewProgressDetails.Clear();
            NewProgressDetails.Add(TableDetailsOfUpdatedProgress.Rows[e.RowIndex].Cells[1].Value.ToString());
            NewProgressDetails.Add(TableDetailsOfUpdatedProgress.Rows[e.RowIndex].Cells[2].Value.ToString());
            NewProgressDetails.Add(TableDetailsOfUpdatedProgress.Rows[e.RowIndex].Cells[3].Value.ToString());
            CurrentProgressDetails(TableDetailsOfUpdatedProgress.Rows[e.RowIndex].Cells[0].Value.ToString(), false);
        }

        private void btnSelectedProgressImport_CheckedChanged(object sender, EventArgs e)
        {
            if (btnSelectedProgressImport.Checked) 
            {
                btnSelectedProgressImport.Text = "Imported";
            }
            else
            {
                btnSelectedProgressImport.Text = "Import";
            }
        }

        private void btnSelectedProgressImport_Click(object sender, EventArgs e)
        {
            if (btnSelectedProgressImport.Text == "Import")
            {
                if (sqlExecutFunctions.ExecuteSQL("INSERT INTO [dbo].[ProgressOfProject] ([ProjectID], [Progress], [Date], [Details]) " +
                    "VALUES ( '" + txtProjectID.Text + "', '" + NewProgressDetails[0] + "', N'" + NewProgressDetails[1] + "', N'" + NewProgressDetails[2] + "')")) 
                {
                    CurrentProgressDetails(txtProjectID.Text, true);
                }
            }
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            MainPagedControl.SelectedPage = pageImportExportAdmin;
        }

        private void btnImportedFilesDetails_Click(object sender, EventArgs e)
        {
            UpdateTableImportedFilesDetails();
            MainPagedControl.SelectedPage = pageImportedFileDetails;
        }

        private void UpdateTableImportedFilesDetails()
        {
            DataSet DataSetOFImportedFiles = new DataSet();
            DataSetOFImportedFiles.ReadXml("Settings.Xml");
            try
            {
                sqlFunctions.RemoveDuplicateRows(DataSetOFImportedFiles.Tables["ImportFileDetails"]);
            }
            catch
            {
                DataSetOFImportedFiles.Tables.Add("ImportFileDetails");
                DataSetOFImportedFiles.Tables["ImportFileDetails"].Columns.Add("EstNo");
                DataSetOFImportedFiles.Tables["ImportFileDetails"].Columns.Add("Date");
            }
            finally
            {
                int n = 0;
                List<string> EstDetails;
                TableImportedFilesDetails.Rows.Clear();
                for (int i = 0; i < DataSetOFImportedFiles.Tables["ImportFileDetails"].Rows.Count; i++)
                {
                    n = TableImportedFilesDetails.Rows.Add();
                    EstDetails = sqlFunctions.SQLRead("SELECT EstName,ZoneNo FROM Estimator" +
                        " WHERE EstimatorNO='" + DataSetOFImportedFiles.Tables["ImportFileDetails"].Rows[i].ItemArray[0].ToString().Trim() + "'", "EstName ZoneNo");

                    TableImportedFilesDetails.Rows[n].Cells[0].Value = EstDetails[0];
                    TableImportedFilesDetails.Rows[n].Cells[1].Value = EstDetails[1];
                    TableImportedFilesDetails.Rows[n].Cells[2].Value = DataSetOFImportedFiles.Tables["ImportFileDetails"].Rows[i].ItemArray[1].ToString().Trim();

                    btnImportFileDetailsShowAll.Checked = true;
                    btnImportFileDetailsShowTodayOnly.Checked = false;
                }
            }
        }

        private void btnBookNew_Click(object sender, EventArgs e)
        {
            UpdateTableBooking();
            MainPagedControl.SelectedPage = pageBookingDetails;
        }

        private void UpdateTableBooking()
        {
            DataSet DataSetOFImportedFiles = new DataSet();
            DataSetOFImportedFiles.ReadXml("Settings.Xml");
            try
            {
                sqlFunctions.RemoveDuplicateRows(DataSetOFImportedFiles.Tables["BookingDetails"]);
            }
            catch
            {
                DataSetOFImportedFiles.Tables.Add("BookingDetails");
                DataSetOFImportedFiles.Tables["BookingDetails"].Columns.Add("ProjectID");
                DataSetOFImportedFiles.Tables["BookingDetails"].Columns.Add("EstNo");
                DataSetOFImportedFiles.Tables["BookingDetails"].Columns.Add("Date");
            }
            finally
            {
                int n;
                List<string> EstDetails;
                TableBookingDetalis.Rows.Clear();
                for (int i = 0; i < DataSetOFImportedFiles.Tables["BookingDetails"].Rows.Count; i++)
                {
                    n = TableBookingDetalis.Rows.Add();
                    EstDetails = sqlFunctions.SQLRead("SELECT EstName,ZoneNo FROM Estimator" +
                        " WHERE EstimatorNO='" + DataSetOFImportedFiles.Tables["BookingDetails"].Rows[i].ItemArray[1].ToString().Trim() + "'", "EstName ZoneNo");

                    TableBookingDetalis.Rows[n].Cells[0].Value = DataSetOFImportedFiles.Tables["BookingDetails"].Rows[i].ItemArray[0].ToString().Trim();
                    TableBookingDetalis.Rows[n].Cells[1].Value = EstDetails[0];
                    TableBookingDetalis.Rows[n].Cells[2].Value = EstDetails[1];
                    TableBookingDetalis.Rows[n].Cells[3].Value = DataSetOFImportedFiles.Tables["BookingDetails"].Rows[i].ItemArray[2].ToString().Trim();

                    btnImportFileDetailsShowAll.Checked = true;
                    btnImportFileDetailsShowTodayOnly.Checked = false;
                }
            }
        }

        private void btnShowConstrains_Click(object sender, EventArgs e)
        {
            Guna2Button btnShowConstrains = (Guna2Button)sender;

            switch (MainPagedControl.SelectedPage.Tag.ToString())
            {
                case "ImportedFileDetails":
                    {
                        switch (btnShowConstrains.Tag.ToString())
                        {
                            case "Today":
                                {
                                    List<DataGridViewRow> ItemsToDelete = new List<DataGridViewRow> { };
                                    for (int i = 0; i < TableImportedFilesDetails.RowCount - 1; i++)
                                    {
                                        if (TableImportedFilesDetails.Rows[i].Cells[2].Value.ToString().Trim() != DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString()) 
                                        {
                                            ItemsToDelete.Add(TableImportedFilesDetails.Rows[i]);
                                        }
                                    }
                                    foreach (DataGridViewRow dataGridViewRow in ItemsToDelete)
                                    {
                                        TableImportedFilesDetails.Rows.Remove(dataGridViewRow);
                                    }
                                    btnImportFileDetailsShowAll.Checked = false;
                                    break;
                                }
                            case "All":
                                {
                                    UpdateTableImportedFilesDetails();
                                    btnImportFileDetailsShowTodayOnly.Checked = false;
                                    break;
                                }
                        }
                        break;
                    }
                case "BookProjectID":
                    {

                        break;
                    }
            }
        }

        private void btnBookingOperation_Click(object sender, EventArgs e)
        {
            Guna2Button btnBookingOperation = (Guna2Button)sender;
            switch (btnBookingOperation.Tag.ToString())
            {
                case "NewBooking":
                    {
                        PanelBookingDetailsImputs.Enabled = true;
                        txtBookingDetailsProjectID.Clear();
                        break;
                    }
                case "Cancel":
                    {
                        PanelBookingDetailsImputs.Enabled = false;
                        break;
                    }
                case "Add":
                    {
                        DataSet DataSetOFImportedFiles = new DataSet();
                        DataSetOFImportedFiles.ReadXml("Settings.Xml");

                        try
                        {
                            sqlFunctions.RemoveDuplicateRows(DataSetOFImportedFiles.Tables["BookingDetails"]);
                        }
                        catch
                        {
                            DataSetOFImportedFiles.Tables.Add("BookingDetails");
                            DataSetOFImportedFiles.Tables["BookingDetails"].Columns.Add("ProjectID");
                            DataSetOFImportedFiles.Tables["BookingDetails"].Columns.Add("EstNo");
                            DataSetOFImportedFiles.Tables["BookingDetails"].Columns.Add("Date");
                        }
                        finally
                        {
                            if (txtBookingDetailsEstimator.SelectedIndex >= 0)
                            {
                                DataSetOFImportedFiles.Tables["BookingDetails"].Rows.Add(txtBookingDetailsProjectID.Text.Trim(),
                                sqlFunctions.SQLRead("SELECT EstimatorNO FROM Estimator WHERE EstName=N'" + txtBookingDetailsEstimator.SelectedItem.ToString().Trim() + "'", "EstimatorNO")[0].Trim(),
                                DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year);
                            }                            
                        }
                        DataSetOFImportedFiles.WriteXml("Settings.Xml");
                        UpdateTableBooking();
                        PanelBookingDetailsImputs.Enabled = false;

                        break;
                    }
            }
        }
    }
}
