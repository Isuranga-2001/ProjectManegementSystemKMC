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

namespace Project_Manegement_System_KMC_Water
{
    public partial class Filter : Form
    {
        public Filter()
        {
            InitializeComponent();
        }
                
        SQLFunctions sqlFunctions = new SQLFunctions();
        MultiFunctions multiFunctions = new MultiFunctions();
        FilterCustomization CustomFilters = new FilterCustomization();

        Dictionary<Guna2CustomCheckBox, Label> DictionarySelectColumn;
        List<string> TestingParametersList = new List<string> 
        {
            "PH", "EC", "TDS", "Tur", "DO", "Al", "Fe", "RCI", "Colour", "Temp" 
        };

        int SelectedColumn = 0;
        string SelectedDataSet = null, SavedFilePath = "";

        private void Filter_Load(object sender, EventArgs e)
        {
            DemoTable.Columns.Clear();
            DemoTable.ColumnHeadersDefaultCellStyle.Font = 
                new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            
            if (Properties.Settings.Default.User == "User")
            {
                guna2CircleButton2.Enabled = guna2CircleButton3.Enabled = guna2CircleButton3.Enabled = 
                    btnProgressHistory.Enabled = btnSettings.Enabled = false;
            }

            DictionarySelectColumn = new Dictionary<Guna2CustomCheckBox, Label>
            {
                { ChBIndexNo , label2 },{ ChBDescription , label3 },{ ChBStartDate , label11 },
                { ChBEH , label4 },{ ChBZone , label9 },{ ChBEstMoney , label5 },
                { ChBPermission , label8 },{ ChBMOE , label6 },{ ChBEstName , label13 },
                { ChBProgress , label7 },{ ChBDetails , label10 },{ guna2CustomCheckBox1 , label17 }
            };

            btnFilterSectionChangeProject.Checked = true;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(1280, 720);
        }

        private void CheckBox_CheckedChange(object sender, EventArgs e)
        {
            Guna2CustomCheckBox SelectedCheckBox = (Guna2CustomCheckBox)sender;

            if (SelectedCheckBox.Checked)
            {
                DemoTable.Visible = true;

                if (DictionarySelectColumn[SelectedCheckBox].Text == "Testing Parameters")
                {
                    foreach (string TestingParameter in TestingParametersList) 
                    {
                        DemoTable.Columns.Add(TestingParameter, TestingParameter);
                    }
                }
                else
                {
                    DemoTable.Columns.Add(SelectedCheckBox.Tag.ToString().Replace(" ", ""),
                        SelectedCheckBox.Tag.ToString());
                }
            }
            else
            {
                try
                {
                    DemoTable.Columns.Remove(DemoTable.Columns[SelectedCheckBox.Tag.ToString().Replace(" ", "")]);
                }
                catch { }

                if (SelectedDataSet == "Production" && DictionarySelectColumn[SelectedCheckBox].Text == "Testing Parameters")
                {
                    foreach (string TestingParameter in TestingParametersList)
                    {
                        DemoTable.Columns.Remove(TestingParameter);
                    }
                }
            }
        }

        private void DemoTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedColumn = e.ColumnIndex;
            for (var i = 0; i < DemoTable.ColumnCount; i++)
            {
                DemoTable.Rows[0].Cells[i].Value = "";
            }
            DemoTable.Rows[0].Cells[e.ColumnIndex].Value = "▲";
        }

