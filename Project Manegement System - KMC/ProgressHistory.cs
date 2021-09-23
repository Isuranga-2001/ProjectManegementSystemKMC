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
using Guna.UI2.WinForms;

namespace Project_Manegement_System___KMC
{
    public partial class ProgressHistory : Form
    {
        public ProgressHistory()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        public Guna2DataGridView DemoTable = new Guna2DataGridView();
        public List<Guna2CustomCheckBox> ActiveCheckBoxList = new List<Guna2CustomCheckBox>() { };
        string SavedFilePath = null;

        private void ProgressHistory_Load(object sender, EventArgs e)
        {
            UpdateDatGridView("SELECT * FROM ProgressOfProject");
            if (txtProjectID.Text != "")
            {
                FilterProgressHistory();
            }
            SwitchProgressDate.Checked = false;

            DemoTable.Columns.Add("ProjectID", "Index No");
            PageSet.SelectedPage = page1;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (txtProjectID.Text != "")
            {
                FilterProgressHistory();
            }

            if (SwitchProgressDate.Checked && DataGridViewProgressHistory.Rows.Count > 0)
            {
                List<string> IndexToRemove = new List<string> { };
                for (int i = 0; i < DataGridViewProgressHistory.Rows.Count - 1; i++)
                {
                    if (DataGridViewProgressHistory.Rows[i].Cells[2].Value.ToString() != txtDate.Text)
                    {
                        IndexToRemove.Add(DataGridViewProgressHistory.Rows[i].Cells[0].Value.ToString());
                    }
                }
                int NoOfRows = DataGridViewProgressHistory.RowCount - 1;
                for (var i = 0; i < NoOfRows; i++)
                {
                    if (DataGridViewProgressHistory.Rows[i].Cells[2].Value.ToString() != txtDate.Text)
                    {
                        DataGridViewProgressHistory.Rows.Remove(DataGridViewProgressHistory.Rows[i]);
                        i -= 1;
                        NoOfRows -= 1;
                        continue;
                    }
                }
            }
            SwitchProgressDate.Checked = false;
        }

        void UpdateDatGridView(string query)
        {
            if (!(sqlFunctions.RetrieveDataTable(query, DataGridViewProgressHistory)))
            {
                sqlFunctions.ErrorMessage();
            }
        }

        void FilterProgressHistory()
        {
            if (DataGridViewProgressHistory.Columns.Count > 3)
            {
                DataGridViewProgressHistory.Columns[0].Visible = false;
            }
            UpdateDatGridView("SELECT * FROM ProgressOfProject WHERE ProjectID='" + txtProjectID.Text + "'");
            FindDescriptionOfProject(txtProjectID.Text);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtProjectID.Clear();
            txtDescription.Clear();
            DataGridViewProgressHistory.Columns[0].Visible = true;
            UpdateDatGridView("SELECT * FROM ProgressOfProject");
            SwitchProgressDate.Checked = false;
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
            SwitchProgressDate.Checked = false;
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

        private void SwitchProgressDate_CheckedChanged(object sender, EventArgs e)
        {
            if (SwitchProgressDate.Checked)
                txtDate.Enabled = true;
            else
                txtDate.Enabled = false;
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
        {
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
                    if (DataGridViewProgressHistory.Rows[i].Cells[DataGridViewProgressHistory.ColumnCount - 2].Value.ToString() != txtDate.Text)
                    {
                        IsFilter = false;
                        break;
                    }
                }
                if (IsFilter)
                {
                    if (FilterForm.ProgressHistoryFilters == null) 
                        FilterForm.ProgressHistoryFilters = "[Date] ='" + txtDate.Text + "' ";
                    else
                        FilterForm.ProgressHistoryFilters += "AND [Date] ='" + txtDate.Text + "' ";
                }
                FilterForm.ShowDialog();
            }
        }

        private void btnShowProjectDetails_Click(object sender, EventArgs e)
        {
            if (PageSet.SelectedPage == page1)
            {
                ChBZone.Checked = ChBPermission.Checked = ChBMOE.Checked = ChBEH.Checked = ChBDescription.Checked = true;
                PageSet.SelectedPage = page2;
                btnExport.Enabled = btnOpen.Enabled = btnShowResults.Enabled = false;
            }
            else
            {
                PageSet.SelectedPage = page1;
                btnExport.Enabled = btnOpen.Enabled = btnShowProjectDetails.Enabled = btnShowResults.Enabled = true;
                btnShowProjectDetails.Checked = false;
                ChBPerson.Checked = ChBEstName.Checked = ChBStartDate.Checked = ChBEstMoney.Checked = ChBZone.Checked = ChBPermission.Checked = ChBMOE.Checked = ChBEH.Checked = ChBDescription.Checked = false;
            }
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
        {
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
                        ResultSheet.ProgressHistoryProjectIDList.Add(DataGridViewProgressHistory.Rows[i].Cells[0].Value.ToString());
                        ResultSheet.ProgressHistoryProgressList.Add(DataGridViewProgressHistory.Rows[i].Cells[1].Value.ToString());
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
                ChBPerson.Checked = ChBEstName.Checked = ChBStartDate.Checked = ChBEstMoney.Checked = ChBZone.Checked = ChBPermission.Checked = ChBMOE.Checked = ChBEH.Checked = ChBDescription.Checked = false;
            }
        }
    }
}
