using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Project_Manegement_System_KMC_Water
{
    public partial class FilteredProjectDetails : Form
    {
        public FilteredProjectDetails()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        public List<string> ActiveColumnList = new List<string> { };
        public List<string> ActiveColumnHeaderList = new List<string> { };
        public List<string> Filters = new List<string> { };
        public List<string> FilterNames = new List<string> { };
        public List<string> ProgressHistoryProjectIDList = new List<string> { };
        public List<string> ProgressHistoryProgressList = new List<string> { };
        public string SortBy = null, ParentFormName = "Filter", ShowResultsType = "All";

        int SelectedRow = 0, SelectedColumn = -1;
        string SavedFilePath = null;
        List<string> DeletedColumn = new List<string> { };

        List<string> DayNames = new List<string>
        {
            "Monday, ", "Tuesday, ", "Wednesday, ", "Thursday, ", "Friday, ", "Saturday, ", "Sunday, ",
            "සදුදා, ", "අගහරුවාදා, ", "බදාදා, ", "බ්‍රහස්පතින්දා, ", "සිකුරාදා, ", "සෙනසුරාදා, ", "ඉරිදා, "
        };

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FilteredProjectDetails_Load(object sender, EventArgs e)
        {
            TableResults.Visible = false;
            GridViewRowOptionsMenuStrip.Items[1].Enabled = false;
            try
            {
                TableResults.Columns.Clear();
                TableResults.Rows.Clear();
                for (var i = 0; i < ActiveColumnList.Count; i++) 
                {
                    TableResults.Columns.Add(ActiveColumnList[i], ActiveColumnHeaderList[i]);
                }
                                
                Dictionary<string, bool> Availability = new Dictionary<string, bool>
                {
                    { "ProjectID", false }, { "Progress", false }, { "Details", false }, { "ExpenditureHead", false }, { "Name", false },{ "ZoneNo", false },
                    { "Discription", false }, { "EstName", false }, { "StartDate", false }, { "MethodOfExecution", false }, { "Permission", false },{ "EstimatedMoney", false }
                };
                foreach (string ColumnName in ActiveColumnList)
                {
                    Availability[ColumnName] = true;
                }

                string ProjectOREstimatorFilters = "", ProgressOfProjectFilters = "", PersonFilters = "", ExpenditureHeadFilters = "";

                for (var i = 0; i < Filters.Count; i++)
                {
                    string FilterTargetColumnName = sqlFunctions.DatabaseColumnName[FilterNames[i].Split('#')[1]];
                    if (Availability[FilterTargetColumnName])
                    {
                        foreach (string DefaultColumnName in sqlFunctions.ProjectEstimatorQueryTableColumnNames)
                        {
                            if (DefaultColumnName == FilterTargetColumnName)
                            {
                                if (ProjectOREstimatorFilters == "")
                                    ProjectOREstimatorFilters += "AND (" + Filters[i];
                                else
                                    ProjectOREstimatorFilters += "AND " + Filters[i];
                            }
                        }

                        foreach (string DefaultColumnName in sqlFunctions.ProgressOfProjectQueryTableColumnNames)
                        {
                            if (DefaultColumnName == FilterTargetColumnName)
                            {
                                if (ProgressOfProjectFilters == "")
                                    ProgressOfProjectFilters += "AND (" + Filters[i];
                                else
                                    ProgressOfProjectFilters += "AND " + Filters[i];
                            }
                        }

                        foreach (string DefaultColumnName in sqlFunctions.PersonQueryTableColumnNames)
                        {
                            if (DefaultColumnName == FilterTargetColumnName)
                            {
                                if (PersonFilters == "")
                                    PersonFilters += "AND (" + Filters[i];
                                else
                                    PersonFilters += "AND " + Filters[i];
                            }
                        }

                        foreach (string DefaultColumnName in sqlFunctions.ExpenditureHeadQueryTableColumnNames)
                        {
                            if (DefaultColumnName == FilterTargetColumnName)
                            {
                                if (ExpenditureHeadFilters == "")
                                    ExpenditureHeadFilters += "AND (" + Filters[i];
                                else
                                    ExpenditureHeadFilters += "AND " + Filters[i];
                            }
                        }
                    }
                }
                
                if (ProjectOREstimatorFilters != "")
                    ProjectOREstimatorFilters += ")";
                if (ProgressOfProjectFilters != "")
                    ProgressOfProjectFilters += ")"; 
                if (PersonFilters != "")
                    PersonFilters += ")"; 
                if (ExpenditureHeadFilters != "")
                    ExpenditureHeadFilters += ")";

                if (Availability["ProjectID"])
                {
                    ///
                    /// Get data from table Project And estimator
                    ///
                    string QueryProjectAndEstimator =
                        "SELECT Project.ProjectID, Project.Discription, Project.EstimatedMoney, Project.Permission, " +
                        "Project.MethodOfExecution, Project.StartDate, Estimator.EstName, Estimator.ZoneNo " +
                        "FROM Project,Estimator " +
                        "WHERE Project.EstimatorNo = Estimator.EstimatorNo " + ProjectOREstimatorFilters;

                    string BasicColumnOrder = "ProjectID Discription EstimatedMoney Permission MethodOfExecution StartDate EstName ZoneNo";
                    string[] DefaultColumnNames = BasicColumnOrder.Split();
                    BasicColumnOrder = "";
                    foreach (string ColumnName in ActiveColumnList)
                    {
                        foreach (string DefaultColumnName in DefaultColumnNames)
                        {
                            if (ColumnName == DefaultColumnName)
                            {
                                BasicColumnOrder += ColumnName + " ";
                            }
                        }
                    }
                    BasicColumnOrder = BasicColumnOrder.Remove(BasicColumnOrder.Length - 1);
                    AddToTableByColumn(sqlFunctions.SQLRead(QueryProjectAndEstimator, BasicColumnOrder), BasicColumnOrder);
                    
                    if (Availability["ZoneNo"])
                    {
                        for (int i = 0; i < TableResults.Rows.Count - 1; i++)
                        {
                            if (TableResults.Rows[i].Cells["ZoneNo"].Value.ToString().Length == 1)
                            {
                                TableResults.Rows[i].Cells["ZoneNo"].Value = "Zone " + TableResults.Rows[i].Cells["ZoneNo"].Value.ToString();
                            }
                        }
                    }

                    string ProjectID = null;
                    int NoOfRows = 0;
                    ///
                    /// Get Data from ProgressOfProject
                    ///
                    if (Availability["Progress"] || Availability["Details"])
                    {
                        string LastProgess = null, DetailOfLastProgress = null, LastProgressWithOutCondition = null;
                        NoOfRows = TableResults.RowCount - 1;

                        if (ParentFormName == "ProgressHistory")
                        {
                            TableResults.Columns.Add("ProgressUpdatedDate", "Progress Updated Date");
                        }

                        for (var i = 0; i < NoOfRows; i++)
                        {
                            ProjectID = TableResults.Rows[i].Cells["ProjectID"].Value.ToString();

                            LastProgess = sqlFunctions.SQLRead("SELECT MAX(Progress) AS LastProgess " +
                                "From ProgressOfProject " +
                                "WHERE (ProjectID = '" + ProjectID + "') " + ProgressOfProjectFilters, "LastProgess")[0];

                            LastProgressWithOutCondition = sqlFunctions.SQLRead("SELECT MAX(Progress) AS LastProgess " +
                                "From ProgressOfProject " +
                                "WHERE (ProjectID = '" + ProjectID + "')", "LastProgess")[0];


                            if (LastProgess == "")
                            {
                                if (LastProgess != LastProgressWithOutCondition)
                                {
                                    TableResults.Rows.Remove(TableResults.Rows[i]);
                                    i -= 1;
                                    NoOfRows -= 1;
                                    continue;
                                }
                                else
                                {
                                    LastProgess = "0";
                                    DetailOfLastProgress = "NotStarted";
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(LastProgess) < Convert.ToInt32(LastProgressWithOutCondition))
                                {
                                    TableResults.Rows.Remove(TableResults.Rows[i]);
                                    i -= 1;
                                    NoOfRows -= 1;
                                    continue;
                                }
                                else
                                {
                                    DetailOfLastProgress = sqlFunctions.SQLRead(
                                   "SELECT Details " +
                                   "FROM ProgressOfProject " +
                                   "WHERE (Progress = '" + LastProgess + "') AND (ProjectID = '" + ProjectID + "')", "Details")[0];

                                    if (ParentFormName == "ProgressHistory")
                                    {
                                        TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value =
                                            sqlFunctions.SQLRead(
                                                "SELECT Date " +
                                                "FROM ProgressOfProject " +
                                                "WHERE (Progress = '" + LastProgess + "') AND (ProjectID = '" + ProjectID + "')", "Date")[0];
                                    }
                                }
                            }

                            if (Availability["Progress"])
                            {
                                TableResults.Rows[i].Cells["Progress"].Value = LastProgess;
                            }
                            if (Availability["Details"])
                            {
                                TableResults.Rows[i].Cells["Details"].Value = DetailOfLastProgress;
                            }/*
                            if (ParentFormName == "ProgressHistoryShowAll")
                            {
                                TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value =
                                    sqlFunctions.SQLRead("SELECT Date FROM ProgressOfProject " +
                                        "WHERE (Progress = '" + LastProgess + "') AND (ProjectID = '" + ProjectID + "')", "Date")[0];
                            }*/
                        }
                    }

                    if (ParentFormName == "ProgressHistoryShowAll" && ProgressHistoryProjectIDList.Count > 0)
                    {
                        TableResults.Rows.Clear();
                        for (int i = 0; i < ProgressHistoryProjectIDList.Count; i++) 
                        {
                            int n = TableResults.Rows.Add();
                            TableResults.Rows[n].Cells["ProjectID"].Value = ProgressHistoryProjectIDList[i];

                            List<string> ProjectDetails = sqlFunctions.SQLRead("" +
                                "SELECT Project.Discription, Project.EstimatedMoney, Project.Permission, " +
                                "Project.MethodOfExecution, Project.StartDate, Estimator.EstName, Estimator.ZoneNo " +
                                "FROM Project,Estimator " +
                                "WHERE Project.EstimatorNo = Estimator.EstimatorNo AND Project.ProjectID='" + ProgressHistoryProjectIDList[i] + "'",
                                "Discription EstimatedMoney Permission MethodOfExecution StartDate EstName ZoneNo");

                            if (Availability["Discription"])
                                TableResults.Rows[n].Cells["Discription"].Value = ProjectDetails[0];
                            if (Availability["EstimatedMoney"])
                                TableResults.Rows[n].Cells["EstimatedMoney"].Value = ProjectDetails[1];
                            if (Availability["Permission"])
                                TableResults.Rows[n].Cells["Permission"].Value = ProjectDetails[2];
                            if (Availability["MethodOfExecution"])
                                TableResults.Rows[n].Cells["MethodOfExecution"].Value = ProjectDetails[3];
                            if (Availability["StartDate"])
                                TableResults.Rows[n].Cells["StartDate"].Value = ProjectDetails[4];
                            if (Availability["EstName"])
                                TableResults.Rows[n].Cells["EstName"].Value = ProjectDetails[5];
                            if (Availability["ZoneNo"])
                                TableResults.Rows[n].Cells["ZoneNo"].Value = ProjectDetails[6];

                            TableResults.Rows[n].Cells["Progress"].Value = ProgressHistoryProgressList[i];

                            List<string> DetailsOfProgress = sqlFunctions.SQLRead("SELECT Details,Date FROM ProgressOfProject " +
                                        "WHERE (Progress = '" + ProgressHistoryProgressList[i] + "') AND " +
                                        "(ProjectID = '" + ProgressHistoryProjectIDList[i] + "')", "Details Date");

                            TableResults.Rows[n].Cells["Details"].Value = DetailsOfProgress[0];
                            TableResults.Rows[n].Cells["ProgressUpdatedDate"].Value = DetailsOfProgress[1];
                        }
                    }

                    ///
                    /// Get Data From ExpenditureHead
                    ///
                    if (Availability["ExpenditureHead"])
                    {
                        List<string> ExpenditureHead = null;

                        NoOfRows = TableResults.RowCount - 1;
                        for (var i = 0; i < NoOfRows; i++)
                        {
                            ProjectID = TableResults.Rows[i].Cells["ProjectID"].Value.ToString();

                            ExpenditureHead = sqlFunctions.SQLRead(
                                    "SELECT ExpenditureHead.EH_Main, ExpenditureHead.EH_Sub1, ExpenditureHead.EH_Sub2, " +
                                    "ExpenditureHead.EH_Sub3 " +
                                    "FROM ExpenditureHead,ExpenditureHeadProject " +
                                    "WHERE ExpenditureHead.EHID=ExpenditureHeadProject.EHID AND " +
                                    "ExpenditureHeadProject.ProjectID='" + ProjectID + "' " + ExpenditureHeadFilters,
                                    "EH_Main EH_Sub1 EH_Sub2 EH_Sub3");

                            if (ExpenditureHead == null)
                            {
                                TableResults.Rows.Remove(TableResults.Rows[i]);
                                i -= 1;
                                NoOfRows -= 1;
                                continue;
                            }

                            TableResults.Rows[i].Cells["ExpenditureHead"].Value =
                                     (ExpenditureHead[0] + "-" + ExpenditureHead[1] + "-" + ExpenditureHead[2] + ExpenditureHead[3]).Trim();
                        }
                    }

                    ///
                    /// Get Data From Person
                    ///
                    if (Availability["Name"])
                    {
                        List<string> PerosnName = null;

                        NoOfRows = TableResults.RowCount - 1;
                        for (var i = 0; i < NoOfRows; i++)
                        {
                            ProjectID = TableResults.Rows[i].Cells["ProjectID"].Value.ToString();

                            PerosnName = sqlFunctions.SQLRead("SELECT Person.Name FROM Person,PersonProject " +
                                    "WHERE Person.PID=PersonProject.PID " +
                                    "AND PersonProject.ProjectID='" + ProjectID + "' " + PersonFilters, "Name");

                            if (PerosnName == null)
                            {
                                TableResults.Rows.Remove(TableResults.Rows[i]);
                                i -= 1;
                                NoOfRows -= 1;
                                continue;
                            }
                            TableResults.Rows[i].Cells["Name"].Value = PerosnName[0];
                        }
                    }

                    /// 
                    /// Sold By
                    ///
                    if (SortBy != null)
                    {
                        for (var i = 0; i < ActiveColumnHeaderList.Count; i++)
                        {
                            if (ActiveColumnHeaderList[i] == SortBy)
                            {
                                if (Availability[ActiveColumnList[i]])
                                {
                                    TableResults.Sort(TableResults.Columns[ActiveColumnList[i]], ListSortDirection.Ascending);
                                }
                            }
                        }
                    }
                }

                TranslateMenuStrip.Items[3].Enabled = false;
                if (Availability["StartDate"] || ParentFormName == "ProgressHistory" || ParentFormName == "ProgressHistoryShowAll") 
                {
                    TranslateMenuStrip.Items[3].Enabled = true;
                }

                if (TableResults.Columns.Count > 0 && TableResults.Rows.Count - 1 > 0) 
                {
                    TableResults.Visible = true;
                }

                if (ParentFormName == "ProgressHistoryShowAll")
                {
                    if (ShowResultsType == "Export") 
                    {
                        ExportData();
                    }
                    else if (ShowResultsType == "Open")
                    {
                        Open();
                    }
                }
            }
            catch (Exception error) /// Something went Wrong
            {
                TableResults.Visible = false;
                lblShowNoOFResults.Text = "An Error Found. " + error;
                lblShowNoOFResults.ForeColor = Color.Red;
            }

            void AddToTableByColumn(List<string> RetrivedDataList, string Order)
            {
                if (RetrivedDataList != null)
                {
                    string[] ColumnOrder = Order.Split();
                    int n, DataIndex = 0;
                    for (var j = 0; j < RetrivedDataList.Count / ColumnOrder.Length; j++)
                    {
                        n = TableResults.Rows.Add();
                        for (var i = 0; i < ColumnOrder.Length; i++)
                        {
                            try
                            {
                                TableResults.Rows[n].Cells[ColumnOrder[i]].Value = RetrivedDataList[DataIndex];
                            }
                            catch
                            {
                                continue;
                            }
                            DataIndex += 1;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Can't find data from database according to the your filters", "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                    TableResults.Visible = false;
                    lblShowNoOFResults.Text = "An Error Found. Can't find data from database according to the your filters";
                    lblShowNoOFResults.ForeColor = Color.Red;
                }
            }
            CountRowsAndColumns();
        }

        private void TableResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRow = e.RowIndex;
        }

        void CountRowsAndColumns()
        {
            if (lblShowNoOFResults.ForeColor!= Color.Red)
                lblShowNoOFResults.Text = "Rows - " + (TableResults.RowCount - 1).ToString() + " , Columns - " + TableResults.ColumnCount.ToString() + " ";
        }

        private void btnFilters_Click(object sender, EventArgs e)
        {
            FiltersContextMenuStrip.Items.Clear();
            foreach (var item in FilterNames)
            {
                FiltersContextMenuStrip.Items.Add(item.Replace('#', ' '));
            }
            FiltersContextMenuStrip.Visible = true;
            FiltersContextMenuStrip.Left = MousePosition.X;
            FiltersContextMenuStrip.Top = MousePosition.Y;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Open();   
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportData();    
        }

        void Open()
        {
            try
            {
                if (SavedFilePath == null)
                {
                    ExportData();
                }
                Process.Start(SavedFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't Open Saved File" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SavedFilePath = null;
        }

        void ExportData()
        {
            if (TableResults.Rows.Count > 0)
            {
                for (var i = 0; i < TableResults.RowCount - 1; i++)
                {
                    for (int j = 0; j < TableResults.Columns.Count; j++)
                    {
                        TableResults.Rows[i].Cells[j].Value = TableResults.Rows[i].Cells[j].Value.ToString().Replace(',', ' ');
                    }
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Result " + DateTime.Now.Date.ToString("yyyy-MM-dd") + " .csv";
                if (SortBy != null)
                    sfd.FileName = "Result " + DateTime.Now.Date.ToString("yyyy-MM-dd") + " by " + TableResults.SortedColumn.HeaderText + " .csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = TableResults.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[TableResults.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += TableResults.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < TableResults.Rows.Count - 1; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += TableResults.Rows[i - 1].Cells[j].Value.ToString().Trim() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SavedFilePath = sfd.FileName;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export!, Table Is Empty", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            TranslateMenuStrip.Show();
            TranslateMenuStrip.Top = MousePosition.Y;
            TranslateMenuStrip.Left = MousePosition.X;
        }

        private void TranslateMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "Header, Method of Execution & Permission":
                    {
                        TranslateHeaderNames();
                        TranslateMOEAndPermission();
                        break;
                    }
                case "Header Only":
                    {
                        TranslateHeaderNames();
                        break;
                    }
                case "Method of Execution & Permission Only":
                    {
                        TranslateMOEAndPermission();
                        break;
                    }
                case "Start Date & Progress Updated Date":
                    {
                        TranslateDate();
                        break;
                    }
                case "Section Only":
                    {
                        TranslateSection();
                        break;
                    }
            }

            void TranslateSection()
            {
                string CurrentValue = TableResults.Rows[0].Cells["ZoneNo"].Value.ToString();
                for (int i = 0; i < TableResults.Rows.Count - 1; i++)
                {
                    CurrentValue = TableResults.Rows[i].Cells["ZoneNo"].Value.ToString();
                    TableResults.Rows[i].Cells["ZoneNo"].Value = TableResults.Rows[i].Cells["ZoneNo"].Value.ToString().Replace("Zone","කලාප");
                    TableResults.Rows[i].Cells["ZoneNo"].Value = TableResults.Rows[i].Cells["ZoneNo"].Value.ToString().Replace("Buildings", "ගොඩනැගිලි අංශය");
                    if (CurrentValue== TableResults.Rows[i].Cells["ZoneNo"].Value.ToString())
                    {
                        TableResults.Rows[i].Cells["ZoneNo"].Value = TableResults.Rows[i].Cells["ZoneNo"].Value.ToString().Replace("කලාප", "Zone");
                        TableResults.Rows[i].Cells["ZoneNo"].Value = TableResults.Rows[i].Cells["ZoneNo"].Value.ToString().Replace("ගොඩනැගිලි අංශය", "Buildings");
                    }
                }
            }

            void TranslateDate()
            {
                List<string> DateEnglish = new List<string> { "Monday, ", "Tuesday, ", "Wednesday, ", "Thursday, ", "Friday, ", "Saturday, ", "Sunday, ", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                List<string> DateSinhala = new List<string> { "සදුදා, ", "අගහරුවාදා, ", "බදාදා, ", "බ්‍රහස්පතින්දා, ", "සිකුරාදා, ", "සෙනසුරාදා, ", "ඉරිදා, ", "ජනවාරි", "පෙබරවාරි", "මාර්තු", "අ‍ප්‍රේල්", "මැයි", "ජූනි", "ජූලි", "අගෝස්තු", "සැප්තැම්බර්", "ඔක්තෝබර්", "නොවැම්බර්", "දෙසැම්බර්" };

                for (int i = 0; i < TableResults.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < DateEnglish.Count; j++) 
                    {
                        string CurrentValue = null;
                        try
                        {
                            CurrentValue = TableResults.Rows[i].Cells["StartDate"].Value.ToString();
                            TableResults.Rows[i].Cells["StartDate"].Value = TableResults.Rows[i].Cells["StartDate"].Value.ToString().Replace(DateEnglish[j], DateSinhala[j]);
                            if (CurrentValue == TableResults.Rows[i].Cells["StartDate"].Value.ToString())
                                TableResults.Rows[i].Cells["StartDate"].Value = TableResults.Rows[i].Cells["StartDate"].Value.ToString().Replace(DateSinhala[j], DateEnglish[j]);
                        }
                        catch { }
                        finally
                        {
                            if (ParentFormName == "ProgressHistory" || ParentFormName == "ProgressHistoryShowAll")
                            {
                                CurrentValue = TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value.ToString();
                                TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value = TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value.ToString().Replace(DateEnglish[j], DateSinhala[j]);
                                if (CurrentValue == TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value.ToString())
                                    TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value = TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value.ToString().Replace(DateSinhala[j], DateEnglish[j]);
                            }
                        }                        
                    }
                }
            }

            void TranslateHeaderNames()
            {
                List<string> HeaderEnglish = new List<string> { "Index No", "Project Description", "Method Of Execution", "Permission", "Start Date", "Estimator", "Section", "Estimated Money", "Expenditure Head", "Proposer", "Progress", "Progress Details", "Progress Updated Date" };
                List<string> HeaderSinhala = new List<string> { "අනු අංකය", "වැඩ විස්තරය", "ක්‍රියාත්මක කිරීමේ ක්‍රමය", "අනුමැතිය", "ආරම්භක දිනය", "ඇස්තමේනතුකරු", "අංශය", "ඇස්තමේනතු මුදල", "වැය ශීර්ෂය", "යොජනා කල පුද්ගලයා", "ප්‍රගතිය", "ප්‍රගති විස්තරය", "ප්‍රගතිය යාවත්කාලීන කල දිනය" };

                for (int i = 0; i < TableResults.Columns.Count; i++)
                {
                    bool ValueInSinhala = true;

                    for (int j = 0; j < HeaderEnglish.Count; j++)
                    {
                        if (TableResults.Columns[i].HeaderText == HeaderEnglish[j])
                        {
                            TableResults.Columns[i].HeaderText = HeaderSinhala[j];
                            ValueInSinhala = false;
                            break;
                        }
                    }

                    if (ValueInSinhala)
                    {
                        for (int j = 0; j < HeaderSinhala.Count; j++)
                        {
                            if (TableResults.Columns[i].HeaderText == HeaderSinhala[j])
                            {
                                TableResults.Columns[i].HeaderText = HeaderEnglish[j];
                                break;
                            }
                        }
                    }
                }
            }

            void TranslateMOEAndPermission()
            {
                try
                {
                    List<string> MOEEnglish = new List<string> { "Society", "Department", "Tenderer", "Special Projects" };
                    List<string> MOESinhala = new List<string> { "සමිති", "දෙපාර්තමේන්තු", "ටෙන්ඩර්", "විශේෂ ව්‍යාපෘති" };

                    for (int i = 0; i < TableResults.Rows.Count - 1; i++)
                    {                        
                        bool ValueInSinhala = true;

                        for (int j = 0; j < MOEEnglish.Count; j++)
                        {
                            if (TableResults.Rows[i].Cells["MethodOfExecution"].Value.ToString() == MOEEnglish[j])
                            {
                                TableResults.Rows[i].Cells["MethodOfExecution"].Value = MOESinhala[j];
                                ValueInSinhala = false;
                                break;
                            }
                        }

                        if (ValueInSinhala)
                        {
                            for (int j = 0; j < MOESinhala.Count; j++)
                            {
                                if (TableResults.Rows[i].Cells["MethodOfExecution"].Value.ToString() == MOESinhala[j])
                                {
                                    TableResults.Rows[i].Cells["MethodOfExecution"].Value = MOEEnglish[j];
                                    break;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("'Method Of Execution' No Available.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                try
                {
                    List<string> PermissionEnglish = new List<string> { "Chief Engineer", "Commissioner", "Council" };
                    List<string> PermissionSinhala = new List<string> { "ප්‍රධාන ඉංජිනේරු", "නාගරික කොමසාරිස්", "සභාව" };

                    for (int i = 0; i < TableResults.Rows.Count - 1; i++)
                    {
                        bool ValueInSinhala = true;

                        for (int j = 0; j < PermissionEnglish.Count; j++)
                        {
                            if (TableResults.Rows[i].Cells["Permission"].Value.ToString() == PermissionEnglish[j])
                            {
                                TableResults.Rows[i].Cells["Permission"].Value = PermissionSinhala[j];
                                ValueInSinhala = false;
                                break;
                            }
                        }

                        if (ValueInSinhala)
                        {
                            for (int j = 0; j < PermissionSinhala.Count; j++)
                            {
                                if (TableResults.Rows[i].Cells["Permission"].Value.ToString() == PermissionSinhala[j])
                                {
                                    TableResults.Rows[i].Cells["Permission"].Value = PermissionEnglish[j];
                                    break;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("'Permission' No Available.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void TableResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GridViewColumnOptionsMenuStrip.Items.Clear();
                GridViewColumnOptionsMenuStrip.Items.Add("Remove Column");
                GridViewColumnOptionsMenuStrip.Items.Add("Sort By Ascending");
                GridViewColumnOptionsMenuStrip.Items.Add("Sort By Descending");
                if (TableResults.Columns[e.ColumnIndex].HeaderText == "Start Date" || TableResults.Columns[e.ColumnIndex].HeaderText == "ආරම්භක දිනය" ||
                    TableResults.Columns[e.ColumnIndex].HeaderText == "Progress Updated Date" || TableResults.Columns[e.ColumnIndex].HeaderText == "ප්‍රගතිය යාවත්කාලීන කල දිනය")
                {
                    GridViewColumnOptionsMenuStrip.Items.Add("Remove Date Name");
                }
                GridViewColumnOptionsMenuStrip.Show();
                GridViewColumnOptionsMenuStrip.Top = MousePosition.Y;
                GridViewColumnOptionsMenuStrip.Left = MousePosition.X;
                SelectedColumn = e.ColumnIndex;
            }
        }

        private void GridViewOptionsMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "Remove Column":
                    {
                        TableResults.Columns.Remove(TableResults.Columns[SelectedColumn]);
                        break;
                    }
                case "Remove Date Name":
                    {
                        for (int i = 0; i < TableResults.Rows.Count - 1; i++)
                        {
                            foreach (string DayName in DayNames)
                            {
                                try
                                {
                                    try
                                    {
                                        TableResults.Rows[i].Cells["StartDate"].Value = TableResults.Rows[i].Cells["StartDate"].Value.ToString().Replace(DayName, "");
                                        TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value = TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value.ToString().Replace(DayName, "");
                                    }
                                    catch
                                    {
                                        TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value = TableResults.Rows[i].Cells["ProgressUpdatedDate"].Value.ToString().Replace(DayName, "");
                                    }
                                }
                                catch
                                {
                                    TableResults.Rows[i].Cells["StartDate"].Value = TableResults.Rows[i].Cells["StartDate"].Value.ToString().Replace(DayName, "");
                                }
                            }
                        }
                        break;
                    }
                case "Sort By Ascending":
                    {
                        TableResults.Sort(TableResults.Columns[SelectedColumn], ListSortDirection.Ascending);
                        break;
                    }
                case "Sort By Descending":
                    {
                        TableResults.Sort(TableResults.Columns[SelectedColumn], ListSortDirection.Descending);
                        break;
                    }
            }
            SelectedColumn = -1;
        }

        private void TableResults_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > 0) 
            {
                GridViewRowOptionsMenuStrip.Show();
                GridViewRowOptionsMenuStrip.Top = MousePosition.Y;
                GridViewRowOptionsMenuStrip.Left = MousePosition.X;
                GridViewRowOptionsMenuStrip.Items[0].Text = "Remove Row";
                SelectedRow = e.RowIndex;
            }
        }

        private void GridViewRowOptionsMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "Remove Row":
                    {
                        DeletedColumn.Clear();
                        try
                        {
                            for (int i = 0; i < TableResults.ColumnCount; i++)
                            {
                                DeletedColumn.Add(TableResults.Rows[SelectedRow].Cells[i].Value.ToString());
                            }
                            TableResults.Rows.RemoveAt(SelectedRow);
                            GridViewRowOptionsMenuStrip.Items[1].Enabled = true;
                        }
                        catch { }
                        CountRowsAndColumns();                        
                        break;
                    }
                case "Undo":
                    {
                        int n = TableResults.Rows.Add();
                        for (int i = 0; i < DeletedColumn.Count; i++)
                        {
                            TableResults.Rows[n].Cells[i].Value = DeletedColumn[i];
                        }
                        CountRowsAndColumns();
                        GridViewRowOptionsMenuStrip.Items[1].Enabled = false;
                        break;
                    }
            }
            SelectedRow = 0;
        }
    }
}
