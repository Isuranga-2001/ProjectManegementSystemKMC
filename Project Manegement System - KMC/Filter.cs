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
    public partial class Filter : Form
    {
        public Filter()
        {
            InitializeComponent();
        }

        int SelectedColumn = 0;
        SQLFunctions sqlFunctions = new SQLFunctions();

        private void Filter_Load(object sender, EventArgs e)
        {
            DemoTable.Visible = false;
            PanelBachDemoTable.Height = 60;
            DemoTable.Columns.Clear();
            DemoTable.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Removebtn.Enabled = Deletebtn.Enabled = Editbtn.Enabled = false;
            if (Properties.Settings.Default.User == "User")
            {
                guna2ImageButton2.Enabled = guna2ImageButton3.Enabled = btnEditData.Enabled = btnProgressHistory.Enabled = btnSettings.Enabled = false;
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            DashBoard form = new DashBoard();
            form.Show();
            this.Close();
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            AddNew form = new AddNew();
            form.Show();
            this.Hide();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            Progress form = new Progress();
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

        private void BtnShowResults_Click(object sender, EventArgs e)
        {
            if (ActiveCheckBoxList.Count > 0 && ChBIndexNo.Checked)
            {
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
                foreach (var item in ListFilterChips)
                {
                    ResultSheet.Filters.Add(item.Tag.ToString().Split('~')[1]);
                    ResultSheet.FilterNames.Add(item.Tag.ToString().Split('~')[0]);
                }
                ResultSheet.SortBy = SortBy;
                ResultSheet.ShowDialog();
            }
            else
            {
                MessageBox.Show("Column 'Index No' is Compulsory. You can remove 'Index No' before Export", 
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CheckBox_CheckedChange(object sender, EventArgs e)
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

        private void CheckBox_DoubleClick(object sender, EventArgs e)
        {
            Guna2CustomCheckBox ColumnCheckBox = (Guna2CustomCheckBox)sender;
            SortBy = ColumnCheckBox.Tag.ToString();
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

        List<Guna2CustomCheckBox> ActiveCheckBoxList = new List<Guna2CustomCheckBox>() { };

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            DemoTable.Columns.Clear();

            Guna2Button SelectedBtn = (Guna2Button)sender;
            List<Guna2CustomCheckBox> CheckBoxesList = new List<Guna2CustomCheckBox>
            {
                ChBIndexNo, ChBEstName, ChBPerson, ChBDescription, ChBEH, ChBEstMoney, ChBMOE, ChBProgress, ChBPermission, 
                ChBZone, ChBDetails, ChBStartDate,
            };

            bool SelectMode;

            if (SelectedBtn.Tag.ToString() == "Default")
            {
                List<int> DefaultCheckBoxesList = new List<int>
                {
                    0, 3, 4, 6, 8, 9, 7, 10
                };
                SelectMode = false;
                CheckBoxValue();

                foreach (var item in DefaultCheckBoxesList)
                {
                    CheckBoxesList[item].Checked = true;
                }
            }
            else
            {
                if (SelectedBtn.Tag.ToString() == "Select All")
                {
                    SelectMode = true;
                }
                else
                {
                    ActiveCheckBoxList.Clear();
                    SelectMode = false;
                }

                CheckBoxValue();
            }

            void CheckBoxValue()
            {
                foreach (var item in CheckBoxesList)
                {
                    item.Checked = SelectMode;
                }
            }
        }

        List<Guna2Chip> ListFilterChips = new List<Guna2Chip> { };
        Guna2Chip SelectedChip = null, SelectedSortByChip = null;
        int FiltersCount = 15;
        string SortBy = null;

        private void btnFilterOperation_Click(object sender, EventArgs e)
        {
            Guna2CircleButton FilterBtn = (Guna2CircleButton)sender;

            if (DemoTable.ColumnCount > 0 || FilterBtn.Tag.ToString() == "Easy") 
            {
                List<string> ShowAddFiltersWindow(string Text, string Tag)
                {
                    List<string> ReturnValues = new List<string>();
                    AddFilters form = new AddFilters();
                    form.ActiveCheckBoxList = ActiveCheckBoxList;
                    if (Text != null & Tag != null)
                    {
                        form.ReturnValueTest = Text;
                        form.ReturnValueTag = Tag;
                    }
                    this.Opacity = 0.85;
                    form.NoOfReturnFilters = FiltersCount;
                    if (FiltersCount != 0 | FilterBtn.Tag.ToString() == "Edit") 
                    {
                        form.ShowDialog();
                    }
                    this.Opacity = 1.00;
                    ReturnValues.Add(form.ReturnValueTest);
                    ReturnValues.Add(form.ReturnValueTag);
                    if (form.ReturnValueTag != "")
                    {
                        FiltersCount -= form.FiltersCount;
                        lblFilterCount.Text = (15 - FiltersCount).ToString() + " of 15";
                    }
                    return ReturnValues;
                }

                switch (FilterBtn.Tag.ToString())
                {
                    case "Add":
                        {
                            Guna2Chip NewChip = new Guna2Chip();
                            NewChip.FillColor = NewChip.BorderColor = Color.LightSeaGreen;
                            NewChip.BackColor = Color.PaleTurquoise;
                            NewChip.AutoSize = true;
                            NewChip.IsClosable = false;
                            NewChip.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                            NewChip.TextAlign = HorizontalAlignment.Left;

                            List<string> ReturnList = ShowAddFiltersWindow(null, null);
                            NewChip.Text = ReturnList[0];
                            NewChip.Tag = ReturnList[1];

                            if (ListFilterChips.Count > 0)
                            {
                                if (!(ListFilterChips[ListFilterChips.Count - 1].Left +
                                    ListFilterChips[ListFilterChips.Count - 1].Width + 10 + NewChip.Width
                                    > FilterPanel.Width))
                                {
                                    NewChip.Top = ListFilterChips[ListFilterChips.Count - 1].Top;
                                    NewChip.Left = ListFilterChips[ListFilterChips.Count - 1].Left +
                                        ListFilterChips[ListFilterChips.Count - 1].Width + 10;
                                }
                                else
                                {
                                    NewChip.Top = ListFilterChips[ListFilterChips.Count - 1].Bottom + 10;
                                    NewChip.Left = 10;
                                }
                            }
                            else
                            {
                                NewChip.Top = 40;
                                NewChip.Left = 10;
                            }

                            NewChip.Click += NewChip_Click;

                            if (NewChip.Text != "")
                            {
                                FilterFlowPanel.Controls.Add(NewChip);
                                ListFilterChips.Add(NewChip);
                            }
                            break;
                        }
                    case "Edit":
                        {
                            try
                            {
                                if (SelectedChip != null)
                                {
                                    List<string> ReturnList = ShowAddFiltersWindow(SelectedChip.Text, SelectedChip.Tag.ToString());
                                    SelectedChip.Text = ReturnList[0];
                                    SelectedChip.Tag = ReturnList[1];
                                    if (SelectedChip.Text == "")
                                    {
                                        FilterFlowPanel.Controls.Remove(SelectedChip);
                                        ListFilterChips.Remove(SelectedChip);
                                    }
                                }
                            }
                            catch { }
                            break;
                        }
                    case "Remove":
                        {
                            foreach (var item in ListFilterChips)
                            {
                                item.Parent.Controls.Remove(item);
                            }
                            ListFilterChips.Clear();
                            FiltersCount = 15;
                            lblFilterCount.Text = (15 - FiltersCount).ToString() + " of 15";
                            Editbtn.Enabled = false;
                            Addbtn.Enabled = true;
                            break;
                        }
                    case "Delete":
                        {                            
                            try
                            {
                                if (SelectedChip != null)
                                {
                                    string[] PartsOfTag = SelectedChip.Tag.ToString().Split('#');
                                    FiltersCount += Convert.ToInt32(PartsOfTag[3]);
                                    if (FiltersCount >= 15)
                                    {
                                        FiltersCount = 15;
                                        lblFilterCount.Text = "0 of 15";
                                    }
                                }                                
                            }
                            catch { }
                            ListFilterChips.Remove(SelectedChip);
                            FilterFlowPanel.Controls.Remove(SelectedChip);
                            break;
                        }
                    case "SortBy":
                        {
                            SortByContextMenuStrip.Items.Clear();
                            foreach (var item in ActiveCheckBoxList)
                            {
                                SortByContextMenuStrip.Items.Add(item.Tag.ToString().Split('~')[0]);
                            }
                            SortByContextMenuStrip.Visible = true;
                            SortByContextMenuStrip.Left = MousePosition.X;
                            SortByContextMenuStrip.Top = MousePosition.Y;                            
                            break;
                        }
                    case "Easy":
                        {
                            EasyMode form = new EasyMode();
                            form.ParentFormName = "Filter";
                            form.ShowDialog();
                            break;
                        }
                }

                Removebtn.Enabled = Deletebtn.Enabled = Editbtn.Enabled = true;
                if (FiltersCount == 15)
                    Removebtn.Enabled = Deletebtn.Enabled = Editbtn.Enabled = false;
            }
            else
            {
                MessageBox.Show("Setelct Table Columns", "Something Went Wrong !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NewChip_Click(object sender, EventArgs e)
        {
            SelectedChip = (Guna2Chip)sender;

            for (var i = 0; i < ListFilterChips.Count; i++)
            {
                ListFilterChips[i].FillColor = Color.LightSeaGreen;                
            }
            SelectedChip.FillColor = Color.Teal;
        }

        private void ColumnChanged(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                if (DemoTable.ColumnCount > 0)
                {
                    DemoTable.Visible = true;
                    PanelBachDemoTable.Height = 81;
                }
            }
            catch
            {
                DemoTable.Visible = false;
                PanelBachDemoTable.Height = 60;
            }
        }

        private void lblFilterCount_TextChanged(object sender, EventArgs e)
        {
            Addbtn.Enabled = true;
            lblFilterCount.ForeColor = Color.Teal;

            if (FiltersCount == 0)
            {
                Addbtn.Enabled = false;
                lblFilterCount.ForeColor = Color.Red;
            }
        }

        private void SortByContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Guna2Chip SortByChip = new Guna2Chip();
            SortByChip.FillColor = SortByChip.BorderColor = Color.LightSeaGreen;
            SortByChip.BackColor = Color.PaleTurquoise;
            SortByChip.AutoSize = true;
            SortByChip.IsClosable = false;
            SortByChip.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            SortByChip.TextAlign = HorizontalAlignment.Left;

            SortByChip.Tag = SortByChip.Text = "Sort By " + e.ClickedItem.Text;
            SortByChip.Top = SortByChip.Left = 10;
            SortBy = e.ClickedItem.Text;
            SortByChip.Click += SortByChip_Click;
            FilterFlowPanel.Controls.Add(SortByChip);
            if (SelectedSortByChip != null)
            {
                SelectedSortByChip.Parent.Controls.Remove(SelectedSortByChip);
            }
            SelectedSortByChip = SortByChip;
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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            sqlFunctions.SignOut(this);
        }

        private void SortByChip_Click(object sender, EventArgs e)
        {
            Guna2Chip SortByChip = (Guna2Chip)sender;
            SelectedSortByChip = SortByChip;
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
            this.Close();
        }

        private void btnProgressHistory_Click(object sender, EventArgs e)
        {
            ProgressHistory form = new ProgressHistory();
            form.ShowDialog();
        }
    }
}
