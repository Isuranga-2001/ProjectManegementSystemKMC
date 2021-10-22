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

namespace Project_Manegement_System___KMC
{
    public partial class AddNewTableMode : Form
    {
        public AddNewTableMode()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();

        const string SelectAllQuery = "SELECT Project.ProjectID, Project.Discription, Project.EstimatedMoney, Project.Permission,Project.MethodOfExecution, " +
            "Project.StartDate, Estimator.EstName FROM Project,Estimator WHERE Project.EstimatorNo = Estimator.EstimatorNo";

        bool UpdateEnabled = false;
        int SelectedRow = 0;
        string CurrentValue = null;

        public string SelectedProjectID = null;
        public bool NavigateToProgress = false;

        DataGridView SavedDataTable = new DataGridView();

        Dictionary<string, int> ColumnOrder = new Dictionary<string, int> { };

        private void AddNewTableMode_Load(object sender, EventArgs e)
        {
            RefreshTable(SelectAllQuery);
            UpdateEnabled = true;

            txtSearch.Visible = false;
            btnSearch.Left = txtSearch.Left;

            SortByMenu.Items.Clear();
            foreach (DataGridViewColumn gridViewColumn in TableResults.Columns)
            {
                SortByMenu.Items.Add(gridViewColumn.HeaderText);
                ColumnOrder.Add(gridViewColumn.HeaderText, SortByMenu.Items.Count - 1);
            }
        }

        public void RefreshTable(string Query)
        {            
            TableResults.Columns.RemoveAt(7);
            TableResults.Columns.RemoveAt(7);
            if (!(sqlFunctions.RetrieveDataTable(Query, TableResults)))
            {
                sqlFunctions.ErrorMessage();
            }
            else
            {
                TableResults.Columns.Add("ExpenditureHead", "Expenditure Head");
                TableResults.Columns.Add("Name", "Proposer");
                List<string> ExpenditureHead;
                int NoOfRows = TableResults.RowCount - 1;
                string ProjectID;

                /// Expenditure Head
                for (var i = 0; i < NoOfRows; i++)
                {
                    ProjectID = TableResults.Rows[i].Cells[0].Value.ToString();

                    ExpenditureHead = sqlFunctions.SQLRead(
                            "SELECT ExpenditureHead.EH_Main, ExpenditureHead.EH_Sub1, ExpenditureHead.EH_Sub2, " +
                            "ExpenditureHead.EH_Sub3 " +
                            "FROM ExpenditureHead,ExpenditureHeadProject " +
                            "WHERE ExpenditureHead.EHID=ExpenditureHeadProject.EHID AND " +
                            "ExpenditureHeadProject.ProjectID='" + ProjectID + "'",
                            "EH_Main EH_Sub1 EH_Sub2 EH_Sub3");

                    if (ExpenditureHead == null)
                    {
                        TableResults.Rows[i].Cells["ExpenditureHead"].Value = "Not Assign";
                        continue;
                    }

                    TableResults.Rows[i].Cells["ExpenditureHead"].Value =
                        (ExpenditureHead[0].Trim() + "-" + ExpenditureHead[1].Trim() + "-" + ExpenditureHead[2].Trim() + "-" + ExpenditureHead[3]).Trim();

                    string CurrentValue = TableResults.Rows[i].Cells["ExpenditureHead"].Value.ToString();
                    if (CurrentValue[CurrentValue.Count() - 1] == '-')
                    {
                        TableResults.Rows[i].Cells["ExpenditureHead"].Value = CurrentValue.Remove(CurrentValue.Length - 1);
                    }
                }

                List<string> PerosnName;
                NoOfRows = TableResults.RowCount - 1;

                /// Person
                for (var i = 0; i < NoOfRows; i++)
                {
                    ProjectID = TableResults.Rows[i].Cells[0].Value.ToString();

                    PerosnName = sqlFunctions.SQLRead("SELECT Person.Name FROM Person,PersonProject " +
                            "WHERE Person.PID=PersonProject.PID " +
                            "AND PersonProject.ProjectID='" + ProjectID + "'", "Name");

                    if (PerosnName == null)
                    {
                        TableResults.Rows[i].Cells["Name"].Value = "Not Assign";
                        continue;
                    }
                    TableResults.Rows[i].Cells["Name"].Value = PerosnName[0];
                }
            }

            ResetSavedTable();
        }

