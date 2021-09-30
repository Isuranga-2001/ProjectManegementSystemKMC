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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        MultiFunctions multiFunctions = new MultiFunctions();

        List<Guna2TextBox> InputControlList;

        int SelectedRow = 0;
        string SelectedOparation = null, SelectedID = null;
        public string ParentSelectedField = null;        

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(1280, 720);

            InputControlList = new List<Guna2TextBox>
            {
                txtEHMain,txtEHSub1,txtEHSub2,txtDescription,txtEHSub3,txtSection,txtEstimator
            };

            MenuSection.Items.Clear();
            foreach (string Item in sqlFunctions.SQLRead("SELECT DISTINCT ZoneNo FROM Estimator WHERE Active='Yes'", "ZoneNo"))
            {
                MenuSection.Items.Add(Item);
            }            

            btnDefaultData.Checked = true;
        }

        private void btnAbout_CheckedChanged(object sender, EventArgs e)
        {
            if (btnAbout.Checked)
            {
                About aboutForm = new About();
                aboutForm.ShowDialog();
                btnAbout.Checked = false;
            }
        }

        private void btnSettigsFromNavigation_CheckedChanged(object sender, EventArgs e)
        {
            Guna2Button btnSettingNavigation = (Guna2Button)sender;

            if (btnSettingNavigation.Checked)
            {
                if (btnSettingNavigation.Tag.ToString() == "Data")
                {
                    if (ParentSelectedField == null) 
                        btnDefualtDataEH.Checked = true;
                    else
                    {
                        if (ParentSelectedField == "Est")
                            btnDefualDataEst.Checked = true;
                        else
                            btnDefualtDataEH.Checked = true;
                    }
                    pagedControl1.SelectedPage = page1;
                }
                else
                {
                    pagedControl1.SelectedPage = page2;
                    // reset method call 
                }
            }
        }

        private void btnDefualDataSettingsNavigation_CheckedChanged(object sender, EventArgs e)
        {
            Guna2Button btnDefualDataSettingsNavigation = (Guna2Button)sender;

            if (btnDefualDataSettingsNavigation.Checked)
            {
                if (btnDefualDataSettingsNavigation.Tag.ToString() == "EH")
                {
                    pagedControl2.SelectedPage = page3;
                }
                else
                {
                    pagedControl2.SelectedPage = page4;
                }

                ResetTable(TableDefualtData, btnDefualDataSettingsNavigation.Tag.ToString());
                pagedControl2.Enabled = false;
            }
        }

        void ResetTable(Guna2DataGridView SelectedDataTable, string SelectedField)
        {
            SelectedDataTable.Columns.Clear();

            if (SelectedField == "EH")
            {
                SelectedDataTable.DataSource = sqlFunctions.RetrieveDataOnDataTable("SELECT * FROM ExpenditureHead WHERE Active='Yes'");

                SelectedDataTable.Columns.RemoveAt(0);
                SelectedDataTable.Columns.Remove("Active");
                SelectedDataTable.Columns[4].HeaderText = "Expenditure Head Description";
                SelectedDataTable.Columns[0].Width = SelectedDataTable.Columns[1].Width =
                    SelectedDataTable.Columns[2].Width = SelectedDataTable.Columns[3].Width = 120;
            }
            else if(SelectedField == "Est")
            {
                SelectedDataTable.DataSource = sqlFunctions.RetrieveDataOnDataTable("SELECT * FROM Estimator WHERE Active='Yes'");

                SelectedDataTable.Columns.RemoveAt(0);
                SelectedDataTable.Columns.Remove("Active");
                SelectedDataTable.Columns[1].HeaderText = "Section";
                SelectedDataTable.Columns[1].DisplayIndex = 0;
            }
            else
            {
                SelectedDataTable.DataSource = sqlFunctions.RetrieveDataOnDataTable("SELECT * FROM Login");
            }

            foreach (Guna2TextBox InputControl in InputControlList)
            {
                InputControl.Clear();
            }
        }

        private void btnDefualtDataOparation_Click(object sender, EventArgs e)
        {
            Guna2ImageButton btnDefualtDataOparation = (Guna2ImageButton)sender;

            if (btnDefualtDataOparation.Tag.ToString() == "Add" || btnDefualtDataOparation.Tag.ToString() == "Edit")
            {
                pagedControl2.Enabled = true;
                SelectedOparation = btnDefualtDataOparation.Tag.ToString();

                foreach (Guna2TextBox InputControl in InputControlList)
                {
                    InputControl.Clear();
                }

                if (btnDefualtDataOparation.Tag.ToString() == "Edit")
                {
                    DataGridViewRow dataRow = TableDefualtData.Rows[SelectedRow];

                    if (pagedControl1.SelectedPage == page2)
                    {
                        // User data 
                    }
                    else if (pagedControl2.SelectedPage.Tag.ToString() == "EH")
                    {
                        txtEHMain.Text = dataRow.Cells[0].Value.ToString().Trim();
                        txtEHSub1.Text = dataRow.Cells[1].Value.ToString().Trim();
                        txtEHSub2.Text = dataRow.Cells[2].Value.ToString().Trim();
                        txtEHSub3.Text = dataRow.Cells[3].Value.ToString().Trim();
                        txtDescription.Text = dataRow.Cells[4].Value.ToString().Trim();
                    }
                    else
                    {
                        txtSection.Text = dataRow.Cells[1].Value.ToString().Trim();
                        txtEstimator.Text = dataRow.Cells[0].Value.ToString().Trim();
                    }
                }
            }
            else
            {
                pagedControl2.Enabled = false;

                string SelectedTable, PrimaryKeyColumnName;
                if (pagedControl2.SelectedPage.Tag.ToString().Trim() == "EH")
                {
                    SelectedTable = "ExpenditureHead";
                    PrimaryKeyColumnName = "EHID";
                }
                else
                {
                    SelectedTable = "Estimator";
                    PrimaryKeyColumnName = "EstimatorNO";
                }

                sqlFunctions.ExecuteSQL(String.Format("UPDATE {0} SET Active='No' WHERE {1}='{2}'",
                    SelectedTable, PrimaryKeyColumnName, SelectedID));

                ResetTable(TableDefualtData, pagedControl2.SelectedPage.Tag.ToString());
            }
        }

        private void TableDefualtData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < TableDefualtData.Rows.Count - 1)
            {
                SelectedRow = e.RowIndex;
                pagedControl2.Enabled = false;

                if (pagedControl2.SelectedPage.Tag.ToString() == "Est")
                {
                    List<string> AvaiableIDList = sqlFunctions.SQLRead(
                        String.Format("SELECT EstimatorNO FROM Estimator WHERE (EstName='{0}' OR EstName IS NULL) AND ZoneNO='{1}'",
                        TableDefualtData.Rows[e.RowIndex].Cells[0].Value.ToString(),
                        TableDefualtData.Rows[e.RowIndex].Cells[1].Value.ToString()), "EstimatorNO");

                    if (AvaiableIDList.Count == 1)
                    {
                        SelectedID = AvaiableIDList[0];
                    }
                    else
                    {
                        SelectedID = sqlFunctions.SQLRead(
                            String.Format("SELECT EstimatorNO FROM Estimator WHERE EstName='{0}' AND ZoneNO='{1}'",
                            TableDefualtData.Rows[e.RowIndex].Cells[0].Value.ToString(),
                            TableDefualtData.Rows[e.RowIndex].Cells[1].Value.ToString()), "EstimatorNO")[0];
                    }                    
                }
                else
                {
                    List<string> AvaiableIDList = sqlFunctions.SQLRead(
                        String.Format("SELECT EHID FROM ExpenditureHead WHERE EH_Main='{0}' AND EH_Sub1='{1}' AND EH_Sub2='{2}' " +
                        "AND (EH_Sub3='{3}' OR EH_Sub3 IS NULL) AND ExpenditureHeadDiscription=N'{4}' AND Active='Yes'",
                        TableDefualtData.Rows[e.RowIndex].Cells[0].Value.ToString().Trim(),
                        TableDefualtData.Rows[e.RowIndex].Cells[1].Value.ToString().Trim(),
                        TableDefualtData.Rows[e.RowIndex].Cells[2].Value.ToString().Trim(),
                        TableDefualtData.Rows[e.RowIndex].Cells[3].Value.ToString().Trim(),
                        TableDefualtData.Rows[e.RowIndex].Cells[4].Value.ToString().Trim()), "EHID");

                    if (AvaiableIDList.Count == 1)
                    {
                        SelectedID = AvaiableIDList[0];
                    }
                    else
                    {
                        SelectedID = sqlFunctions.SQLRead(
                            String.Format("SELECT EHID FROM ExpenditureHead WHERE EH_Main='{0}' AND EH_Sub1='{1}' AND EH_Sub2='{2}' " +
                            "AND EH_Sub3='{3}' AND ExpenditureHeadDiscription=N'{4}' AND Active='Yes'",
                            TableDefualtData.Rows[e.RowIndex].Cells[0].Value.ToString().Trim(),
                            TableDefualtData.Rows[e.RowIndex].Cells[1].Value.ToString().Trim(),
                            TableDefualtData.Rows[e.RowIndex].Cells[2].Value.ToString().Trim(),
                            TableDefualtData.Rows[e.RowIndex].Cells[3].Value.ToString().Trim(),
                            TableDefualtData.Rows[e.RowIndex].Cells[4].Value.ToString().Trim()), "EHID")[0];
                    }
                }
            }                
        }

        private void btnDefualDataEditedAdd_Click(object sender, EventArgs e)
        {
            Guna2Button btnSave = (Guna2Button)sender;

            int StartIndex = 0, EndIndex = InputControlList.Count - 2;

            if (btnSave.Tag.ToString() == "EH")
                EndIndex = 3;
            else
                StartIndex = 5;

            bool CompleteRequirments = true;

            for (int i = StartIndex; i <= EndIndex; i++)
            {
                if (InputControlList[i].Text.Trim() == "") 
                {
                    CompleteRequirments = false;
                    break;
                }
            }

            if (CompleteRequirments)
            {
                string SelectedTable, PrimaryKeyColumnName;
                if (pagedControl2.SelectedPage.Tag.ToString().Trim() == "EH")
                {
                    SelectedTable = "ExpenditureHead";
                    PrimaryKeyColumnName = "EHID";
                }
                else
                {
                    SelectedTable = "Estimator";
                    PrimaryKeyColumnName = "EstimatorNO";
                }

                if (SelectedOparation == "Add")
                {
                    // add to database

                    string CheckActivation = "SELECT Active From " + SelectedTable + " WHERE ";
                    string ActiveAgain = "UPDATE " + SelectedTable + " SET Active='Yes' WHERE ";

                    string MaxOfPrimaryKey = sqlFunctions.SQLRead("SELECT MAX(" + PrimaryKeyColumnName + ") " +
                        "AS MaxOfPK FROM " + SelectedTable, "MaxOfPK")[0];

                    string InsertQuery = "INSERT INTO [dbo].[" + SelectedTable + "] ([" + PrimaryKeyColumnName + "],";

                    for (var i = 0; i < TableDefualtData.ColumnCount; i++)
                    {
                        InsertQuery += "[" + TableDefualtData.Columns[i].Name + "], ";
                    }
                    InsertQuery = InsertQuery.Remove(InsertQuery.Length - 2) + ", [Active]) VALUES (";

                    string EditDataInputs = null;

                    switch (SelectedTable)
                    {
                        case "ExpenditureHead":
                            {
                                string MaxOfPrimaryKeyNumbers = (Convert.ToInt32(MaxOfPrimaryKey[2].ToString() + MaxOfPrimaryKey[3].ToString()
                                    + MaxOfPrimaryKey[4].ToString() + MaxOfPrimaryKey[5].ToString()) + 1).ToString();

                                int count = MaxOfPrimaryKeyNumbers.Length;
                                for (int i = count; i < 4; i++)
                                {
                                    MaxOfPrimaryKeyNumbers = "0" + MaxOfPrimaryKeyNumbers;
                                }

                                InsertQuery += "'EH" + MaxOfPrimaryKeyNumbers + "', '" + txtEHMain.Text + "', '"
                                    + txtEHSub1.Text + "', '" + txtEHSub2.Text + "', '" + txtEHSub3.Text + "', N'"
                                    + txtDescription.Text + "', 'Yes')";

                                EditDataInputs = "EH_Main='" + txtEHMain.Text + "' AND EH_Sub1='" + txtEHSub1.Text + "' AND EH_Sub2='" + txtEHSub2.Text + "' AND" +
                                    " EH_Sub3='" + txtEHSub3.Text + "' AND ExpenditureHeadDiscription=N'" + txtDescription.Text + "' ";

                                break;
                            }
                        case "Estimator":
                            {
                                InsertQuery += "'" + (Convert.ToInt32(MaxOfPrimaryKey) + 1).ToString() + "', N'"
                                    + txtEstimator.Text + "', '" + txtSection.Text + "', 'Yes')";

                                EditDataInputs = "EstName=N'" + txtEstimator.Text + "' AND ZoneNo='" + txtSection.Text + "'";
                                break;
                            }
                    }

                    CheckActivation += EditDataInputs;
                    ActiveAgain += EditDataInputs;

                    if (sqlFunctions.SQLRead(CheckActivation, "Active") == null)
                    {
                        if (!(sqlFunctions.ExecuteSQL(InsertQuery)))
                        {
                            sqlFunctions.ErrorMessage();
                        }
                        else
                        {
                            MessageBox.Show("Defualt Data Added Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            pagedControl2.Enabled = false;
                        }
                    }
                    else
                    {
                        if (!(sqlFunctions.ExecuteSQL(ActiveAgain)))
                        {
                            sqlFunctions.ErrorMessage();
                        }
                        else
                        {
                            MessageBox.Show("Defualt Data Added Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            pagedControl2.Enabled = false;
                        }
                    }
                }
                else
                {
                    // edit data in database

                    string UpdateQuery = null;

                    switch (pagedControl2.SelectedPage.Tag.ToString())
                    {
                        case "EH":
                            {
                                UpdateQuery = "UPDATE ExpenditureHead SET EH_Main='" + txtEHMain.Text + "', EH_Sub1='" + txtEHSub1.Text + "', EH_Sub2='" + txtEHSub2.Text + "'," +
                                    " EH_Sub3='" + txtEHSub3.Text + "', ExpenditureHeadDiscription=N'" + txtDescription.Text + "' ";

                                break;
                            }
                        case "Est":
                            {
                                UpdateQuery = "UPDATE Estimator SET EstName=N'" + txtEstimator.Text + "', ZoneNo=N'" + txtSection.Text + "' ";
                                break;
                            }
                    }

                    UpdateQuery += "WHERE " + PrimaryKeyColumnName + "='" + SelectedID + "'";

                    if (!(sqlFunctions.ExecuteSQL(UpdateQuery)))
                    {
                        sqlFunctions.ErrorMessage();
                    }
                    else
                    {
                        MessageBox.Show("Updated Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pagedControl2.Enabled = false;
                    }
                }

                ResetTable(TableDefualtData, pagedControl2.SelectedPage.Tag.ToString());
            }
            else
            {
                MessageBox.Show("Fill All Requirment Fields", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            TableDefualtData.Rows[0].Selected = true;
            pagedControl2.Enabled = false;
        }

        private void MenuSection_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            btnDownArrow.Checked = false;
        }

        private void btnDownArrow_CheckedChanged(object sender, EventArgs e)
        {
            MenuSection.Show();
            MenuSection.Left = this.Left + pagedControl1.Left + pagedControl2.Left + txtSection.Left + 20;
            MenuSection.Top = this.Top + pagedControl1.Top + pagedControl2.Top + txtSection.Bottom;
        }

        private void MenuSection_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            txtSection.Text = e.ClickedItem.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            multiFunctions.NavigateTo(btnClose.Tag.ToString(), this);
        }

        private void NavigationBarButton_Click(object sender, EventArgs e)
        {
            Guna2CircleButton btnNavigation = (Guna2CircleButton)sender;
            multiFunctions.NavigateTo(btnNavigation.Tag.ToString(), this);
        }
    }
}
