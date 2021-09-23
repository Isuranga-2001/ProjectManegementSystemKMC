using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Project_Manegement_System___KMC
{
    public partial class EditData : Form
    {
        public EditData()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        AddNew sqlExecuteFunctions = new AddNew();
        int SelectedRow = 0, SelectedColumn = 0;
        public string SelectAllQuery = "SELECT Project.ProjectID, Project.Discription, Project.EstimatedMoney, Project.Permission,Project.MethodOfExecution, " +
                "Project.StartDate, Estimator.EstName FROM Project,Estimator WHERE Project.EstimatorNo = Estimator.EstimatorNo";

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            DashBoard form = new DashBoard();
            form.Show();
            this.Close();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew form = new AddNew();
            form.Show();
            this.Hide();
        }

        private void btnProgress_Click(object sender, EventArgs e)
        {
            Progress form = new Progress();
            form.Show();
            this.Hide();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter form = new Filter();
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

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string SelectedRowProjectID = TableResults.Rows[SelectedRow].Cells[0].Value.ToString();

            if (MessageBox.Show("Do you want to delete " + SelectedRowProjectID, "Information", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (MessageBox.Show("You can't restore " + SelectedRowProjectID + " again !", "Information", 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    sqlExecuteFunctions.ExecuteSQL("DELETE FROM [dbo].[ExpenditureHeadProject] " +
                        "WHERE ([ProjectID] = " + SelectedRowProjectID + ")");

                    sqlExecuteFunctions.ExecuteSQL("DELETE FROM [dbo].[ProgressOfProject] " +
                        "WHERE ([ProjectID] = " + SelectedRowProjectID + ")");

                    sqlExecuteFunctions.ExecuteSQL("DELETE FROM [dbo].[PersonProject] " +
                        "WHERE ([ProjectID] = " + SelectedRowProjectID + ")");

                    sqlExecuteFunctions.ExecuteSQL("DELETE FROM [dbo].[Project] " +
                        "WHERE ([ProjectID] = " + SelectedRowProjectID + ")");

                    RefreshTable(SelectAllQuery);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
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
        }

        private void EditData_Load(object sender, EventArgs e)
        {
            RefreshTable(SelectAllQuery);/*
            OleDbEnumerator enumerator = new OleDbEnumerator();
            TableResults.DataSource = enumerator.GetElements();
            foreach (DataGridViewColumn col in TableResults.Columns)
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;*/
        }

        private void TableResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedColumn = e.ColumnIndex;
            SelectedRow = e.RowIndex;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            sqlFunctions.SignOut(this);
        }

        private void btnProgressHistory_Click(object sender, EventArgs e)
        {
            ProgressHistory form = new ProgressHistory();
            form.ShowDialog();
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
            this.Close();
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            RefreshTable(SelectAllQuery);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() != "")
            {
                RefreshTable("SELECT Project.ProjectID, Project.Discription, Project.EstimatedMoney, " +
                    "Project.Permission,Project.MethodOfExecution, Project.StartDate, Estimator.EstName " +
                    "FROM Project,Estimator WHERE Project.EstimatorNo = Estimator.EstimatorNo AND " +
                    "Project.Discription Like N'%" + txtSearch.Text.Trim() + "%'");
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
                List<string> ExpenditureHead = null;
                int NoOfRows = TableResults.RowCount - 1;
                string ProjectID = null;

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

                List<string> PerosnName = null;
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
        }
    }
}