        public void ResetSavedTable()
        {
            SavedDataTable.Columns.Clear();

            foreach (DataGridViewColumn TableColumnEditTable in TableResults.Columns)
            {
                SavedDataTable.Columns.Add(TableColumnEditTable.Name, TableColumnEditTable.HeaderText);
            }

            int n;
            foreach (DataGridViewRow TableRowEditTable in TableResults.Rows)
            {
                n = SavedDataTable.Rows.Add();

                for (int i = 0; i < TableResults.Columns.Count; i++)
                {
                    SavedDataTable.Rows[n].Cells[i].Value = TableRowEditTable.Cells[i].Value;
                }
            }
        }

        private void TableResults_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (UpdateEnabled)
            {
                //TableResults.Rows[e.RowIndex - 1].Cells[9].Value = Properties.Resources.Save;
                //TableResults.Rows[e.RowIndex - 1].Cells[10].Value = Properties.Resources.Remove;
            }
        }

        private void TableResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectedRow = e.RowIndex;
                                
                if (e.RowIndex == TableResults.Rows.Count - 1)
                {
                    TableResults.Rows[e.RowIndex].Cells[0].Value = (Convert.ToInt32(sqlFunctions.SQLRead(
                        "SELECT MAX(ProjectID) AS LastProjectID FROM dbo.Project", "LastProjectID")[0]) + 1).ToString();
                }
            }            
        }

        private void TableResults_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (UpdateEnabled)
            {
                RemoveRaw(e.RowIndex, true);
                ResetSavedTable();
            }
        }

        void RemoveRaw(int SelectedRowIndex, bool Deleted)
        {
            UpdateEnabled = false;

            if (MessageBox.Show("Do you want to delete selected row", "Conform Update",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!Deleted)
                {
                    string SelectedRowProjectID = TableResults.Rows[SelectedRowIndex].Cells[0].Value.ToString().Trim();

                    sqlFunctions.ExecuteSQL("DELETE FROM [dbo].[ExpenditureHeadProject] " +
                        "WHERE ([ProjectID] = " + SelectedRowProjectID + ")");

                    sqlFunctions.ExecuteSQL("DELETE FROM [dbo].[ProgressOfProject] " +
                        "WHERE ([ProjectID] = " + SelectedRowProjectID + ")");

                    sqlFunctions.ExecuteSQL("DELETE FROM [dbo].[PersonProject] " +
                        "WHERE ([ProjectID] = " + SelectedRowProjectID + ")");

                    sqlFunctions.ExecuteSQL("DELETE FROM [dbo].[Project] " +
                        "WHERE ([ProjectID] = " + SelectedRowProjectID + ")");

                    TableResults.Rows.RemoveAt(SelectedRowIndex);
                }
            }
            else
            {
                RefreshTable(SelectAllQuery);
            }

            UpdateEnabled = true;
        }

        private void AddNewTableMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ProgressBarWaiting.Visible)
            {
                if (MessageBox.Show("Do you want to close without save", "Save", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) 
                {
                    this.Close();
                }
                else
                {
                    //this.
                }               
            }
        }

        async void OpeartionCalled(string OparationName)
        {
            UpdateEnabled = false;

            switch (OparationName)
            {
                case "Search":
                    {
                        if (txtSearch.Visible)
                        {
                            txtSearch.Visible = false;
                            btnSearch.Left = txtSearch.Left;

                            if (txtSearch.Text.Trim() != "")
                            {
                                RefreshTable("SELECT Project.ProjectID, Project.Discription, Project.EstimatedMoney, " +
                                    "Project.Permission,Project.MethodOfExecution, Project.StartDate, Estimator.EstName " +
                                    "FROM Project,Estimator WHERE Project.EstimatorNo = Estimator.EstimatorNo AND " +
                                    "Project.Discription Like N'%" + txtSearch.Text.Trim() + "%'");
                            }
                        }
                        else
                        {
                            btnSearch.Left = txtSearch.Right - btnSearch.Width;
                            txtSearch.Visible = true;
                        }
                        break;
                    }
                case "Refresh":
                    {
                        RefreshTable(SelectAllQuery);
                        break;
                    }
                case "Edit":
                    {
                        EditDataCustomize form = new EditDataCustomize();

                        form.ProjectID = TableResults.Rows[SelectedRow].Cells[0].Value.ToString();
                        string[] EHParts = TableResults.Rows[SelectedRow].Cells[7].Value.ToString().Split('-');
                        form.Person = TableResults.Rows[SelectedRow].Cells[8].Value.ToString();

                        if (form.Person == "Not Assign")
                            form.Person = "";
                        if (EHParts[0] == "Not Assign")
                            form.txtEH.Text = "";
                        else
                        {
                            if (EHParts.Length > 3)
                            {
                                form.txtEH.Text = EHParts[0] + " - " + EHParts[1] + " - " + EHParts[2] + " - " + EHParts[3];
                                form.EHSub3 = EHParts[3];
                            }
                            else
                            {
                                form.txtEH.Text = EHParts[0] + " - " + EHParts[1] + " - " + EHParts[2];
                                form.EHSub3 = "";
                            }

                            form.EHMain = EHParts[0];
                            form.EHSub1 = EHParts[1];
                            form.EHSub2 = EHParts[2];
                        }

                        form.MOE = TableResults.Rows[SelectedRow].Cells[4].Value.ToString();
                        form.Permission = TableResults.Rows[SelectedRow].Cells[3].Value.ToString();
                        form.StartDate = TableResults.Rows[SelectedRow].Cells[5].Value.ToString();
                        form.EstimatedMoney = TableResults.Rows[SelectedRow].Cells[2].Value.ToString();
                        form.Estimator = TableResults.Rows[SelectedRow].Cells[6].Value.ToString();
                        form.Description = TableResults.Rows[SelectedRow].Cells[1].Value.ToString();
                        
                        form.ShowDialog();
                        RefreshTable(SelectAllQuery);

                        break;
                    }
                case "Remove":
                    {
                        RemoveRaw(SelectedRow, false);
                        break;
                    }
                case "Progress":
                    {
                        SelectedProjectID = TableResults.Rows[SelectedRow].Cells[0].Value.ToString().Trim();
                        NavigateToProgress = true;
                        this.Close();
                        break;
                    }
                case "Save":
                    {
                        if (MessageBox.Show("Do you want to save data","Conform Save", 
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            ProgressBarWaiting.Visible = true;
                            ProgressBarWaiting.Value = 0;
                            ProgressBarWaiting.Maximum = TableResults.Rows.Count - 1;

                            await SaveData(ProgressBarWaiting, TableResults, SavedDataTable, this);

                            ProgressBarWaiting.Visible = false;
                            RefreshTable(SelectAllQuery);
                        }
                        break;
                    }
            }

            UpdateEnabled = true;
        }

        public static async Task SaveData(Guna2ProgressBar ProgressBarWaitingForSave,
            Guna2DataGridView EditedTable, DataGridView SavedTable, AddNewTableMode ParentForm)
        {
            SQLFunctions sqlFunctions = new SQLFunctions();

            List<string> DataBaseColumnNames = new List<string> 
            { 
                "ProjectID", "Discription", "EstimatedMoney", "Permission", "MethodOfExecution", "StartDate"
            };

            string ProjectID,EditedValue, EstNo;
            int Failed = 0;

            List<string> ProjectDetailsList = new List<string> { };

            await Task.Run(() =>
            {
                for (int i = 0; i < EditedTable.Rows.Count - 1; i++)
                {
                    ProgressBarWaitingForSave.Value += 1;

                    ProjectID = EditedTable.Rows[i].Cells[0].Value.ToString().Trim();

                    ProjectDetailsList = sqlFunctions.SQLRead("SELECT Discription FROM Project WHERE ProjectID='" + ProjectID + "'", "Discription");

                    if (ProjectDetailsList == null)
                    {
                        try
                        {
                            bool NullValueAvaiable = false;

                            foreach (DataGridViewCell NewRowCell in EditedTable.Rows[i].Cells)
                            {
                                if (NewRowCell == null)
                                {
                                    NullValueAvaiable = true;
                                    break;
                                }
                            }

                            if (!NullValueAvaiable)
                            {
                                string StartDate = EditedTable.Rows[i].Cells[5].Value.ToString().Trim();
                                string[] EHParts = EditedTable.Rows[i].Cells[7].Value.ToString().Trim().Split('-');

                                string EstimatorNo = sqlFunctions.SQLRead("select EstimatorNO from Estimator " +
                                    "WHERE EstName = N'" + EditedTable.Rows[i].Cells[6].Value.ToString().Trim() + "' ;", "EstimatorNO")[0];

                                string EHID;

                                if (EHParts.Length > 3)
                                {
                                    EHID = sqlFunctions.SQLRead("SELECT [EHID] FROM [dbo].[ExpenditureHead] " +
                                        "WHERE ([EH_Main]='" + EHParts[0] + "' AND [EH_Sub1]='" + EHParts[1] + "' " +
                                        "AND [EH_Sub2]='" + EHParts[2] + "' AND [EH_Sub3]='" + EHParts[3] + "')", "EHID")[0];
                                }
                                else
                                {
                                    EHID = sqlFunctions.SQLRead("SELECT [EHID] FROM [dbo].[ExpenditureHead] " +
                                        "WHERE ([EH_Main]='" + EHParts[0] + "' AND [EH_Sub1]='" + EHParts[1] + "' " +
                                        "AND [EH_Sub2]='" + EHParts[2] + "')", "EHID")[0];
                                }

                                string PersonName = sqlFunctions.SQLRead("select PID from Person " +
                                    "WHERE Name = N'" + EditedTable.Rows[i].Cells[8].Value.ToString().Trim() + "' ;", "PID")[0];

                                List<string> QueryList = new List<string>
                                    {
                                        "INSERT INTO[dbo].[Project]([ProjectID], [Discription], [EstimatedMoney], [Permission], " +
                                        "[StartDate], [MethodOfExecution], [EstimatorNO]) " +
                                        "VALUES(" + ProjectID + ", N'" + EditedTable.Rows[i].Cells[1].Value.ToString().Trim() + "', " +
                                        EditedTable.Rows[i].Cells[2].Value.ToString().Trim() + ", N'"
                                        + EditedTable.Rows[i].Cells[3].Value.ToString().Trim() + "', N'" + StartDate + "', N'" +
                                        EditedTable.Rows[i].Cells[4].Value.ToString().Trim() + "', " + EstimatorNo + ")",

                                        "INSERT INTO [dbo].[ExpenditureHeadProject] ([EHID], [ProjectID]) VALUES (N'" + EHID + "', " + ProjectID +")",

                                        "INSERT INTO [dbo].[PersonProject] ([ProjectID], [PID]) VALUES (" + ProjectID  +", N'" + PersonName + "')",

                                        "INSERT INTO [dbo].[ProgressOfProject] ([ProjectID], [Progress], [Date], [Details]) " +
                                        "VALUES ('" + ProjectID + "','0','" + StartDate + "','Not Started')"
                                    };

                                for (var k = 0; k < QueryList.Count; k++)
                                {
                                    if (!sqlFunctions.ExecuteSQL(QueryList[k]))
                                    {
                                        Failed += 1;
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                Failed += 1;
                                continue;
                            }
                        }
                        catch
                        {
                            Failed += 1;
                            continue;
                        }                        
                    }
                    else
                    {
                        for (int j = 1; j < EditedTable.Columns.Count; j++)
                        {
                            try
                            {
                                if (SavedTable.Rows[i].Cells[j].Value.ToString().Trim() !=
                                    EditedTable.Rows[i].Cells[j].Value.ToString().Trim())
                                {
                                    EditedValue = EditedTable.Rows[i].Cells[j].Value.ToString().Trim();

                                    if (EditedValue == "")
                                    {
                                        Failed += 1;
                                    }
                                    else if (j == 6)
                                    {
                                        EstNo = sqlFunctions.SQLRead("SELECT [EstimatorNO] FROM [dbo].[Estimator] " +
                                            "WHERE [EstName]=N'" + EditedValue + "'", "EstimatorNO")[0];

                                        if (EstNo == "")
                                        {
                                            Failed += 1;
                                        }
                                        else if (!sqlFunctions.ExecuteSQL("UPDATE Project SET [EstimatorNO]='" + EstNo + "' " +
                                            "WHERE ProjectID='" + ProjectID + "'"))
                                        {
                                            Failed += 1;
                                        }
                                    }
                                    else if (j == 7)
                                    {
                                        string[] EHParts = EditedValue.Split('-');
                                        string EHID = null;

                                        if (EHParts.Length > 3)
                                        {
                                            EHID = sqlFunctions.SQLRead("SELECT [EHID] FROM [dbo].[ExpenditureHead] " +
                                                "WHERE ([EH_Main]='" + EHParts[0] + "' AND [EH_Sub1]='" + EHParts[1] + "' " +
                                                "AND [EH_Sub2]='" + EHParts[2] + "' AND [EH_Sub3]='" + EHParts[3] + "')", "EHID")[0];
                                        }
                                        else
                                        {
                                            EHID = sqlFunctions.SQLRead("SELECT [EHID] FROM [dbo].[ExpenditureHead] " +
                                                "WHERE ([EH_Main]='" + EHParts[0] + "' AND [EH_Sub1]='" + EHParts[1] + "' " +
                                                "AND [EH_Sub2]='" + EHParts[2] + "')", "EHID")[0];
                                        }

                                        if (EHID == "")
                                        {
                                            Failed += 1;
                                        }
                                        else if (!sqlFunctions.ExecuteSQL("UPDATE [dbo].[ExpenditureHeadProject] " +
                                            "SET [EHID] = '" + EHID + "' " + "WHERE ([ProjectID] = '" + ProjectID + "');"))
                                        {
                                            Failed += 1;
                                        }
                                    }
                                    else if (j == 8)
                                    {
                                        string PID = sqlFunctions.SQLRead("SELECT [PID] FROM [dbo].[Person] " +
                                            "WHERE [Name]=N'" + EditedValue + "'", "PID")[0];

                                        if (PID == "")
                                        {
                                            Failed += 1;
                                        }
                                        else if (!sqlFunctions.ExecuteSQL("UPDATE [dbo].[PersonProject] SET [PID] = '" + PID + "' " +
                                            "WHERE ([ProjectID] = '" + ProjectID + "');"))
                                        {
                                            Failed += 1;
                                        }
                                    }
                                    else if (j < 6)
                                    {
                                        if (!sqlFunctions.ExecuteSQL(String.Format(
                                            "UPDATE Project SET {0}=N'{1}' WHERE ProjectID='{2}'",
                                            DataBaseColumnNames[j], EditedValue, ProjectID)))
                                        {
                                            Failed += 1;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                Failed += 1;
                                continue;
                            }
                        }
                    }                    
                }

                MessageBox.Show(String.Format("Rows Passed {0}, Failed {1}", EditedTable.Rows.Count - Failed, Failed),
                    "Save Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        private void btnOperation_Click(object sender, EventArgs e)
        {
            Guna2CircleButton btnOperation = (Guna2CircleButton)sender;
            OpeartionCalled(btnOperation.Tag.ToString());
        }

        private void MenuDefaultData_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridViewCellMouseEventArgs CellAdress = (DataGridViewCellMouseEventArgs)MenuDefaultData.Tag;

            TableResults.Rows[CellAdress.RowIndex].Cells[CellAdress.ColumnIndex].Value = e.ClickedItem.ToString();
        }

        private void TableResults_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TableRightClickMenu.Show();
                TableRightClickMenu.Left = MousePosition.X;
                TableRightClickMenu.Top = MousePosition.Y;
            }
            else
            {
                if (e.RowIndex >= 0)
                {
                    SelectedProjectID = TableResults.Rows[e.RowIndex].Cells[0].Value.ToString();

                    if (TableResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        CurrentValue = TableResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                    else
                    {
                        CurrentValue = "";
                    }

                    if (e.ColumnIndex == 5)
                    {
                        SelectDate form = new SelectDate();
                        form.ShowDialog();
                        if (form.DateChangeEnabled)
                        {
                            TableResults.Rows[e.RowIndex].Cells[5].Value = 
                                form.Calander.SelectionStart.ToLongDateString();
                        }
                    }

                    if (e.ColumnIndex == 7)
                    {
                        SelectExpenditureHead form = new SelectExpenditureHead();
                        form.ShowDialog();

                        List<string> EHParts = sqlFunctions.SQLRead("SELECT EH_Main,EH_Sub1,EH_Sub2,EH_Sub3 FROM dbo.ExpenditureHead WHERE (EHID='"
                            + Properties.Settings.Default.SelectedEHID + "')", "EH_Main EH_Sub1 EH_Sub2 EH_Sub3");
                                      
                        if (EHParts.Count > 3)
                        {
                            TableResults.Rows[e.RowIndex].Cells[7].Value = EHParts[0].Trim() + "-" + EHParts[1].Trim() + "-" + EHParts[2].Trim() + "-" + EHParts[3].Trim();
                        }
                        else
                        {
                            TableResults.Rows[e.RowIndex].Cells[7].Value = EHParts[0].Trim() + "-" + EHParts[1].Trim() + "-" + EHParts[2].Trim();
                        }
                    }
                    else if (e.ColumnIndex > 5 || e.ColumnIndex == 3 || e.ColumnIndex == 4)
                    {
                        AddDefaultDataIntoMenu(e);
                    }
                    else if (e.RowIndex == TableResults.Rows.Count - 1)
                    {
                        TableResults.Rows[e.RowIndex].Cells[0].Value = (Convert.ToInt32(sqlFunctions.SQLRead(
                            "SELECT MAX(ProjectID) AS LastProjectID FROM dbo.Project", "LastProjectID")[0]) + 1).ToString();
                    }
                }
            }
        }

        List<string> FindDefaultDataList(int Column_Index)
        {
            switch (Column_Index)
            {
                case 6: // Section or ZoneNO
                    {
                        List<string> DefaultDataList = sqlFunctions.SQLRead("SELECT DISTINCT EstName " +
                            "FROM Estimator WHERE Active='YES'", "EstName");

                        if (DefaultDataList == null)
                        {
                            MessageBox.Show("Can't find data for Section feild", "Data Missing",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        else
                        {
                            return DefaultDataList;
                        }
                    }
                case 4: // MOE
                    {
                        return new List<string> { "Society", "Department", "Tenderer", "Special Projects" };
                    }
                case 3: // Permission
                    {
                        return new List<string> { "Chief Engineer", "Commissioner", "Council" };
                    }
                case 7: // Expenditure Head
                    {
                        List<string> DefaultDataList = sqlFunctions.SQLRead("SELECT EH_Main,EH_Sub1,EH_Sub2,EH_Sub3 " +
                            "FROM ExpenditureHead WHERE Active='Yes'", "EH_Main EH_Sub1 EH_Sub2 EH_Sub3");

                        if (DefaultDataList == null)
                        {
                            MessageBox.Show("Can't find data for Expenditure Head feild", "Data Missing",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        else
                        {
                            List<string> ReturnDataList = new List<string> { };

                            for (int i = 0; i < DefaultDataList.Count; i += 4)
                            {
                                if (DefaultDataList[i + 3].Trim() == "")
                                    ReturnDataList.Add(DefaultDataList[i].Trim() + "-" + DefaultDataList[i + 1].Trim() + "-" + DefaultDataList[i + 2].Trim());
                                else
                                    ReturnDataList.Add(DefaultDataList[i].Trim() + "-" + DefaultDataList[i + 1].Trim() + "-" + DefaultDataList[i + 2].Trim() + "-" + DefaultDataList[i + 3].Trim());
                            }

                            return ReturnDataList;
                        }
                    }
                case 8:
                    {
                        List<string> DefaultDataList = sqlFunctions.SQLRead("SELECT DISTINCT Name " +
                            "FROM Person WHERE Active='YES'", "Name");

                        if (DefaultDataList == null)
                        {
                            MessageBox.Show("Can't find data for Section feild", "Data Missing",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        else
                        {
                            return DefaultDataList;
                        }
                    }
            }

            MessageBox.Show("Something Went Wrong", "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        void AddDefaultDataIntoMenu(DataGridViewCellMouseEventArgs CellAdress)
        {
            List<string> DefaultDataList = FindDefaultDataList(CellAdress.ColumnIndex);

            if (DefaultDataList != null)
            {
                if (DefaultDataList.Count > 0)
                {
                    MenuDefaultData.Items.Clear();
                    foreach (string item in DefaultDataList)
                    {
                        MenuDefaultData.Items.Add(item.Trim());
                    }

                    MenuDefaultData.Tag = CellAdress;
                    MenuDefaultData.Show();
                    MenuDefaultData.Left = MousePosition.X;
                    MenuDefaultData.Top = MousePosition.Y;
                }
            }
        }

        private void TableRightClickMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TableRightClickMenu.Close();
            if (e.ClickedItem.Tag.ToString() != "StartDate")
            {
                OpeartionCalled(e.ClickedItem.Tag.ToString());
            }
        }

        private void SortByMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TableResults.Sort(TableResults.Columns[ColumnOrder[e.ClickedItem.Text.ToString()]], ListSortDirection.Ascending);
            ResetSavedTable();
        }

        private void TableResults_Sorted(object sender, EventArgs e)
        {
            ResetSavedTable();
        }
    }
}
