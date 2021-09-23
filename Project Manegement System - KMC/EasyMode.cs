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
    public partial class EasyMode : Form
    {
        public EasyMode()
        {
            InitializeComponent();
        }

        public string ParentFormName = "DashBoard";
        public string ProgressHistoryFilters = null;
        public Guna2DataGridView DemoTable = new Guna2DataGridView();
        public List<Guna2CustomCheckBox> ActiveCheckBoxList = new List<Guna2CustomCheckBox>() { };
        List<string> ListFilters = new List<string> { };
        List<string> SelectedZone = new List<string> { };
        List<string> SelectedMOE = new List<string> { };

        private void EasyMode_Load(object sender, EventArgs e)
        {            
            ChBDetails.Checked = ChBProgress.Checked = ChBZone.Checked = ChBPermission.Checked = ChBMOE.Checked =
                    ChBEH.Checked = ChBDescription.Checked = ChBIndexNo.Checked = true;

            txtStartYear.Items.Clear();
            txtEndYear.Items.Clear();
            for (int year = 2020; year < (Convert.ToInt32(DateTime.Now.Year) + 1); year++) 
            {
                txtStartYear.Items.Add(year);
                txtEndYear.Items.Add(year);
            }
            if (ParentFormName == "DashBoard")
            {
                txtStartYear.SelectedItem = DateTime.Now.Year.ToString();
                btnZoneAll.Checked = true;
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

        private void btnCurrentStatus_CheckedChange(object sender, EventArgs e)
        {
            Guna2Button btnZoneNo = (Guna2Button)sender;
            txtFrom.ReadOnly = txtTo.ReadOnly = true;
            txtTo.Text = txtFrom.Text = "";
            txtTo.Enabled = txtFrom.Enabled = false;
            if (btnZoneNo.Tag.ToString()== "Complited")
            {
                txtFrom.Text = "100";
                txtTo.Enabled = false;
            }
            else if (btnZoneNo.Tag.ToString() == "Under Work")
            {
                txtFrom.Text = "1";
                txtTo.Text = "99";
            }
            else if (btnZoneNo.Tag.ToString() == "Not Started")
            {
                txtTo.Text = "0";
                txtFrom.Enabled = false;
            }
            else
            {
                txtFrom.ReadOnly = txtTo.ReadOnly = false;
                txtTo.Enabled = txtFrom.Enabled = true;
                txtFrom.Text = "0";
                txtTo.Text = "100";
            }
        }

        private void txtStartYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEndYear.SelectedItem = txtStartYear.SelectedItem;
        }

        private void btnZone_CheckChanged(object sender, EventArgs e)
        {
            Guna2Button btnZoneNo = (Guna2Button)sender;
            if (btnZoneNo.Checked)
            {
                SelectedZone.Add(btnZoneNo.Tag.ToString());
                if (btnZoneNo.Tag.ToString() == "All")
                    btnZone1.Checked = btnZone2.Checked = btnZone3.Checked = btnZone4.Checked = btnZone5.Checked = btnZoneBuildings.Checked = false;
                else
                    btnZoneAll.Checked = false;
            }
            else
            {
                SelectedZone.Remove(btnZoneNo.Tag.ToString());
            }
        }

        private void SelectMOE(object sender, EventArgs e)
        {
            Guna2Button btnMOENo = (Guna2Button)sender;
            if (btnMOENo.Checked)
            {
                SelectedMOE.Add(btnMOENo.Tag.ToString());
                if (btnMOENo.Tag.ToString() == "All")
                    btnSociety.Checked = btnDepartment.Checked = btnTenderer.Checked = btnSpecialProjects.Checked = false;
                else
                    btnMOEAll.Checked = false;
            }
            else
            {
                SelectedMOE.Remove(btnMOENo.Tag.ToString());
            }
        }

        private void BtnShowResults_Click(object sender, EventArgs e)
        {
            if (ActiveCheckBoxList.Count > 0)
            {
                bool error = false;

                if (txtStartYear.Text != "" && txtStartYear.Text == txtEndYear.Text)
                {
                    ListFilters.Add("#Index No#~ProjectID Like '2" + txtStartYear.SelectedIndex.ToString() + "%'");
                }
                else if (txtStartYear.Text != "" && txtEndYear.Text != "" && txtStartYear.Text != txtEndYear.Text)
                {
                    string Query = "#Index No#~";
                    if (Convert.ToInt32(txtStartYear.Text) < Convert.ToInt32(txtEndYear.Text))
                    {
                        for (var count = txtStartYear.SelectedIndex; count <= txtEndYear.SelectedIndex; count++)
                        {
                            Query += "ProjectID Like '2" + count.ToString() + "%' OR ";
                        }
                        Query = Query.Remove(Query.Length - 4);
                        ListFilters.Add(Query);
                    }
                    else
                    {
                        MessageBox.Show("Year Entered Incorrctly!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        error = true;
                    }
                }

                if (SelectedZone.Count > 0)
                {
                    string Query = "#Section#~(";
                    foreach (var ZoneNo in SelectedZone)
                    {
                        if (ZoneNo == "All")
                        {
                            Query = "#Section#~(";
                            foreach (var ZoneNoAll in new List<string> { "1", "2", "3", "4", "5" })
                            {
                                Query += "ZoneNo='" + ZoneNoAll + "' OR ";
                            }
                            Query += "ZoneNo='Buildings' OR ";
                            break;
                        }
                        else
                        {
                            Query += "ZoneNo='" + ZoneNo + "' OR ";
                        }
                    }
                    Query = Query.Remove(Query.Length - 4);
                    ListFilters.Add(Query + ")");
                }

                if (SelectedMOE.Count > 0)
                {
                    string Query = "#Method Of Execution#~";
                    foreach (var MOE in SelectedMOE)
                    {
                        if (MOE == "All")
                        {
                            Query = "#Method Of Execution#~(MethodOfExecution='Department' OR MethodOfExecution='Society' " +
                                "OR MethodOfExecution='Tenderer' OR MethodOfExecution='Special Projects') OR ";
                            break;
                        }
                        else
                        {
                            Query += "MethodOfExecution='" + MOE + "' OR ";
                        }
                    }
                    Query = Query.Remove(Query.Length - 4);
                    ListFilters.Add(Query);
                }

                if (txtFrom.Text != "")
                {
                    ListFilters.Add("#Progress#~Progress >= '" + txtFrom.Text + "'");
                }
                if (txtTo.Text != "")
                {
                    ListFilters.Add("#Progress#~Progress <= '" + txtTo.Text + "'");
                }

                if (!(ChBZone.Checked) || !(ChBProgress.Checked) || !(ChBIndexNo.Checked)) 
                {
                    MessageBox.Show("Select All Compulsory Columns", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    error = true;
                }

                if (!(error))
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

                    if (ParentFormName == "ProgressHistory")
                    {
                        ResultSheet.ParentFormName = ParentFormName;
                        if (ProgressHistoryFilters != null)
                        {
                            ListFilters.Add("#Progress#~" + ProgressHistoryFilters);
                        }
                    }

                    foreach (var item in ListFilters)
                    {
                        ResultSheet.Filters.Add(item.Split('~')[1]);
                        ResultSheet.FilterNames.Add(item.Split('~')[0]);
                    }                   
                    
                    ResultSheet.SortBy = null;
                    ResultSheet.ShowDialog();
                    ListFilters.Clear();
                }                
            }
        }

        private void txtProgressDifferent_TextChange(object sender, EventArgs e)
        {
            Guna2TextBox txtProgress = (Guna2TextBox)sender;
            if (txtProgress.Text != "")
            {
                char[] originalText = txtProgress.Text.ToCharArray();
                foreach (char c in originalText)
                {
                    if (!(Char.IsNumber(c)))
                    {
                        txtProgress.Text = txtProgress.Text.Remove(txtProgress.Text.IndexOf(c));
                    }
                }
                txtProgress.Select(txtProgress.Text.Length, 0);

                if (Convert.ToInt32(txtProgress.Text) > 100)
                {
                    txtProgress.Text = txtProgress.Text.Remove(txtProgress.Text.Length - 1);
                }
            }
        }
    }
}
