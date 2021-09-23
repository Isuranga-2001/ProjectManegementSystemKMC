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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }
        int FormTop = 0, FormTopInEditData = 0;
        AddNew SqlFunctionsExecute = new AddNew();
        SQLFunctions SqlFunctionsRead = new SQLFunctions();

        private void NavigationClick(object sender, EventArgs e)
        {
            this.Height = 550;
            this.Top = FormTop;

            Guna2GradientButton NavigationButton = (Guna2GradientButton)sender;
            if (NavigationButton == btnUsers)
            {
                PagedControlBox.SelectedPage = page2;
                PanelUserInput.Visible = false;
            }
            else if (NavigationButton == btnEditData)
            {
                PagedControlBox.SelectedPage = page3;
                this.Height = 710;
                this.Top = FormTopInEditData;
            }
            else if (NavigationButton == btnAbout)
            {
                PagedControlBox.SelectedPage = page2;
                About form = new About();
                form.ShowDialog();
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            FormTop = this.Top;
            FormTopInEditData = FormTop - 80;

            if (btnEditData.Checked == true)
            {
                btnUsers.Checked = false;
                PagedControlBox.SelectedPage = page3;
                this.Height = 710;
            }
            else
            {
                btnUsers.Checked = true;
                PagedControlBox.SelectedPage = page2;
                btnEditDataEH.Checked = true;
            }

            RefreshtxtEstimator();
            RefreshLoginDataGrid();
        }

        void RefreshtxtEstimator()
        {
            txtEstimator.Items.Clear();
            List<string> EstimatorNameList = SqlFunctionsRead.SQLRead("SELECT EstName FROM dbo.Estimator WHERE Active='Yes'", "EstName");
            foreach (var Item in EstimatorNameList)
            {
                txtEstimator.Items.Add(Item);
            }
        }

        public void BeforeLoad()
        {
            this.Height = 550;
            if (btnEditData.Checked == true)
            {
                this.Height = 710;
            }
        }

        /// <summary>
        /// User Page # Page no 2
        /// </summary>        
        string UserType = "User", SelectedOperation = "NoN";
        int UserID = 0;

        void RefreshLoginDataGrid()
        {
            DataGridViewLogin.Columns.RemoveAt(3);
            if (!(SqlFunctionsRead.RetrieveDataTable("SELECT Username,Password,Type FROM Login", DataGridViewLogin)))
            {
                SqlFunctionsRead.ErrorMessage();
            }/*
            else
            {
                for (int i = 0; i < DataGridViewLogin.RowCount - 1; i++) 
                {
                    if (DataGridViewLogin.Rows[i].Cells[3].Value.ToString().Trim() != "") 
                    {
                        DataGridViewLogin.Rows[i].Cells[3].Value = SqlFunctionsRead.SQLRead("SELECT EstName FROM Estimator WHERE EstimatorNo='" + DataGridViewLogin.Rows[i].Cells[3].Value.ToString().Trim() + "'", "EstName")[0].Trim();
                    }
                }
            }*/
            UserID = 0;
            ClearUserInput();
        }

        void ClearUserInput()
        {
            txtPassword.Text = txtUsername.Text = "";
            txtEstimator.SelectedIndex = -1;
            SwitchUserType.Checked = false;
            btnAddUser.Checked = btnRemoveUser.Checked = btnChangeUsername.Checked = false;
        }

        private void btnPasswordShow_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar == false)
            {
                btnPasswordShow.Image = Properties.Resources.Password_Hide;
                txtPassword.UseSystemPasswordChar = true;
            }
            else
            {
                btnPasswordShow.Image = Properties.Resources.Password_Show;
                txtPassword.UseSystemPasswordChar = false;
            }            
        }

        private void btnUserOK_Click(object sender, EventArgs e)
        {
            PanelUserInput.Visible = false;
            if (SelectedOperation == "Change" || SelectedOperation == "Add")
            {
                var QueryList = new List<string> { };

                if (SelectedOperation == "Change")
                {
                    QueryList.Add("DELETE FROM [dbo].[Login] WHERE [Username] = '" + DataGridViewLogin.Rows[UserID].Cells[0].Value.ToString() + "'");
                }
                /*
                if (UserType == "User")
                {
                    QueryList.Add("INSERT INTO [dbo].[Login] ([Username], [Password], [Type], [EstimatorNO]) " +
                                    "VALUES ('" + txtUsername.Text + "', '" + txtPassword.Text + "', '" + UserType + "'," +
                                    "'" + SqlFunctionsRead.SQLRead("SELECT EstimatorNO FROM Estimator WHERE EstName='" + txtEstimator.Text.Trim() + "'", "EstimatorNO")[0] + "')");
                }
                else
                {
                    QueryList.Add("INSERT INTO [dbo].[Login] ([Username], [Password], [Type]) " +
                                    "VALUES ('" + txtUsername.Text + "', '" + txtPassword.Text + "', '" + UserType + "')");
                }*/

                QueryList.Add("INSERT INTO [dbo].[Login] ([Username], [Password], [Type]) " +
                "VALUES ('" + txtUsername.Text + "', '" + txtPassword.Text + "', '" + UserType + "')");


                ClearUserInput();

                for (var i = 0; i < QueryList.Count; i++)
                {
                    if (!(SqlFunctionsExecute.ExecuteSQL(QueryList[i])))
                    {
                        SqlFunctionsRead.ErrorMessage();
                        PanelUserInput.Visible = true;
                        break;
                    }
                }
            }
            RefreshLoginDataGrid();
        }

        private void UserOperationButton_Click(object sender, EventArgs e)
        {
            Guna2Button UserOperationButton = (Guna2Button)sender;
            PanelUserInput.Visible = true;
            if (UserOperationButton.Tag.ToString() == "Remove") 
            {
                PanelUserInput.Visible = false;
                SqlFunctionsExecute.ExecuteSQL("DELETE FROM [dbo].[Login] WHERE [Username] = '" + DataGridViewLogin.Rows[UserID].Cells[0].Value.ToString() + "'");
                RefreshLoginDataGrid();
            }
            else if (UserOperationButton.Tag.ToString() == "Add")
            {
                ClearUserInput();
            }
            SelectedOperation = UserOperationButton.Tag.ToString();
        }

        private void DataGridViewLogin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                UserID = e.RowIndex;
                txtUsername.Text = DataGridViewLogin.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtPassword.Text = DataGridViewLogin.Rows[e.RowIndex].Cells[1].Value.ToString();

                if (DataGridViewLogin.Rows[e.RowIndex].Cells[2].Value.ToString().Trim() == "Admin")
                {
                    SwitchUserType.Checked = true;
                }
                else
                {
                    SwitchUserType.Checked = false;
                    //txtEstimator.SelectedItem = DataGridViewLogin.Rows[e.RowIndex].Cells[3].Value.ToString().Trim();
                }
                btnAddUser.Checked = btnChangeUsername.Checked = false;
                PanelUserInput.Visible = false;
            }
        }

        private void SwitchUserType_CheckedChanged(object sender, EventArgs e)
        {
            if (SwitchUserType.Checked)
            {
                UserType = "Admin";
                txtEstimator.Enabled = false;
            }
            else
            {
                txtEstimator.Enabled = true;
                UserType = "User";
            }
        }

        /// <summary>
        /// Edit Page #Page no 3
        /// </summary>
        int SelectedRowNo = 0;
        string SelectedTable = "";

        private void EditDataSelected(object sender, EventArgs e)
        {
            Guna2Button EditDataNavigationButton = (Guna2Button)sender;            
            SelectedTable = EditDataNavigationButton.Tag.ToString();
            RefreshEditDataDataGrid(); 
            if (EditDataNavigationButton.Tag.ToString() == btnEditDataEst.Tag.ToString())
            {
                DataGridViewEditData.Columns[2].HeaderText = "Section";
            }
        }

        private void EditDataOperationBtn(object sender, EventArgs e)
        {
            Guna2Button btnEditDataControl = (Guna2Button)sender;
            SelectedOperation = btnEditDataControl.Tag.ToString();            

            if (btnEditDataControl.Tag.ToString()== "Remove")
            {
                if (!(SqlFunctionsExecute.ExecuteSQL("UPDATE " + SelectedTable + " SET Active = 'No' " +
                    "WHERE " + DataGridViewEditData.Columns[0].Name + "='"
                    + DataGridViewEditData.Rows[SelectedRowNo].Cells[0].Value.ToString() + "'; "))) 
                {
                    SqlFunctionsRead.ErrorMessage();
                }
                RefreshEditDataDataGrid();
            }
            else
            {
                ClearEditDataInputs();
                PanelPerson.Enabled = PanelEst.Enabled = PanelEH.Enabled = true;

                if (btnEditDataControl.Tag.ToString() == "Change")
                {
                    switch (SelectedTable)
                    {
                        case "ExpenditureHead":
                            {
                                txtEHMain.Text = DataGridViewEditData.Rows[SelectedRowNo].Cells[1].Value.ToString();
                                txtEHSub1.Text = DataGridViewEditData.Rows[SelectedRowNo].Cells[2].Value.ToString();
                                txtEHSub2.Text = DataGridViewEditData.Rows[SelectedRowNo].Cells[3].Value.ToString();
                                txtEHSub3.Text = DataGridViewEditData.Rows[SelectedRowNo].Cells[4].Value.ToString();
                                txtDescription.Text = DataGridViewEditData.Rows[SelectedRowNo].Cells[5].Value.ToString();
                                break;
                            }
                        case "Estimator":
                            {
                                txtEditDataEstName.Text = DataGridViewEditData.Rows[SelectedRowNo].Cells[1].Value.ToString();
                                txtEditDataEstZone.Text = DataGridViewEditData.Rows[SelectedRowNo].Cells[2].Value.ToString();
                                break;
                            }
                        case "Person":
                            {
                                txtEditDataPersonName.Text = DataGridViewEditData.Rows[SelectedRowNo].Cells[1].Value.ToString();                                
                                txtEditDataJob.Text = DataGridViewEditData.Rows[SelectedRowNo].Cells[2].Value.ToString();
                                break;
                            }
                    }
                }
                btnEditDataEH.Enabled = btnEditDataPerson.Enabled = btnEditDataEst.Enabled = btnEditDataAddNew.Enabled =
                btnEditDataRemove.Enabled = btnEditDataChange.Enabled = false;
            }
        }

        private void DataGridViewEditData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRowNo = e.RowIndex;
        }

        void RefreshEditDataDataGrid()
        {
            PanelEH.Visible = PanelEst.Visible = PanelPerson.Visible = false;
            PanelPerson.Enabled = PanelEst.Enabled = PanelEH.Enabled = false;
            SelectedRowNo = 0;
            btnEditDataAddNew.Checked = btnEditDataRemove.Checked = btnEditDataChange.Checked = false;

            switch (SelectedTable)
            {
                case "ExpenditureHead":
                    {
                        PanelEH.Visible = true;

                        DataGridViewEditData.Columns.Clear();
                        DataGridViewEditData.Columns.Add("EHID", "EHID");
                        DataGridViewEditData.Columns.Add("EH_Main", "EH Main");
                        DataGridViewEditData.Columns.Add("EH_Sub1", "EH Sub1");
                        DataGridViewEditData.Columns.Add("EH_Sub2", "EH Sub2");
                        DataGridViewEditData.Columns.Add("EH_Sub3", "EH Sub3");
                        DataGridViewEditData.Columns.Add("ExpenditureHeadDiscription", "Description");

                        if (!(SqlFunctionsRead.RetrieveDataTable("SELECT * FROM ExpenditureHead WHERE Active='Yes'", DataGridViewEditData)))
                        {
                            SqlFunctionsRead.ErrorMessage();
                        }
                        else
                        {
                            DataGridViewEditData.Columns[1].FillWeight = DataGridViewEditData.Columns[2].FillWeight =
                                DataGridViewEditData.Columns[3].FillWeight = DataGridViewEditData.Columns[4].FillWeight = 20;
                            DataGridViewEditData.Columns[5].FillWeight = 100;
                        }

                        break;
                    }
                case "Estimator":
                    {
                        PanelEst.Visible = true;
                        PanelEst.Location = PanelEH.Location;

                        DataGridViewEditData.Columns.Clear();
                        DataGridViewEditData.Columns.Add("EstimatorNO", "EstimatorNO");
                        DataGridViewEditData.Columns.Add("EstName", "Estimator Name");
                        DataGridViewEditData.Columns.Add("ZoneNo", "Zone No");

                        if (!(SqlFunctionsRead.RetrieveDataTable("SELECT * FROM Estimator WHERE Active='Yes'", DataGridViewEditData)))
                        {
                            SqlFunctionsRead.ErrorMessage();
                        }
                        else
                        {
                            DataGridViewEditData.Columns[1].FillWeight = DataGridViewEditData.Columns[2].FillWeight = 50;
                        }

                        break;
                    }
                case "Person":
                    {
                        PanelPerson.Visible = true;
                        PanelPerson.Location = PanelEH.Location;

                        DataGridViewEditData.Columns.Clear();
                        DataGridViewEditData.Columns.Add("PID", "PID");
                        DataGridViewEditData.Columns.Add("Name", "Name");
                        DataGridViewEditData.Columns.Add("Job Title", "Job Title");

                        if (!(SqlFunctionsRead.RetrieveDataTable("SELECT * FROM Person WHERE Active='Yes'", DataGridViewEditData)))
                        {
                            SqlFunctionsRead.ErrorMessage();
                        }
                        else
                        {
                            DataGridViewEditData.Columns[1].FillWeight = DataGridViewEditData.Columns[2].FillWeight = 50;
                        }

                        break;
                    }
            }
            DataGridViewEditData.Columns[0].Visible = false;
        }

        private void btnCancel(object sender, EventArgs e)
        {
            btnEditDataEH.Enabled = btnEditDataPerson.Enabled = btnEditDataEst.Enabled = btnEditDataAddNew.Enabled =
                btnEditDataRemove.Enabled = btnEditDataChange.Enabled = true;
            RefreshEditDataDataGrid();
        }

        private void btnEditDataAdd(object sender, EventArgs e)
        {
            btnEditDataEH.Enabled = btnEditDataPerson.Enabled = btnEditDataEst.Enabled = btnEditDataAddNew.Enabled = 
                btnEditDataRemove.Enabled = btnEditDataChange.Enabled = true;

            if (SelectedOperation == "Change")
            {
                string UpdateQuery = "UPDATE " + SelectedTable + " SET ";

                switch (SelectedTable)
                {
                    case "ExpenditureHead":
                        {
                            UpdateQuery += "EH_Main='" + txtEHMain.Text + "', EH_Sub1='" + txtEHSub1.Text + "', EH_Sub2='" + txtEHSub2.Text + "'," +
                                " EH_Sub3='" + txtEHSub3.Text + "', ExpenditureHeadDiscription=N'" + txtDescription.Text + "' ";

                            break;
                        }
                    case "Estimator":
                        {
                            UpdateQuery += "EstName=N'" + txtEditDataEstName.Text + "', ZoneNo=N'" + txtEditDataEstZone.Text + "' ";
                            break;
                        }
                    case "Person":
                        {

                            UpdateQuery += "[Name]=N'" + txtEditDataPersonName.Text + "', [Job Title]=N'" + txtEditDataJob.Text + "' ";
                            break;
                        }
                }

                UpdateQuery += "WHERE " + DataGridViewEditData.Columns[0].Name + "='"
                    + DataGridViewEditData.Rows[SelectedRowNo].Cells[0].Value.ToString() + "'; ";

                if (!(SqlFunctionsExecute.ExecuteSQL(UpdateQuery)))
                {
                    SqlFunctionsRead.ErrorMessage();
                }
            }
            else
            {
                string CheckActivation = "SELECT Active From " + SelectedTable + " WHERE ";
                string ActiveAgain = "UPDATE " + SelectedTable + " SET Active='Yes' WHERE ";

                string MaxOfPrimaryKey = SqlFunctionsRead.SQLRead("SELECT MAX(" + DataGridViewEditData.Columns[0].Name + ") " +
                    "AS MaxOfPK FROM " + SelectedTable, "MaxOfPK")[0];
                string InsertQuery = "INSERT INTO [dbo].[" + SelectedTable + "] (";

                for (var i = 0; i < DataGridViewEditData.ColumnCount; i++) 
                {
                    InsertQuery += "[" + DataGridViewEditData.Columns[i].Name + "], ";
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
                                + txtEditDataEstName.Text + "', '" + txtEditDataEstZone.Text + "', 'Yes')";

                            EditDataInputs = "EstName=N'" + txtEditDataEstName.Text + "' AND ZoneNo='" + txtEditDataEstZone.Text + "'";
                            break;
                        }
                    case "Person":
                        {
                            InsertQuery += "'" + (Convert.ToInt32(MaxOfPrimaryKey) + 1).ToString() + "', N'"
                                + txtEditDataPersonName.Text + "', N'" + txtEditDataJob.Text + "', 'Yes')";

                            EditDataInputs = "Name=N'" + txtEditDataPersonName.Text + "' AND [Job Title]=N'" + txtEditDataJob.Text + "'";
                            break;
                        }
                }

                CheckActivation += EditDataInputs;
                ActiveAgain += EditDataInputs;

                if (SqlFunctionsRead.SQLRead(CheckActivation, "Active") == null)
                {
                    if (!(SqlFunctionsExecute.ExecuteSQL(InsertQuery)))
                    {
                        SqlFunctionsRead.ErrorMessage();
                    }
                }
                else
                {
                    if (!(SqlFunctionsExecute.ExecuteSQL(ActiveAgain)))
                    {
                        SqlFunctionsRead.ErrorMessage();
                    }
                }
            }

            ClearEditDataInputs();
            RefreshEditDataDataGrid();
        }

        void ClearEditDataInputs()
        {
            txtEditDataJob.Text = txtEditDataPersonName.Text = txtEditDataEstName.Text = txtEditDataEstZone.Text =
                txtEHMain.Text = txtEHSub1.Text = txtEHSub2.Text = txtEHSub3.Text = txtDescription.Text = "";
        }

        /// <summary>
        /// Main
        /// </summary>

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
