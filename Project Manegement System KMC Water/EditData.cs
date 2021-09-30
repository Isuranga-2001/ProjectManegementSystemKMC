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

namespace Project_Manegement_System_KMC_Water
{
    public partial class EditData : Form
    {
        public EditData()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        MultiFunctions multiFunctions = new MultiFunctions();

        Dictionary<DataGridViewCellEventArgs, string> UpdateQueryArray = new Dictionary<DataGridViewCellEventArgs, string> { };

        string CurrentValue = null, SelectedProjectID = null;

        private void EditData_Load(object sender, EventArgs e)
        {
            ResetObjects();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(1280, 720);
        }

        void ResetObjects()
        {
            txtSearch.Clear();
            UpdateQueryArray.Clear();
            CurrentValue = null;
            LoadTableData("");
            TableProjectDataSet.Sort(TableProjectDataSet.Columns[0], ListSortDirection.Ascending);
        }

        public void LoadTableData(string Condition)
        {
            DataTable ReturnedDataTable = sqlFunctions.RetrieveDataOnDataTable(
                "SELECT Project.ProjectID, Project.Discription, Project.EstimatedMoney, Project.Permission, " +
                "Project.StartDate, Project.MethodOfExecution," +
                "Estimator.ZoneNo, ExpenditureHead.EH_Main, ExpenditureHead.EH_Sub1, " +
                "ExpenditureHead.EH_Sub2, ExpenditureHead.EH_Sub3 " +
                "FROM Project INNER JOIN Estimator ON Project.EstimatorNO = Estimator.EstimatorNO " +
                "INNER JOIN ExpenditureHeadProject ON Project.ProjectID = ExpenditureHeadProject.ProjectID " +
                "INNER JOIN ExpenditureHead ON ExpenditureHeadProject.EHID = ExpenditureHead.EHID " + Condition);

            List<string> ColumnNameList = new List<string>
            {
                "Index No","Project Description","Estimated Money","Permission","Start Date","Method Of Execution",
                "Section","Expenditure Head"
            };

            foreach (DataRow ItemRow in ReturnedDataTable.Rows)
            {
                if (ItemRow.ItemArray[10].ToString().Trim() != "")
                {
                    ItemRow[7] = ItemRow.ItemArray[7] + "-" + ItemRow.ItemArray[8] + "-" + ItemRow.ItemArray[9] + "-" + ItemRow.ItemArray[10];
                }
                else
                {
                    ItemRow[7] = ItemRow.ItemArray[7] + "-" + ItemRow.ItemArray[8] + "-" + ItemRow.ItemArray[9];
                }
            }

            // Delete Extra Expenditure Head Columns form Data Table 
            for (var i = 0; i < 3; i++)
            {
                ReturnedDataTable.Columns.RemoveAt(8);
            }

            for (int i = 0; i < ColumnNameList.Count; i++)
            {
                ReturnedDataTable.Columns[i].ColumnName = ColumnNameList[i];
            }

            TableProjectDataSet.Columns.Clear();
            TableProjectDataSet.DataSource = ReturnedDataTable;
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() != "")
            {
                LoadTableData("WHERE Project.Discription LIKE N'%" + txtSearch.Text.Trim() + "%'");
            }
        }

        private void MenuDefaultData_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridViewCellEventArgs CellAdress = (DataGridViewCellEventArgs)MenuDefaultData.Tag;

            TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[CellAdress.ColumnIndex].Value = e.ClickedItem.ToString();
        }

        private void TableProjectDataSet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectedProjectID = TableProjectDataSet.Rows[e.RowIndex].Cells[0].Value.ToString();

                CurrentValue = TableProjectDataSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                if (e.ColumnIndex > 4 || e.ColumnIndex == 3)
                {
                    AddDefaultDataIntoMenu(e);
                }

                if (e.RowIndex == TableProjectDataSet.Rows.Count - 1) 
                {
                    TableProjectDataSet.Rows[e.RowIndex].Cells[0].Value = (Convert.ToInt32(sqlFunctions.SQLRead(
                        "SELECT MAX(ProjectID) AS LastProjectID FROM dbo.Project", "LastProjectID")[0]) + 1).ToString();
                }
            }
        }

        List<string> FindDefaultDataList(int Column_Index)
        {
            switch (Column_Index)
            {
                case 6: // Section or ZoneNO
                    {
                        List<string> DefaultDataList = sqlFunctions.SQLRead("SELECT DISTINCT ZoneNo " +
                            "FROM Estimator WHERE Active='YES'", "ZoneNo");

                        if (DefaultDataList == null)
                        {
                            MessageBox.Show("Can't find data for Section feild", "Data Missing",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            multiFunctions.NavigateTo("Settings", this);
                            break;
                        }
                        else
                        {
                            return DefaultDataList;
                        }
                    }
                case 5: // MOE
                    {
                        return new List<string> { "Department", "Tenderer", "Special Projects" };
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
                            multiFunctions.NavigateTo("Settings", this);
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
            }

            MessageBox.Show("Something Went Wrong", "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        void AddDefaultDataIntoMenu(DataGridViewCellEventArgs CellAdress)
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

        void CheckInputValue(DataGridViewCellEventArgs CellAdress)
        {
            bool ErrorInputData = true;
            int ColumnID = CellAdress.ColumnIndex;

            string ValueOfSelectedCell =
                TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[ColumnID].Value.ToString();

            if (ColumnID == 2)
            {
                // Check value type is float using method in multiFunction class

                TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[ColumnID].Value = 
                    Convert.ToDouble(multiFunctions.TypeCheckInput(ValueOfSelectedCell.Trim(), "N.F", 2));
            }
            else if (ColumnID == 3 || ColumnID > 4)
            {
                // Check value is legel

                foreach (string DefualtItem in FindDefaultDataList(ColumnID))
                {
                    if (ValueOfSelectedCell.Trim() == DefualtItem.Trim())
                    {
                        ErrorInputData = false;
                        break;
                    }
                }

                if (ErrorInputData)
                {
                    // set previous value

                    MessageBox.Show("Invalid input. You can input data only using menu", "Input Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[ColumnID].Value = CurrentValue;
                }
            }
            else if (ColumnID == 4)
            {
                // check value legel for date format

                try
                {
                    if (ValueOfSelectedCell.Length <= 10)
                    {
                        string[] DateParts = ValueOfSelectedCell.Split('-');

                        if (DateParts.Length == 3)
                        {
                            if (Convert.ToInt32(DateParts[0]) >= 2020 && Convert.ToInt32(DateParts[0]) <= 2029 &&
                                Convert.ToInt32(DateParts[1]) >= 1 && Convert.ToInt32(DateParts[1]) <= 12 &&
                                Convert.ToInt32(DateParts[2]) >= 1 &&
                                Convert.ToInt32(DateParts[2]) <= multiFunctions.NoOfDays(Convert.ToInt32(DateParts[1])))
                            {
                                ErrorInputData = false;
                            }
                        }
                    }
                    else if (ValueOfSelectedCell.Trim().Length <= 10)
                    {
                        TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[ColumnID].Value = ValueOfSelectedCell.Trim();
                        ErrorInputData = false;
                    }
                }
                catch
                {
                    ErrorInputData = true;
                }

                if (ErrorInputData)
                {
                    // set previous value

                    MessageBox.Show("Invalid input. Your input is not in legel format", "Input Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[ColumnID].Value = CurrentValue;
                }
            }
            else if (ColumnID == 1)
            {
                TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[ColumnID].Value = 
                    TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[ColumnID].Value.ToString().Trim();
            }

            if (TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[ColumnID].Value.ToString() != CurrentValue)
            {
                AddToUpdateQueryArray(CellAdress);
            }
        }

        void AddToUpdateQueryArray(DataGridViewCellEventArgs CellAdress)
        {
            bool AvaiableOnArray = false;

            foreach (KeyValuePair<DataGridViewCellEventArgs, string> QueryKey in UpdateQueryArray)
            {
                if (QueryKey.Key == CellAdress)
                {
                    AvaiableOnArray = true;
                    break;
                }
            }

            if (!AvaiableOnArray)
            {
                UpdateQueryArray.Add(CellAdress, "");
            }

            string ProjectID = TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[0].Value.ToString().Trim(),
                Value = TableProjectDataSet.Rows[CellAdress.RowIndex].Cells[CellAdress.ColumnIndex].Value.ToString().Trim();

            Dictionary<int, string> ColumnNameArray = new Dictionary<int, string>
            {
                { 1,"Discription" },{ 4,"StartDate" },{ 2,"EstimatedMoney" },{ 3,"Permission" },{ 5,"MethodOfExecution" }
            };

            if (CellAdress.ColumnIndex >= 1 && CellAdress.ColumnIndex <= 5) 
            {
                UpdateQueryArray[CellAdress] = "UPDATE Project SET " + ColumnNameArray[CellAdress.ColumnIndex] + 
                    "=N'" + Value + "' WHERE ProjectID='" + ProjectID + "'";
            }
            else if (CellAdress.ColumnIndex == 7)
            {
                try
                {
                    string[] EHParts = Value.Split('-');
                    string EHID;

                    if (EHParts.Length > 2) 
                    {
                        EHID = sqlFunctions.SQLRead("SELECT [EHID] FROM [dbo].[ExpenditureHead] " +
                            "WHERE ([EH_Main]='" + EHParts[0].Trim() + "' AND [EH_Sub1]='" + EHParts[1].Trim() + "' " +
                            "AND [EH_Sub2]='" + EHParts[2].Trim() + "')", "EHID")[0];
                    }
                    else
                    {
                        EHID = sqlFunctions.SQLRead("SELECT [EHID] FROM [dbo].[ExpenditureHead] " +
                            "WHERE ([EH_Main]='" + EHParts[0].Trim() + "' AND [EH_Sub1]='" + EHParts[1].Trim() + "' " +
                            "AND [EH_Sub2]='" + EHParts[2].Trim() + "' AND [EH_Sub3]='" + EHParts[3].Trim() + "')", "EHID")[0];
                    }

                    UpdateQueryArray[CellAdress] = "UPDATE ExpenditureHeadProject SET EHID='" + EHID + "' WHERE ProjectID='" + ProjectID + "'";
                }
                catch
                {
                    MessageBox.Show("Can't load data form datatable", "Data Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (CellAdress.ColumnIndex == 6)
            {
                try
                {
                    string EstimatorNo = sqlFunctions.SQLRead("SELECT EstimatorNo FROM Estimator WHERE ZoneNo='" + Value + "'", "EstimatorNo")[0];

                    UpdateQueryArray[CellAdress] = "UPDATE Project SET EstimatorNo='" + EstimatorNo + "' WHERE ProjectID='" + ProjectID + "'";
                }
                catch
                {
                    MessageBox.Show("Can't load data form datatable", "Data Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TableProjectDataSet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentValue != null)
            {
                if (CurrentValue != "")
                {
                    if (e.ColumnIndex == 0 &&
                    TableProjectDataSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() != CurrentValue)
                    {
                        MessageBox.Show("You Can't Change Index No", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TableProjectDataSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Convert.ToInt32(CurrentValue);
                    }
                    else
                    {
                        CheckInputValue(e);
                        txtSearch.Focus();
                    }
                }
                else
                {
                    if (e.ColumnIndex > 1)
                    {
                        CheckInputValue(e);
                    }

                    bool CompleteDataInput = true;

                    foreach (DataGridViewCell dataGridCell in TableProjectDataSet.Rows[e.RowIndex].Cells)
                    {
                        if (dataGridCell.Value == null)
                        {
                            CompleteDataInput = false;
                            break;
                        }
                    }

                    if (CompleteDataInput)
                    {
                        AddNewRowIntoDatabase(e.RowIndex);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (UpdateQueryArray.Count > 0)
            {
                int Passed = 0, Faild = 0;
                foreach (KeyValuePair<DataGridViewCellEventArgs, string> Query in UpdateQueryArray)
                {
                    if (!sqlFunctions.ExecuteSQL(Query.Value))
                        Faild += 1;
                    else
                        Passed += 1;
                }

                if (Faild != 0)
                    MessageBox.Show("Some data could not be saved successfully.", "Save Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Data Saved Successfully.", "Save Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetObjects();
            }
        }

        void AddNewRowIntoDatabase(int SelectedRow)
        {
            try
            {
                DataGridViewRow dataRow = TableProjectDataSet.Rows[SelectedRow];

                // Check Enter Value for every required fields

                bool CompleteAllRequairedFields = true;

                foreach (DataGridViewCell gridViewCell in dataRow.Cells)
                {
                    if (gridViewCell.Value == null)
                    {
                        CompleteAllRequairedFields = false;
                        break;
                    }
                    if (gridViewCell.Value.ToString().Trim() == "")
                    {
                        CompleteAllRequairedFields = false;
                        break;
                    }
                }

                if (CompleteAllRequairedFields)
                {
                    string EstimatorNo = sqlFunctions.SQLRead("select EstimatorNO from Estimator WHERE ZoneNo = N'" + dataRow.Cells[6].Value + "' ;", "EstimatorNO")[0];

                    string[] EHParts = dataRow.Cells[7].Value.ToString().Trim().Split('-');
                    string EHID;

                    if (EHParts.Length > 3)
                    {
                        EHID = sqlFunctions.SQLRead("SELECT EHID FROM ExpenditureHead " +
                            "WHERE EH_Main='" + EHParts[0] + "' AND EH_Sub1='" + EHParts[1] + "' " +
                            "AND EH_Sub2='" + EHParts[2] + "' AND EH_Sub3='" + EHParts[3] + "'", "EHID")[0];
                    }
                    else
                    {
                        EHID = sqlFunctions.SQLRead("SELECT EHID FROM ExpenditureHead " +
                            "WHERE EH_Main='" + EHParts[0] + "' AND EH_Sub1='" + EHParts[1] + "' " +
                            "AND EH_Sub2='" + EHParts[2] + "'", "EHID")[0];
                    }

                    List<string> QueryList = new List<string>
                    {
                        "INSERT INTO[dbo].[Project]([ProjectID], [Discription], [EstimatedMoney], " +
                        "[Permission], [StartDate], [MethodOfExecution], [EstimatorNO]) " +
                        "VALUES(" + dataRow.Cells[0].Value.ToString().Trim() + ", " +
                        "N'" + dataRow.Cells[1].Value.ToString().Trim() + "', " +
                        dataRow.Cells[2].Value.ToString().Trim() + ", N'" +
                        dataRow.Cells[3].Value.ToString().Trim() + "', N'" +
                        dataRow.Cells[4].Value.ToString().Trim() + "', N'" +
                        dataRow.Cells[5].Value.ToString().Trim() + "', "
                        + EstimatorNo + ")",

                        "INSERT INTO [dbo].[ExpenditureHeadProject] ([EHID], [ProjectID]) " +
                        "VALUES (N'" + EHID + "', " + dataRow.Cells[0].Value.ToString().Trim() +")",

                        "INSERT INTO [dbo].[ProgressOfProject] ([ProjectID], [Progress], [Date], [Details]) " +
                        "VALUES ('" + dataRow.Cells[0].Value.ToString().Trim() + "','0'," +
                        "'" + dataRow.Cells[4].Value.ToString().Trim()+ "','Not Started')"
                    };

                    for (var i = 0; i < QueryList.Count; i++)
                    {
                        if (sqlFunctions.ExecuteSQL(QueryList[i]))
                        {
                            if (i + 1 == QueryList.Count)
                            {
                                MessageBox.Show("Data Row Added Successfully", "Successfully Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Input Data !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            TableProjectDataSet.Rows.RemoveAt(SelectedRow);
                            break;
                        }
                    }

                    ResetObjects();
                }
                else
                {
                    if (dataRow.Cells[7].Value.ToString().Trim() != "") 
                    {
                        MessageBox.Show("Please Enter All Required Fields !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Found!" + ex.Message, "Something Went Wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TableProjectDataSet_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (CurrentValue != null && CurrentValue == "")
            {
                TableProjectDataSet.Rows[e.RowIndex - 1].Cells[0].Value = (Convert.ToInt32(sqlFunctions.SQLRead(
                    "SELECT MAX(ProjectID) AS LastProjectID FROM dbo.Project", "LastProjectID")[0]) + 1).ToString();
            }
        }

        private void TableProjectDataSet_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (CurrentValue != null)
            {
                if (MessageBox.Show("Do you want to remove selected row", "Conform delete Row",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (MessageBox.Show("You can't restore " + SelectedProjectID + " again !", "Information",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        sqlFunctions.ExecuteSQL("DELETE FROM [dbo].[ExpenditureHeadProject] " +
                            "WHERE ([ProjectID] = " + SelectedProjectID + ")");

                        sqlFunctions.ExecuteSQL("DELETE FROM [dbo].[ProgressOfProject] " +
                            "WHERE ([ProjectID] = " + SelectedProjectID + ")");

                        sqlFunctions.ExecuteSQL("DELETE FROM [dbo].[Project] " +
                            "WHERE ([ProjectID] = " + SelectedProjectID + ")");
                    }
                }

                ResetObjects();
            }
        }

        private void FormNavigationButton_Click(object sender, EventArgs e)
        {
            Guna2CircleButton btnNavigation = (Guna2CircleButton)sender;
            multiFunctions.NavigateTo(btnNavigation.Tag.ToString(), this);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            multiFunctions.NavigateTo(btnClose.Tag.ToString(), this);
        }
    }
}
