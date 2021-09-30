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
using System.Windows.Forms.DataVisualization.Charting;
using Guna.UI2.WinForms;

namespace Project_Manegement_System_KMC_Water
{
    public partial class ProgressHistory : Form
    {
        public ProgressHistory()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        MultiFunctions multiFunctions = new MultiFunctions();
        public Guna2DataGridView DemoTable = new Guna2DataGridView();

        public List<Guna2CustomCheckBox> ActiveCheckBoxList = new List<Guna2CustomCheckBox>() { };
        string SavedFilePath = null;
        int SelectedRow = 0;

        private void ProgressHistory_Load(object sender, EventArgs e)
        {
            UpdateDataGridViewFromQuary("SELECT * FROM ProgressOfProject WHERE Date LIKE '" + DateTime.Now.Year.ToString() + "%'");

            if (txtProjectID.Text != "")
            {
                try
                {
                    FilterProgressHistory();
                }
                catch
                {
                    MessageBox.Show("Can't find results for Index No.", "Incorrect Index No",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtProjectID.Text = "";
                }
            }

            DemoTable.Columns.Add("ProjectID", "Index No");
            PageSet.SelectedPage = page1;


            ChartProgressHistory.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            ChartProgressHistory.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            UpDownDateYear.Value = DateTime.Now.Year;
            UpDownDateMonth.Value = DateTime.Now.Month;
            UpDownDateDay.Value = DateTime.Now.Day;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(1280, 720);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            DataGridViewProgressHistory.Columns[0].Visible = true;
            UpdateDataGridViewFromQuary("SELECT * FROM ProgressOfProject WHERE Date='" + CurrentDate() + "'");
        }

        string CurrentDate()
        {
            return UpDownDateYear.Value.ToString() + "-" + UpDownDateMonth.Value.ToString() + "-" + UpDownDateDay.Value.ToString();
        }

        void UpdateDataGridViewFromQuary(string query)
        {
            if (!(sqlFunctions.RetrieveDataOnDataGridView(query, DataGridViewProgressHistory)))
            {
                MessageBox.Show("Can't find projects for chooesed date", "Input Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateChart();
        }

        void FilterProgressHistory()
        {
            if (DataGridViewProgressHistory.Columns.Count > 3)
            {
                DataGridViewProgressHistory.Columns[0].Visible = false;
            }
            UpdateDataGridViewFromQuary("SELECT * FROM ProgressOfProject WHERE ProjectID='" + txtProjectID.Text + "'");
            FindDescriptionOfProject(txtProjectID.Text);
        }

        void UpdateChart()
        {
            for (var i = 0; i < DataGridViewProgressHistory.RowCount - 1; i++)
            {
                DataGridViewProgressHistory.Rows[i].Cells[1].Value = Convert.ToInt32(DataGridViewProgressHistory.Rows[i].Cells[1].Value);
            }

            ChartProgressHistory.Series.Clear();
            if (DataGridViewProgressHistory.Columns[0].Visible)
            {
                DataGridViewProgressHistory.Sort(DataGridViewProgressHistory.Columns[0], ListSortDirection.Ascending);
                string ProjectID = "";
                for (var i = 0; i < DataGridViewProgressHistory.RowCount - 1; i++) 
                {
                    if (ProjectID != DataGridViewProgressHistory.Rows[i].Cells[0].Value.ToString().Trim())
                    {
                        ProjectID = DataGridViewProgressHistory.Rows[i].Cells[0].Value.ToString().Trim();
                        ChartProgressHistory.Series.Add(ProjectID);
                        ChartProgressHistory.Series[ProjectID].ToolTip = ProjectID;
                        ChartProgressHistory.Series[ProjectID].ChartType = SeriesChartType.Line;
                        ChartProgressHistory.Series[ProjectID].Points.AddXY(0, 0);
                    }
                    ChartProgressHistory.Series[ProjectID].Points.AddXY(
                        DataGridViewProgressHistory.Rows[i].Cells[2].Value.ToString().Trim(),
                        DataGridViewProgressHistory.Rows[i].Cells[1].Value.ToString().Trim());
                }
            }
            else
            {
                DataGridViewProgressHistory.Sort(DataGridViewProgressHistory.Columns[1], ListSortDirection.Ascending);
                ChartProgressHistory.Series.Add(txtProjectID.Text);
                ChartProgressHistory.Series[txtProjectID.Text].ToolTip = txtProjectID.Text;
                ChartProgressHistory.Series[txtProjectID.Text].ChartType = SeriesChartType.Area;
                ChartProgressHistory.PaletteCustomColors[0] = Color.FromArgb(240, 0, 110, 179);
                
                for (var i = 0; i < DataGridViewProgressHistory.RowCount - 1; i++)
                {                    
                    ChartProgressHistory.Series[txtProjectID.Text].Points.AddXY(
                        DataGridViewProgressHistory.Rows[i].Cells[2].Value.ToString().Trim(),
                        DataGridViewProgressHistory.Rows[i].Cells[1].Value.ToString().Trim());
                }
            }
        }

        private void DataGridViewProgressHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                FindDescriptionOfProject(DataGridViewProgressHistory.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
        }

        void FindDescriptionOfProject(string ProjectID)
        {
            txtDescription.Text = sqlFunctions.SQLRead("SELECT Discription FROM dbo.Project WHERE (ProjectID='"
                    + ProjectID + "')", "Discription")[0];
            txtProjectID.Text = ProjectID;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectProject form = new SelectProject();
            form.AddNewEnabled = false;
            form.ShowDialog();
            if (!(form.AddNew) && form.SelectedProjectID != null) 
            {
                txtProjectID.Text = form.SelectedProjectID;
                FilterProgressHistory();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Export();
        }

        void Export()
        {
            if (DataGridViewProgressHistory.Rows.Count > 0) 
            {
                for (var i = 0; i < DataGridViewProgressHistory.RowCount - 1; i++)
                {
                    DataGridViewProgressHistory.Rows[i].Cells[2].Value = DataGridViewProgressHistory.Rows[i].Cells[2].Value.ToString().Replace(',', ' ');
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Progress History " + txtProjectID.Text + " " + DateTime.Now.Date.ToString("yyyy-MM-dd") + " .csv";
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
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = DataGridViewProgressHistory.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[DataGridViewProgressHistory.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += DataGridViewProgressHistory.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < DataGridViewProgressHistory.Rows.Count - 1; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += DataGridViewProgressHistory.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !", "Info");
                            SavedFilePath = sfd.FileName;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void txtProjectID_TextChanged(object sender, EventArgs e)
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

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (txtDescription.Text.Trim() == "" && txtProjectID.Text.Trim() == "") 
                BtnUpdateProgress.Enabled = false;
            else
                BtnUpdateProgress.Enabled = true;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (SavedFilePath == null)
                {
                    Export();
                }
                Process.Start(SavedFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't Open Saved File" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SavedFilePath = null;
        }

        private void btnShowResults_Click(object sender, EventArgs e)
        {/*
            EasyMode FilterForm = new EasyMode();
            FilterForm.btnAll.Checked = true;
            FilterForm.ParentFormName = "ProgressHistory";
            if (DataGridViewProgressHistory.Rows.Count - 1 <= 0)
            {
                MessageBox.Show("Need more than one rows to show results", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (DataGridViewProgressHistory.Columns.Count < 4)
            {
                FilterForm.ProgressHistoryFilters = "[ProjectID] ='" + txtProjectID.Text + "' ";
                DateIsFilter();
            }
            else
            {
                DateIsFilter();
            }
            
            void DateIsFilter()
            {
                bool IsFilter = true;
                for (int i = 0; i < DataGridViewProgressHistory.Rows.Count - 1; i++)
                {
                    if (DataGridViewProgressHistory.Rows[i].Cells[DataGridViewProgressHistory.ColumnCount - 2].Value.ToString() != UpDownDateYear.Value.ToString() + "-" + UpDownDateMonth.Value.ToString() + "-" + UpDownDateDay.Value.ToString())
                    {
                        IsFilter = false;
                        break;
                    }
                }
                if (IsFilter)
                {
                    if (FilterForm.ProgressHistoryFilters == null) 
                        FilterForm.ProgressHistoryFilters = "[Date] ='" + UpDownDateYear.Value.ToString() + "-" + UpDownDateMonth.Value.ToString() + "-" + UpDownDateDay.Value.ToString() + "' ";
                    else
                        FilterForm.ProgressHistoryFilters += "AND [Date] ='" + UpDownDateYear.Value.ToString() + "-" + UpDownDateMonth.Value.ToString() + "-" + UpDownDateDay.Value.ToString() + "' ";
                }
                FilterForm.ShowDialog();
            }*/
        }

        private void btnShowProjectDetails_Click(object sender, EventArgs e)
        {
            ChBZone.Checked = ChBPermission.Checked = ChBMOE.Checked = ChBEH.Checked = ChBDescription.Checked = true;
            PageSet.SelectedPage = page2;
            btnExport.Enabled = btnOpen.Enabled = btnShowResults.Enabled = false;
        }

        private void SelectColumnCheckBox_CheckedChange(object sender, EventArgs e)
        {
            Guna2CustomCheckBox ColumnCheckBox = (Guna2CustomCheckBox)sender;
            string[] ColumnNames = ColumnCheckBox.Tag.ToString().Split('~');
            if (ColumnCheckBox.Checked)
            {
                DemoTable.Columns.Add(ColumnNames[1], ColumnNames[0]);

                List<Guna2CustomCheckBox> ListToRemove = new List<Guna2CustomCheckBox> { };
                foreach (var item in ActiveCheckBoxList)
                {
                    if (item == ColumnCheckBox)
                    {
                        ListToRemove.Add(ColumnCheckBox);
                    }
                }
                foreach (var item in ListToRemove)
                {
                    ActiveCheckBoxList.Remove(item);
                }
                ActiveCheckBoxList.Add(ColumnCheckBox);
            }
            else
            {
                try
                {
                    DemoTable.Columns.Remove(DemoTable.Columns[ColumnNames[1]]);
                    ActiveCheckBoxList.Remove(ColumnCheckBox);
                }
                catch
                {
                    DemoTable.Visible = false;
                }
            }
        }

        private void btnSubFormExport_Click(object sender, EventArgs e)
        {
            ShowAllDetails("Export");
        }

        private void btnSubFormOpen_Click(object sender, EventArgs e)
        {
            ShowAllDetails("Open");
        }

        private void btnSubFormShowResult_Click(object sender, EventArgs e)
        {
            ShowAllDetails("All");
        }

        void ShowAllDetails(string ResultsType)
        {/*
            if (DemoTable.Columns.Count > 0 && DataGridViewProgressHistory.Rows.Count - 1 > 0) 
            {
                DemoTable.Columns.Add("Progress", "Progress");
                DemoTable.Columns.Add("Details", "Progress Details");
                DemoTable.Columns.Add("ProgressUpdatedDate", "Progress Updated Date");
                FilteredProjectDetails ResultSheet = new FilteredProjectDetails();
                int Index = 0;
                for (var i = 0; i < DemoTable.ColumnCount; i++)
                {
                    for (var j = 0; j < DemoTable.ColumnCount; j++)
                    {
                        if (DemoTable.Columns[j].DisplayIndex == Index)
                        {
                            ResultSheet.ActiveColumnList.Add(DemoTable.Columns[j].Name);
                            ResultSheet.ActiveColumnHeaderList.Add(DemoTable.Columns[j].HeaderText);
                            Index += 1;
                            break;
                        }
                    }
                }
                ResultSheet.ParentFormName = "ProgressHistoryShowAll";
                ResultSheet.SortBy = null;

                if (DataGridViewProgressHistory.Columns.Count > 3)
                {
                    for (var i = 0; i < DataGridViewProgressHistory.Rows.Count - 1; i++)
                    {
                        ResultSheet.ProgressHistoryProjectIDList.Add(
                            DataGridViewProgressHistory.Rows[i].Cells[0].Value.ToString());
                        ResultSheet.ProgressHistoryProgressList.Add(
                            DataGridViewProgressHistory.Rows[i].Cells[1].Value.ToString());
                    }
                }
                else if (DataGridViewProgressHistory.Columns.Count == 3)
                {
                    for (var i = 0; i < DataGridViewProgressHistory.Rows.Count - 1; i++)
                    {
                        ResultSheet.ProgressHistoryProjectIDList.Add(txtProjectID.Text);
                    }
                }
                ResultSheet.ShowResultsType = ResultsType;
                ResultSheet.ShowDialog();
                DemoTable.Rows.Clear();
                DemoTable.Columns.Clear();
                DemoTable.Columns.Add("ProjectID", "Index No");

               ChBEstName.Checked = ChBStartDate.Checked = ChBEstMoney.Checked = ChBZone.Checked = 
                    ChBPermission.Checked = ChBMOE.Checked = ChBEH.Checked = ChBDescription.Checked = false;
            }*/
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtProjectID.Text != "")
            {
                FilterProgressHistory();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            PageSet.SelectedPage = page1;
            btnExport.Enabled = btnOpen.Enabled = btnShowProjectDetails.Enabled = btnShowResults.Enabled = true;
            btnShowProjectDetails.Checked = false;
            ChBEstName.Checked = ChBStartDate.Checked = ChBEstMoney.Checked = ChBZone.Checked = 
                ChBPermission.Checked = ChBMOE.Checked = ChBEH.Checked = ChBDescription.Checked = false;
        }

        private void BtnUpdateProgress_Click(object sender, EventArgs e)
        {
            UpdateProgress form = new UpdateProgress();
            form.txtProjectID.Text = txtProjectID.Text;
            form.btnHistory.Visible = false;
            form.ShowDialog();
            FilterProgressHistory();
        }

        private void UpDownDateMonth_ValueChanged(object sender, EventArgs e)
        {
            UpDownDateDay.Maximum = multiFunctions.NoOfDays(Convert.ToInt32(UpDownDateMonth.Value));
        }

        private void DataGridViewProgressHistory_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MenuStripShowStatues.Items[1].Visible = true;
                if (!DataGridViewProgressHistory.Columns[0].Visible)
                    MenuStripShowStatues.Items[1].Visible = false;

                MenuStripShowStatues.Visible = true;
                MenuStripShowStatues.Left = MousePosition.X;
                MenuStripShowStatues.Top = MousePosition.Y;
                SelectedRow = e.RowIndex;
            }
        }

        private void MenuStripShowStatues_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridViewProgressHistory.Columns[0].Visible = true;
            if (e.ClickedItem.Text == "Show All")
            {
                txtProjectID.Clear();
                txtDescription.Clear();
                UpdateDataGridViewFromQuary("SELECT * FROM ProgressOfProject");
            }
            else if (e.ClickedItem.Text == "Show Selected Project")
            {
                if (SelectedRow >= 0)
                {
                    FindDescriptionOfProject(DataGridViewProgressHistory.Rows[SelectedRow].Cells[0].Value.ToString());
                    FilterProgressHistory();
                }
            }
            else
            {
                UpdateDataGridViewFromQuary("SELECT * FROM ProgressOfProject " +
                    "WHERE Date LIKE'" + DateTime.Now.Year.ToString() + "-%'");
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
