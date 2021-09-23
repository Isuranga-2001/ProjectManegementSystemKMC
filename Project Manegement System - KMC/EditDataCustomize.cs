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
    public partial class EditDataCustomize : Form
    {
        public EditDataCustomize()
        {
            InitializeComponent();
        }

        public string ProjectID = null, EHMain = null, EHSub1 = null, EHSub2 = null, EHSub3 = null,
            Person = null, MOE = null, Permission = null, StartDate = null, EstimatedMoney = null,
            Estimator = null, Description = null;       

        SQLFunctions sqlFunctions = new SQLFunctions();
        AddNew addNewSqlFunctions = new AddNew();

        private void btnEHSelect_Click(object sender, EventArgs e)
        {
            SelectExpenditureHead form = new SelectExpenditureHead();
            form.ShowDialog();
            try
            {
                string EH = addNewSqlFunctions.SQLRead("SELECT EH_Main,EH_Sub1,EH_Sub2,EH_Sub3 FROM dbo.ExpenditureHead WHERE (EHID='"
               + Properties.Settings.Default.SelectedEHID + "')", "string", "EH_Main EH_Sub1 EH_Sub2 EH_Sub3");

                string[] EHParts = EH.Split();
                if (EHParts[3] == null)
                {
                    txtEH.Text = EHParts[0] + " - " + EHParts[1] + " - " + EHParts[2];
                }
                else
                {
                    txtEH.Text = EHParts[0] + " - " + EHParts[1] + " - " + EHParts[2] + " - " + EHParts[3];
                    EHSub3 = EHParts[3];
                }
                EHMain = EHParts[0];
                EHSub1 = EHParts[1];
                EHSub2 = EHParts[2];
            }
            catch
            {
                txtEH.Text = "";
            }

            if (txtEH.Text[txtEH.Text.Count() - 1] == ' ' || txtEH.Text[txtEH.Text.Count() - 2] == '-' || txtEH.Text[txtEH.Text.Count() - 3] == ' ')
            {
                txtEH.Text = txtEH.Text.Remove(txtEH.Text.Length - 3);
            }            
        }

        private void EditDataCustomize_Load(object sender, EventArgs e)
        {            
            txtDate.ResetText();
            txtProjectID.Text = ProjectID;            

            txtName.Items.Clear();
            List<string> PersonNameList = sqlFunctions.SQLRead("SELECT Name FROM dbo.Person WHERE Active = 'Yes'", "Name");
            foreach (var Item in PersonNameList)
            {
                txtName.Items.Add(Item);
            }
            txtName.Items.Add("Other");

            txtEstimator.Items.Clear();
            List<string> EstimatorNameList = sqlFunctions.SQLRead("SELECT EstName FROM dbo.Estimator WHERE Active = 'Yes'", "EstName");
            foreach (var Item in EstimatorNameList)
            {
                txtEstimator.Items.Add(Item);
            }
            txtEstimator.Items.Add("Other");

            txtPermission.Items.Clear();
            string[] PermissionMethodList = { "Chief Engineer", "Commissioner", "Council" };
            txtPermission.Items.AddRange(PermissionMethodList);

            txtMOE.Items.Clear();
            string[] MOEMethodList = { "Society", "Department", "Tenderer", "Special Projects" };
            txtMOE.Items.AddRange(MOEMethodList);

            txtDescription.Text = Description;
            txtDate.Text = StartDate;
            txtName.SelectedItem = Person;
            txtMOE.SelectedItem = MOE;
            txtPermission.SelectedItem = Permission;
            txtEstimatedMoney.Text = EstimatedMoney;
            txtEstimator.SelectedItem = Estimator;

            if (txtName.Text == "")
            {
                MessageBox.Show("Proposer is no longer available now, Enter Another one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.SelectedIndex = 0;
            }
            if (txtEstimator.Text == "")
            {
                MessageBox.Show("Estimator is no longer available now, Enter Another one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEstimator.SelectedIndex = 0;
            }
        }

        private void txtEH_TextChanged(object sender, EventArgs e)
        {
            string[] EHParts = txtEH.Text.Split('-');            
            try
            {
                EHMain = EHParts[0].Trim();
                EHSub1 = EHParts[1].Trim();
                EHSub2 = EHParts[2].Trim();
                EHSub3 = EHParts[3].Trim();
            }
            catch
            {
                EHSub3 = "";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProjectID.Text != "" && txtName.SelectedIndex != -1 && txtEstimator.SelectedIndex != -1 && txtEstimatedMoney.Text != ""
                    && txtEH.Text != "" && txtDescription.Text != "" && txtPermission.SelectedIndex != -1 && txtMOE.SelectedIndex != -1)
                {
                    string EstNo = sqlFunctions.SQLRead("SELECT [EstimatorNO] FROM [dbo].[Estimator] WHERE [EstName]=N'" + txtEstimator.Text + "'", "EstimatorNO")[0];

                    string EHID = sqlFunctions.SQLRead("SELECT [EHID] FROM [dbo].[ExpenditureHead] " +
                        "WHERE ([EH_Main]='" + EHMain + "' AND [EH_Sub1]='" + EHSub1 + "' AND [EH_Sub2]='" + EHSub2 + "' AND ([EH_Sub3]='" + EHSub3 + "' OR [EH_Sub3] IS Null))", "EHID")[0];

                    string PID = sqlFunctions.SQLRead("SELECT [PID] FROM [dbo].[Person] WHERE [Name]=N'" + txtName.Text + "'", "PID")[0];

                    if (addNewSqlFunctions.ExecuteSQL("UPDATE [dbo].[Project] SET [Discription] = N'" + txtDescription.Text.Trim() + "', " +
                        "[EstimatedMoney] = '" + txtEstimatedMoney.Text + "', [Permission] = N'" + txtPermission.Text + "', " +
                        "[StartDate] = N'" + txtDate.Text + "', [MethodOfExecution] = N'" + txtMOE.Text + "', [EstimatorNO] = '" + EstNo + "' " +
                        "WHERE ([ProjectID] = " + txtProjectID.Text + ");"))
                    {
                        if (addNewSqlFunctions.ExecuteSQL("UPDATE [dbo].[ExpenditureHeadProject] SET [EHID] = '" + EHID + "' " +
                            "WHERE ([ProjectID] = '" + txtProjectID.Text + "');"))
                        {
                            if (addNewSqlFunctions.ExecuteSQL("UPDATE [dbo].[PersonProject] SET [PID] = '" + PID + "' " +
                                "WHERE ([ProjectID] = '" + txtProjectID.Text + "');"))
                            {
                                MessageBox.Show("Updated Successfully !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                Error();
                            }
                        }
                        else
                        {
                            Error();
                        }
                    }
                    else
                    {
                        Error();
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please Enter All Required Fields !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                Error();
            }
        }

        private void txtEstimatedMoney_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox textBox = (Guna2TextBox)sender;
            if (textBox.Name == txtEstimatedMoney.Name)
            {
                char[] originalText = textBox.Text.ToCharArray();
                foreach (char c in originalText)
                {
                    if (c == '.')
                        continue;
                    else if (!(Char.IsNumber(c)))
                    {
                        textBox.Text = textBox.Text.Remove(textBox.Text.IndexOf(c));
                    }
                }
                textBox.Select(textBox.Text.Length, 0);
            }
        }

        void Error()
        {
            MessageBox.Show("Something Went Wrong!, Can't Update Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
