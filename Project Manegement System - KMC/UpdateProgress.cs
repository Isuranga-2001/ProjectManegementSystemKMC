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
using System.Data.SqlClient;

namespace Project_Manegement_System___KMC
{
    public partial class UpdateProgress : Form
    {
        public UpdateProgress()
        {
            InitializeComponent();
        }
        AddNew SQlWriteFunctions = new AddNew();
        Progress SqlReadFunction = new Progress();
        SQLFunctions sqlFunctions = new SQLFunctions();
        public int CurrentProgress = 0;

        private void UpdateProgress_Load(object sender, EventArgs e)
        {
            txtProgress.Value = Convert.ToInt32(TrackBarProgress.Value);
            txtProgressDate.ResetText();
            if (txtProjectID.Text != "")
            {
                SearchProjectProgress();
            }
            CurrentProgress = TrackBarProgress.Value;
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            AddNew form = new AddNew();
            form.Show();
            this.Hide();
        }

        private void TrackBarProgress_Scroll(object sender, ScrollEventArgs e)
        {
            if (TrackBarProgress.Value > CurrentProgress)
            {
                txtProgress.Value = Convert.ToInt32(TrackBarProgress.Value);
            }
            else
            {
                TrackBarProgress.Value = CurrentProgress;
            }
        }

        private void txtProgress_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtProgress.Value >= 0 && txtProgress.Value <= 100 && txtProgress.Value > CurrentProgress)
                {
                    TrackBarProgress.Value = Convert.ToInt32(txtProgress.Value);
                }
                else
                {
                    txtProgress.Value = Convert.ToInt32(TrackBarProgress.Value);
                }
            }
            catch
            {
                txtProgress.Value = Convert.ToInt32(TrackBarProgress.Value);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (TrackBarProgress.Value >= CurrentProgress)
            {
                if (SQlWriteFunctions.ExecuteSQL("INSERT INTO [dbo].[ProgressOfProject] ([ProjectID], [Progress], [Date], [Details]) " +
                    "VALUES ( '" + txtProjectID.Text + "', '" + Convert.ToInt32(txtProgress.Value).ToString() + "', N'" + txtProgressDate.Text + "', N'" + txtOther.Text + "')"))
                {
                    //SQlWriteFunctions.ExecuteSQL("DELETE FROM [dbo].[ProgressOfProject] WHERE ([ProjectID] = '" + Convert.ToInt32(txtProjectID.Text).ToString() + "') AND ([Progress] = '0')");
                    this.Close();
                }
            }            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtProgressDate.ResetText();
            TrackBarProgress.Value = CurrentProgress;
            txtProjectID.Clear();
            txtOther.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchProjectProgress();
        }

        void SearchProjectProgress()
        {
            TrackBarProgress.ResetText();
            try
            {
                string LastProgress = sqlFunctions.SQLRead("SELECT MAX(Progress) AS LastProgress FROM dbo.ProgressOfProject WHERE ProjectID='" + txtProjectID.Text + "'", "LastProgress")[0];
                List<string> ProjectProgressData = sqlFunctions.SQLRead("SELECT * FROM dbo.ProgressOfProject WHERE ProjectID='" + txtProjectID.Text + "' AND Progress='" + LastProgress + "'",
                    "Progress Date Details");

                bool IsNoProgressData = true;
                if (ProjectProgressData != null)
                {
                    for (var i = 0; i < ProjectProgressData.Count; i++)
                    {
                        if (ProjectProgressData[i] != "")
                            IsNoProgressData = false;
                    }
                }

                if (IsNoProgressData)
                {
                    txtProgress.Value = 0;
                    txtProgressDate.ResetText();
                }
                else
                {
                    txtProgress.Value = Convert.ToInt32(ProjectProgressData[ProjectProgressData.Count - 3]);
                    txtProgressDate.Text = ProjectProgressData[ProjectProgressData.Count - 2];
                    txtOther.Text = ProjectProgressData[ProjectProgressData.Count - 1];
                }
            }
            catch (Exception e)
            {
                CurrentProgress = -1;                
                txtProgress.Value = TrackBarProgress.Value = 0;
                txtProgressDate.ResetText();
                txtOther.Clear();
            }
            TrackBarProgress.Value = Convert.ToInt32(txtProgress.Value);
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            ProgressHistory form = new ProgressHistory();
            form.txtProjectID.Text = txtProjectID.Text;
            form.ShowDialog();
        }

        private void txtProgress_ValueChanged(object sender, EventArgs e)
        {
            if (txtProgress.Value > CurrentProgress)
            {
                TrackBarProgress.Value = Convert.ToInt32(txtProgress.Value);
            }
            else
            {
                txtProgress.Value = TrackBarProgress.Value = CurrentProgress;
            }
        }
    }
}
