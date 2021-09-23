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
    public partial class FilterCustomization : Form
    {
        public FilterCustomization()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();

        public string SelectedDataSet = null;
        public bool ApplyFilters, Reached = false;

        public List<string> SelectedPermission = new List<string> { };
        public List<string> SelectedMOE = new List<string> { };
        public List<string> SelectedProgressRange = new List<string> { };
        public List<string> SelectedSection = new List<string> { };
        public DateTime SelectedMinimumStartDate = DateTime.Now,
            SelectedMaximumStartDate = DateTime.Now;

        public List<string> SelectedPlant = new List<string> { };

        public string SelectedArea = "", SelectedYear = "", SelectedMonth = "";

        List<Guna2Button> ListSectionButtons;
        DateTime MinimumDateOFMaximumStartDate = DateTime.Now;

        private void FilterCustomization_Load(object sender, EventArgs e)
        {
            if (!Reached)
            {
                if (SelectedDataSet == "Production")
                    pagedControl1.SelectedPage = page2;
                else if (SelectedDataSet == "Consumption")
                {
                    pagedControl1.SelectedPage = page3;
                    foreach (string AreaName in 
                        sqlFunctions.SQLRead("SELECT DISTINCT LocationName FROM Area", "LocationName"))
                    {
                        ComboBoxSelectedArea.Items.Add(AreaName);
                    }
                    ComboBoxSelectedArea.Items.Add("All");

                    ComboBoxSelectedArea.SelectedItem = ComboBoxSelectedYear.SelectedItem = 
                        ComboBoxSelectedMonth.SelectedItem = "All";
                }
                else
                {
                    ListSectionButtons = new List<Guna2Button>
                    {
                        guna2Button13,guna2Button12,guna2Button16,guna2Button15,guna2Button19,
                        guna2Button17,guna2Button18,guna2Button14,guna2Button11
                    };

                    ToggleSwitchStartDateAll.Checked = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ApplyFilters = false;
            Reached = true;
            this.Hide();
        }

        private void btnApplyFilters_Click(object sender, EventArgs e)
        {
            ApplyFilters = true;
            Reached = true;
            this.Hide();
        }

        private void btnPermissionOrMOE_CheckedChanged(object sender, EventArgs e)
        {
            Guna2Button SelectedButton = (Guna2Button)sender;

            if (SelectedButton.Checked)
            {
                if (SelectedButton.Tag.ToString() == "Permission")
                    SelectedPermission.Add(SelectedButton.Text);
                else if (SelectedButton.Tag.ToString() == "MOE")
                    SelectedMOE.Add(SelectedButton.Text);
                else
                    SelectedProgressRange.Add(SelectedButton.Text);
            }
            else
            {
                if (SelectedButton.Tag.ToString() == "Permission")
                    SelectedPermission.Remove(SelectedButton.Text);
                else if (SelectedButton.Tag.ToString() == "MOE")
                    SelectedMOE.Remove(SelectedButton.Text);
                else
                    SelectedProgressRange.Remove(SelectedButton.Text);
            }
        }

        private void Section_CheackedChanged(object sender, EventArgs e)
        {
            Guna2Button SelectedSectionButton = (Guna2Button)sender;

            if (SelectedSectionButton.Checked)
            {
                SelectedSection.Add(SelectedSectionButton.Tag.ToString());
                ToggleSwitchSectionAll.Checked = false;
            }
            else
            {
                SelectedSection.Remove(SelectedSectionButton.Tag.ToString());

                if (SelectedSection.Count == 0)
                    ToggleSwitchSectionAll.Checked = true;
            }
        }

        private void AllToggleSwitch_Click(object sender, EventArgs e)
        {
            Guna2ToggleSwitch SelectAllToggleSwitch = (Guna2ToggleSwitch)sender;

            if (SelectAllToggleSwitch.Checked)
            {
                if (SelectAllToggleSwitch.Tag.ToString() == "Section")
                {
                    foreach (Guna2Button btnSelectedCheack in ListSectionButtons)
                        btnSelectedCheack.Checked = false;
                }
            }
            else
            {
                if (SelectAllToggleSwitch.Tag.ToString() == "Section")
                {
                    ListSectionButtons[ListSectionButtons.Count - 1].Checked = true;
                }
            }
        }

        private void ToggleSwitchProgressHistory_CheckedChanged(object sender, EventArgs e)
        {
            if (ToggleSwitchProgressHistory.Checked)
            {
                btnSelectedProgressStatusCompleted.Checked = btnSelectedProgressStatusNotStarted.Checked =
                    btnSelectedProgressStatusUnderWork.Checked = btnSelectedProgressStatusCompleted.Enabled =
                    btnSelectedProgressStatusNotStarted.Enabled = btnSelectedProgressStatusUnderWork.Enabled = false;
            }
            else
            {
                btnSelectedProgressStatusCompleted.Enabled =
                    btnSelectedProgressStatusNotStarted.Enabled = btnSelectedProgressStatusUnderWork.Enabled = true;
            }
        }

        private void StartDateRange_ValueChanged(object sender, EventArgs e)
        {
            Guna2DateTimePicker StartDateRange = (Guna2DateTimePicker)sender;

            if (StartDateRange.Tag.ToString() == "MinimumStartDate") 
            {
                MinimumDateOFMaximumStartDate = StartDateRange.Value;
                SelectedMinimumStartDate = MaxmumStartDate.Value = StartDateRange.Value;
            }
            else
            {
                if (StartDateRange.Value < MinimumDateOFMaximumStartDate)
                {
                    MessageBox.Show("Something Went Wrong!", "Input Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StartDateRange.Value = MinimumStartDate.Value;
                }
                SelectedMaximumStartDate = StartDateRange.Value;
            }

            if (MaxmumStartDate.Value.Date != DateTime.Now.Date || MinimumStartDate.Value.Date != DateTime.Now.Date) 
            {
                ToggleSwitchStartDateAll.Checked = false;
            }
        }

        private void ToggleSwitchStartDateAll_CheckedChanged(object sender, EventArgs e)
        {
            Guna2ToggleSwitch ToggleSwitchStartDateAll = (Guna2ToggleSwitch)sender;

            if (ToggleSwitchStartDateAll.Checked)
            {
                MinimumStartDate.Value = DateTime.Now;
                MaxmumStartDate.Value = DateTime.Now;
            }
        }

        private void btnSelectedPlant_CheckedChanged(object sender, EventArgs e)
        {
            Guna2Button btnSelectedPlant = (Guna2Button)sender;
            if (btnSelectedPlant.Checked)
            {
                SelectedPlant.Add(btnSelectedPlant.Tag.ToString());
            }
            else
            {
                SelectedPlant.Remove(btnSelectedPlant.Tag.ToString());
            }
        }

        private void AreaFilterItemChanged(object sender, EventArgs e)
        {
            Guna2ComboBox AreaFilterItem = (Guna2ComboBox)sender;

            if (AreaFilterItem.Tag.ToString() == "Area")
                SelectedArea = AreaFilterItem.SelectedItem.ToString();
            else if (AreaFilterItem.Tag.ToString() == "Year")
                SelectedYear = AreaFilterItem.SelectedItem.ToString();
            else
                SelectedMonth = (AreaFilterItem.SelectedIndex + 1).ToString();
        }

    }
}