        private void DemoTable_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Left)
                {
                    DemoTable.Columns[SelectedColumn].DisplayIndex -= 1;
                }
                else if (e.KeyData == Keys.Right)
                {
                    DemoTable.Columns[SelectedColumn].DisplayIndex += 1;
                }
            }
            catch { }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            multiFunctions.SignOut(this);
        }

        private void NavigationBarButton_Click(object sender, EventArgs e)
        {
            Guna2CircleButton NavigationBarButton = (Guna2CircleButton)sender;
            multiFunctions.NavigateTo(NavigationBarButton.Tag.ToString(), this);
        }        

        private void btnFilterSectionChange_CheckedChanged(object sender, EventArgs e)
        {
            btnFilterSectionChangeProject.ShadowDecoration.Enabled = 
                btnFilterSectionProduction.ShadowDecoration.Enabled = 
                btnFilterSectionConsumption.ShadowDecoration.Enabled = false;

            Guna2Button btnFilterSection = (Guna2Button)sender;
            btnFilterSection.ShadowDecoration.Enabled = true;
            SelectedDataSet = btnFilterSection.Tag.ToString();

            if (btnFilterSection.Checked)
            {
                DemoTable.Columns.Clear();
                List<string> SelectedSectionColumnNameList = new List<string> { };

                switch (btnFilterSection.Tag.ToString())
                {
                    case "Project":
                        {
                            SelectedSectionColumnNameList = new List<string>
                            {
                                "Index No","Project Description","Start Date","Expenditure Head",
                                "Section","Estimated Money","Permission","Method Of Execution",
                                "Estimator","Progress","Progress Details"
                            };

                            break;
                        }
                    case "Production":
                        {
                            SelectedSectionColumnNameList = new List<string>
                            {
                                "Plant","Date","Produced Capacity","Production Cost",
                                "Comment","Water Quality","Testing Parameters"
                            };
                            break;
                        }
                    case "Consumption":
                        {
                            SelectedSectionColumnNameList = new List<string>
                            {
                                "Area","Related Month","Monthly Consumption Capacity","Consumption Income",
                                "Monthly Produced Capacity","Monthly Production Cost"
                            };
                            break;
                        }
                }
                
                int IndexCount = 0;

                foreach (KeyValuePair<Guna2CustomCheckBox, Label> SelectColumn in DictionarySelectColumn)
                {
                    if (IndexCount < SelectedSectionColumnNameList.Count) 
                    {
                        SelectColumn.Key.Tag = SelectColumn.Value.Text = 
                            SelectedSectionColumnNameList[IndexCount];

                        SelectColumn.Key.Checked = SelectColumn.Key.Visible = true;
                    }
                    else
                    {
                        SelectColumn.Key.Tag = SelectColumn.Value.Text = "";
                        SelectColumn.Key.Visible = SelectColumn.Key.Checked = false;
                    }                    
                    IndexCount += 1;
                }
            }            
        }

        Dictionary<string, string> FindSimilarColumnHeaderForDatabaseColumnName()
        {
            Dictionary<string, string> SimilarColumnHeaderForDatabaseColumnName = new Dictionary<string, string> { };
            
            switch (SelectedDataSet)
            {
                case "Project":
                    {
                        SimilarColumnHeaderForDatabaseColumnName = new Dictionary<string, string>
                        {
                            { "Index No","ProjectID" },{ "Project Description","Discription" },{ "Estimated Money","EstimatedMoney" },
                            { "Permission","Permission" },{ "Start Date","StartDate" },{ "Method Of Execution","MethodOfExecution" },
                            { "Progress","Progress" },{ "Progress Updated Date","Date" },{ "Progress Details","Details" },
                            { "Estimator","EstName" },{ "Section","ZoneNo" },{ "Expenditure Head","Expnediture Head" }
                        };

                        break;
                    }
                case "Production":
                    {
                        SimilarColumnHeaderForDatabaseColumnName = new Dictionary<string, string>
                        {
                            { "Plant","PlantName" },{ "Date","Date" },{ "Produced Capacity","Capacity" },{ "Production Cost","Cost" },
                            { "Water Quality","Quality" },{ "Comment","Details" },
                            { "PH","PH" },{ "EC","EC" },{ "TDS","TDS" },{ "Tur","Tur" },{ "DO","DO" },
                            { "Al","Al" },{ "Fe","Fe" },{ "RCI","RCI" },{ "Colour","Colour" },{ "Temp","Temp" }
                        };
                        break;
                    }
                case "Consumption":
                    {
                        SimilarColumnHeaderForDatabaseColumnName = new Dictionary<string, string>
                        {
                            { "Related Month","Month" },{ "Monthly Consumption Capacity","Capacity" },
                            { "Consumption Income","MonthAmount" },{ "Area","LocationName" },
                            { "Monthly Produced Capacity","ProductionCapacity" },{ "Monthly Production Cost","Cost" }
                        };
                        break;
                    }
            }

            return SimilarColumnHeaderForDatabaseColumnName;
        }

        int FindColumnNameIndexInDataTable(DataTable SelectedDataTable, string RequiedColumnName)
        {
            for (var i = 0; i < SelectedDataTable.Columns.Count; i++)
            {
                if (SelectedDataTable.Columns[i].ColumnName == RequiedColumnName)
                {
                    return i;
                }
            }
            return -1;
        }

        void CreateResultSheet(string FilterQuery)
        {
            Dictionary<string, string> SimilarColumnHeaderForDatabaseColumnName;
            SortedDictionary<int, string> ColumnOrder = new SortedDictionary<int, string> { };
            string Query = null, SelectedColumn = null, DemoTableColumnName = null;
            int RowNo = 0, ColumnNo = 0;
            DataTable SelectionDataTable;

            if (FilterQuery == null)
            {
                if (SelectedDataSet == "Project")
                    FilterQuery = " WHERE Project.StartDate LIKE '" + DateTime.Now.Year.ToString() + "%'";
                else if (SelectedDataSet == "Production")
                    FilterQuery = " WHERE Production.Date LIKE '" + DateTime.Now.Year.ToString() + "%'";
                else
                    FilterQuery = "";
            }

            switch (SelectedDataSet)
            {
                case "Project":
                    {
                        if (CustomFilters.ToggleSwitchProgressHistory.Checked)
                        {
                            Query = "SELECT Project.ProjectID, Project.Discription, Project.EstimatedMoney, " +
                            "Project.Permission, Project.StartDate, Project.MethodOfExecution, " +
                            "ProgressOfProject.Progress, ProgressOfProject.Date, ProgressOfProject.Details, " +
                            "Estimator.EstName, Estimator.ZoneNo, ExpenditureHead.EH_Main, " +
                            "ExpenditureHead.EH_Sub1, ExpenditureHead.EH_Sub2, ExpenditureHead.EH_Sub3 " +
                            "FROM Project INNER JOIN ProgressOfProject ON Project.ProjectID = ProgressOfProject.ProjectID " +
                            "INNER JOIN Estimator ON Project.EstimatorNO = Estimator.EstimatorNO " +
                            "INNER JOIN ExpenditureHeadProject ON Project.ProjectID = ExpenditureHeadProject.ProjectID " +
                            "INNER JOIN ExpenditureHead ON ExpenditureHeadProject.EHID = ExpenditureHead.EHID" + FilterQuery;
                        }
                        else
                        {
                            Query = "SELECT Project.ProjectID, Project.Discription, Project.EstimatedMoney, Project.Permission, " +
                                "Project.StartDate, Project.MethodOfExecution, MAX(ProgressOfProject.Progress) AS Progress, " +
                                "Estimator.EstName, Estimator.ZoneNo, ExpenditureHead.EH_Main, ExpenditureHead.EH_Sub1, " +
                                "ExpenditureHead.EH_Sub2, ExpenditureHead.EH_Sub3 " +
                                "FROM Project INNER JOIN ProgressOfProject ON Project.ProjectID = ProgressOfProject.ProjectID " +
                                "INNER JOIN Estimator ON Project.EstimatorNO = Estimator.EstimatorNO " +
                                "INNER JOIN ExpenditureHeadProject ON Project.ProjectID = ExpenditureHeadProject.ProjectID " +
                                "INNER JOIN ExpenditureHead ON ExpenditureHeadProject.EHID = ExpenditureHead.EHID " + FilterQuery +
                                "GROUP BY Project.ProjectID, Project.Discription, Project.EstimatedMoney, Project.Permission, " +
                                "Project.StartDate, Project.MethodOfExecution, Estimator.EstName, Estimator.ZoneNo, " +
                                "ExpenditureHead.EH_Main, ExpenditureHead.EH_Sub1, ExpenditureHead.EH_Sub2, ExpenditureHead.EH_Sub3";
                        }                        

                        break;
                    }
                case "Production":
                    {
                        Query = "SELECT Plant.PlantName, Production.Date, Production.Capacity, Production.Cost, " +
                            "Production.Details, Production.PH, Production.EC, Production.TDS, Production.Tur, " +
                            "Production.DO, Production.Al, Production.Fe, Production.RCI, Production.Colour, " +
                            "Production.Temp FROM Production " +
                            "INNER JOIN Plant ON Production.PlantID = Plant.PlantID" + FilterQuery;

                        break;
                    }
                case "Consumption":
                    {
                        Query = "SELECT Consumption.Month, Consumption.Capacity, Consumption.MonthAmount, Area.LocationName " +
                            "FROM Consumption INNER JOIN Area ON Consumption.AreaID = Area.AreaID" + FilterQuery;

                        break;
                    }
            }

            SelectionDataTable = sqlFunctions.RetrieveDataOnDataTable(Query);
            if (SelectionDataTable != null)
            {
                if (SelectedDataSet == "Project")
                {
                    int EHStartColumnIndex;
                    if (CustomFilters.ToggleSwitchProgressHistory.Checked)
                        EHStartColumnIndex = 11;
                    else
                        EHStartColumnIndex = 9;

                    // Combine Expenditure Head Columns
                    foreach (DataRow ItemRow in SelectionDataTable.Rows)
                    {
                        if (ItemRow[EHStartColumnIndex + 3].ToString().Trim() != "")
                        {
                            ItemRow[EHStartColumnIndex] = ItemRow[EHStartColumnIndex] + "-" + ItemRow[EHStartColumnIndex + 1] + "-" + ItemRow[EHStartColumnIndex + 2] + "-" + ItemRow[EHStartColumnIndex + 3];
                        }
                        else
                        {
                            ItemRow[EHStartColumnIndex] = ItemRow[EHStartColumnIndex] + "-" + ItemRow[EHStartColumnIndex + 1] + "-" + ItemRow[EHStartColumnIndex + 2];
                        }
                    }
                    SelectionDataTable.Columns[EHStartColumnIndex].ColumnName = "Expnediture Head";

                    // Delete Extra Expenditure Head Columns form Data Table 
                    for (var i = 0; i < 3; i++)
                    {
                        SelectionDataTable.Columns.RemoveAt(EHStartColumnIndex + 1);
                    }                    

                    if (!CustomFilters.ToggleSwitchProgressHistory.Checked)
                    {
                        List<string> ReturnData;

                        SelectionDataTable.Columns.Add("Details");

                        foreach (DataRow SelectionDataTableDataRow in SelectionDataTable.Rows)
                        {
                            ReturnData = sqlFunctions.SQLRead(String.Format("SELECT Details FROM ProgressOFProject " +
                                "WHERE ProjectID='{0}' AND Progress ='{1}'", SelectionDataTableDataRow.ItemArray[0], 
                                SelectionDataTableDataRow.ItemArray[6]), "Details");

                            if (ReturnData != null)
                            {
                                SelectionDataTableDataRow["Details"] = ReturnData[0];
                            }
                            else
                            {
                                SelectionDataTableDataRow["Details"] = "";
                            }
                        }
                    }
                }

                // Fins Column Header Names in Database
                SimilarColumnHeaderForDatabaseColumnName = FindSimilarColumnHeaderForDatabaseColumnName();

                foreach (DataGridViewColumn DemoTableColumnNameAfterChangedPlace in DemoTable.Columns)
                {
                    ColumnOrder.Add(DemoTableColumnNameAfterChangedPlace.DisplayIndex, DemoTableColumnNameAfterChangedPlace.HeaderText);
                }

                // Show Data On Results Table                
                TableResults.Columns.Clear();
                for (var j = 0; j < DemoTable.Columns.Count; j++)
                {
                    DemoTableColumnName = ColumnOrder[j];
                    SelectedColumn = SimilarColumnHeaderForDatabaseColumnName[DemoTableColumnName];
                    ColumnNo = FindColumnNameIndexInDataTable(SelectionDataTable, SelectedColumn);

                    if (ColumnNo != -1)
                    {
                        TableResults.Columns.Add(SelectedColumn, DemoTableColumnName);

                        if (SelectedDataSet == "Production")
                        {
                            foreach (string TestingParameterColumnName in TestingParametersList)
                            {
                                if (SelectedColumn == TestingParameterColumnName)
                                {
                                    TableResults.Columns[SelectedColumn].Width = 75;
                                    break;
                                }
                            }
                        }

                        ShowDataTableDataOnDataGridView();
                    }
                    else
                    {
                        if (SelectedColumn == "Quality")
                        {
                            TableResults.Columns.Add(SelectedColumn, DemoTableColumnName);
                            TableResults.Columns[SelectedColumn].Width = 110;

                            Dictionary<string, int> TestingParameterColumnIndex = new Dictionary<string, int> { };

                            foreach (string TestingParameter in TestingParametersList)
                            {
                                TestingParameterColumnIndex.Add(TestingParameter,
                                    FindColumnNameIndexInDataTable(SelectionDataTable, TestingParameter));
                            }

                            SelectionDataTable.Columns.Add(SelectedColumn);
                            ColumnNo = FindColumnNameIndexInDataTable(SelectionDataTable, SelectedColumn);
                            string Value;

                            foreach (DataRow ParameterRow in SelectionDataTable.Rows)
                            {
                                ParameterRow["Quality"] = "0";

                                foreach (string TestingParameter in TestingParametersList)
                                {
                                    Value = ParameterRow.ItemArray[TestingParameterColumnIndex[TestingParameter]].ToString();
                                    if (Value.Trim() != "")
                                    {
                                        if (multiFunctions.CheckRange(TestingParameter,
                                        float.Parse(Value)) == "YES")
                                        {
                                            ParameterRow["Quality"] = (Convert.ToInt32(ParameterRow["Quality"].ToString()) + 10).ToString();
                                        }
                                    }
                                }

                                ParameterRow["Quality"] += "%";
                            }

                            ShowDataTableDataOnDataGridView();
                        }
                        else if (SelectedColumn == "ProductionCapacity" || SelectedColumn == "Cost") 
                        {
                            TableResults.Columns.Add(SelectedColumn, DemoTableColumnName);

                            string CurrentRelatedMonth = "", MonthlyTotalSelectedField = "", SelectedProductionTableColumn;

                            if (SelectedColumn == "ProductionCapacity")
                                SelectedProductionTableColumn = "Capacity";
                            else
                                SelectedProductionTableColumn = "Cost";

                            foreach (DataRow SelectionDataTableRow in SelectionDataTable.Rows)
                            {
                                if (CurrentRelatedMonth != SelectionDataTableRow["Month"].ToString()) 
                                {
                                    CurrentRelatedMonth = SelectionDataTableRow["Month"].ToString();

                                    List<string> SelectedColumnDataList =
                                       sqlFunctions.SQLRead("SELECT SUM(" + SelectedProductionTableColumn + ") FROM Production " +
                                       "WHERE Date LIKE '" + CurrentRelatedMonth + "%'", SelectedProductionTableColumn);

                                    MonthlyTotalSelectedField = "N/A";
                                    if (SelectedColumnDataList != null)
                                    {
                                        if (SelectedColumnDataList[0] != "")
                                        {
                                            MonthlyTotalSelectedField = SelectedColumnDataList[0];
                                        }
                                    }

                                    SelectionDataTable.Columns.Add(SelectedColumn);
                                    ColumnNo = FindColumnNameIndexInDataTable(SelectionDataTable, SelectedColumn);
                                }

                                SelectionDataTableRow[SelectedColumn] = MonthlyTotalSelectedField.ToString();
                            }                            

                            ShowDataTableDataOnDataGridView();
                        }
                    }
                }

                void ShowDataTableDataOnDataGridView()
                {
                    RowNo = -1;
                    for (var i = 0; i < SelectionDataTable.Rows.Count; i++)
                    {
                        if (TableResults.Columns.Count == 1)
                            RowNo = TableResults.Rows.Add();
                        else
                            RowNo += 1;

                        TableResults.Rows[RowNo].Cells[SelectedColumn].Value =
                            SelectionDataTable.Rows[RowNo].ItemArray[ColumnNo].ToString();
                    }
                }
            }
            else
            {
                TableResults.Rows.Clear();
                MessageBox.Show("Something Went Wrong!. Can't Find Results", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowResults_Click(object sender, EventArgs e)
        {
            CreateResultSheet(null);
            CustomFilters = new FilterCustomization();
            pagedControl1.SelectedPage = page2;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            CustomFilters.Close();
            pagedControl1.SelectedPage = page1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        void ExportData()
        {
            if (TableResults.Rows.Count > 0)
            {
                // delete coma from table
                for (var i = 0; i < TableResults.RowCount - 1; i++)
                {
                    for (int j = 0; j < TableResults.Columns.Count; j++)
                    {
                        TableResults.Rows[i].Cells[j].Value = 
                            TableResults.Rows[i].Cells[j].Value.ToString().Replace(',', ' ');
                    }
                }

                // create save file
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";

                if (SelectedDataSet == "Production")
                    sfd.FileName = "Water Production Data, Exported On " + DateTime.Now.Date.ToString("yyyy-MM-dd") + " .csv";
                else if (SelectedDataSet == "Consumption")
                    sfd.FileName = "Water Consumption Data, Exported On " + DateTime.Now.Date.ToString("yyyy-MM-dd") + " .csv";
                else
                    sfd.FileName = "Result " + DateTime.Now.Date.ToString("yyyy-MM-dd") + " .csv";

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
                            MessageBox.Show("It wasn't possible to write the data to the disk." + 
                                ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            SavedFilePath = sfd.FileName;

                            // Open Exported file
                            if (MessageBox.Show("Data Exported Successfully! Do You Want To Open It?", 
                                "Information", MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                try
                                {                                    
                                    Process.Start(SavedFilePath);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Can't Open Saved File" + ex.Message,
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export!, Table Is Empty", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            CustomFilters.SelectedDataSet = SelectedDataSet;

            if (SelectedDataSet == "Production")
                CustomFilters.Height = 423;
            else if (SelectedDataSet == "Consumption")
                CustomFilters.Height = 400;
            else
                CustomFilters.Height = 547;

            CustomFilters.Width = 585;
            CustomFilters.ShowDialog();

            if (CustomFilters.ApplyFilters)
            {
                string ConditionPartOfQuery = " WHERE ", ConditionField = "";
                List<string> ConditionParts = new List<string> { };

                if (SelectedDataSet == "Project")
                {                    
                    if (!CustomFilters.ToggleSwitchSectionAll.Checked && CustomFilters.SelectedSection.Count > 0)
                    {
                        ConditionField = "(";

                        foreach (string Item in CustomFilters.SelectedSection)
                        {
                            ConditionField += String.Format("Estimator.ZoneNo='{0}' OR ", Item);
                        }

                        ConditionField = ConditionField.Remove(ConditionField.Length - 4);
                        ConditionField += ")";

                        ConditionParts.Add(ConditionField);
                    }

                    if (CustomFilters.SelectedPermission.Count > 0)
                    {
                        ConditionField = "(";

                        foreach (string Item in CustomFilters.SelectedPermission)
                        {
                            ConditionField += String.Format("Project.Permission='{0}' OR ", Item);
                        }

                        ConditionField = ConditionField.Remove(ConditionField.Length - 4);
                        ConditionField += ")";

                        ConditionParts.Add(ConditionField);
                    }

                    if (CustomFilters.SelectedMOE.Count > 0)
                    {
                        ConditionField = "(";

                        foreach (string Item in CustomFilters.SelectedMOE)
                        {
                            ConditionField += String.Format("Project.MethodOfExecution='{0}' OR ", Item);
                        }

                        ConditionField = ConditionField.Remove(ConditionField.Length - 4);
                        ConditionField += ")";

                        ConditionParts.Add(ConditionField);
                    }

                    if (CustomFilters.SelectedProgressRange.Count > 0)
                    {
                        int UpRange = 0, DownRange = 100;

                        foreach (string Item in CustomFilters.SelectedProgressRange)
                        {
                            if (Item == "Under Work")
                                ChangeRange(99, 1);
                            else if (Item == "Not Started")
                                ChangeRange(UpRange, 0);
                            else
                                ChangeRange(100, DownRange);
                        }

                        void ChangeRange(int SelectedItemUpRange,int SelectedItemDownRange)
                        {
                            if (UpRange < SelectedItemUpRange)
                                UpRange = SelectedItemUpRange;

                            if (DownRange > SelectedItemDownRange)
                                DownRange = SelectedItemDownRange;
                        }

                        ConditionField = String.Format("(ProgressOfProject.Progress<={0} AND ProgressOfProject.Progress>={1})", UpRange, DownRange);
                        ConditionParts.Add(ConditionField);
                    }   
                }
                else if (SelectedDataSet == "Production")
                {
                    if (CustomFilters.SelectedPlant.Count > 0)
                    {
                        ConditionField = "(";

                        foreach (string Item in CustomFilters.SelectedPlant)
                        {
                            ConditionField += String.Format("Plant.PlantName='{0}' OR ", Item);
                        }

                        ConditionField = ConditionField.Remove(ConditionField.Length - 4);
                        ConditionField += ")";

                        ConditionParts.Add(ConditionField);
                    }
                }
                else
                {
                    if (CustomFilters.SelectedArea != "All")
                        ConditionParts.Add(String.Format("(Area.LocationName='{0}')", CustomFilters.SelectedArea));
                    
                    if (CustomFilters.SelectedYear != "All" && CustomFilters.SelectedMonth != "All")
                    {
                        ConditionParts.Add(String.Format("(Consumption.Month='{0}-{1}')",
                            CustomFilters.SelectedYear, CustomFilters.SelectedMonth));
                    }
                    else if (CustomFilters.SelectedYear != "All")
                    {
                        ConditionParts.Add(String.Format("(Consumption.Month LIKE '{0}-%')",
                            CustomFilters.SelectedYear));
                    }
                    else if (CustomFilters.SelectedMonth != "All")
                    {
                        ConditionParts.Add(String.Format("(Consumption.Month LIKE '%-{0}')",
                            CustomFilters.SelectedMonth));
                    }
                }

                if (ConditionParts.Count > 0)
                {
                    foreach (string item in ConditionParts)
                    {
                        ConditionPartOfQuery += item + " AND ";
                    }

                    ConditionPartOfQuery = ConditionPartOfQuery.Remove(ConditionPartOfQuery.Length - 4);
                    CreateResultSheet(ConditionPartOfQuery);
                }
                else
                {
                    CreateResultSheet(null);
                }

                // Filter by Date (StartDate Or ProductionDate)

                if (SelectedDataSet == "Project")
                {
                    if (!CustomFilters.ToggleSwitchStartDateAll.Checked)
                    {
                        if (CustomFilters.SelectedMinimumStartDate < CustomFilters.SelectedMaximumStartDate)
                        {/*
                            DateTimePicker DateTimeCheck = new DateTimePicker();
                            string[] StartDate;

                            foreach(DataGridViewRow TableResultsRow in TableResults.Rows)
                            {
                                StartDate = TableResultsRow.Cells["StartDate"].Value.ToString().Split('-');
                                DateTimeCheck.Value. = Convert.ToInt32(StartDate[0]);

                            }*/

                        }
                        else
                        {
                            MessageBox.Show("Something went wrong! Can't Filter Start Date", "Input Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (SelectedDataSet == "Production")
                {

                }
            }
        }

        private void TableResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (TableResults.Columns[e.ColumnIndex].Name == "ProjectID")
            {

            }
        }
    }
}
