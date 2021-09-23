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

namespace Project_Manegement_System_KMC_Water
{
    public partial class UpdateProgress : Form
    {
        public UpdateProgress()
        {
            InitializeComponent();
        }

        AddNew SQlWriteFunctions = new AddNew();
        SQLFunctions sqlFunctions = new SQLFunctions();
        MultiFunctions multiFunctions = new MultiFunctions();

        public int CurrentProgress = 0;
        public bool CloseParent = false;

        private void UpdateProgress_Load(object sender, EventArgs e)
        {
            if (txtProjectID.Text != "")
            {
                SearchProjectProgress();
            }
            else
            {
                ResetUpdatedDate();
            }

            txtProgress.Value = Convert.ToInt32(TrackBarProgress.Value);
            CurrentProgress = TrackBarProgress.Value;
        }

        void ResetUpdatedDate()
        {
            UpDownDateYear.Value = DateTime.Now.Year;
            UpDownDateMonth.Value = DateTime.Now.Month;
            UpDownDateDay.Value = DateTime.Now.Day;
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
            if (TrackBarProgress.Value >= CurrentProgress && CurrentProgress != 100)
            {
                if (SQlWriteFunctions.ExecuteSQL("INSERT INTO [dbo].[ProgressOfProject] ([ProjectID], [Progress], [Date], [Details]) " +
                    "VALUES ( '" + txtProjectID.Text + "', '" + Convert.ToInt32(txtProgress.Value).ToString() + "'," +
                    " N'" + UpDownDateYear.Value.ToString() + "-" + UpDownDateMonth.Value.ToString()
                    + "-" + UpDownDateDay.Value.ToString() + "', N'" + txtOther.Text + "')")) 
                {
                    this.Close();
                }
            }  
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetUpdatedDate();
            TrackBarProgress.Value = CurrentProgress;
            txtOther.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchProjectProgress();
        }

        public void SearchProjectProgress()
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
                    ResetUpdatedDate();
                }
                else
                {
                    txtProgress.Value = Convert.ToInt32(ProjectProgressData[ProjectProgressData.Count - 3]);

                    string[] LastUpdatedDate = ProjectProgressData[ProjectProgressData.Count - 2].Trim().Split('-');
                    UpDownDateYear.Value = Convert.ToInt32(LastUpdatedDate[0]);
                    UpDownDateMonth.Value = Convert.ToInt32(LastUpdatedDate[1]);
                    UpDownDateDay.Value = Convert.ToInt32(LastUpdatedDate[2]);

                    txtOther.Text = ProjectProgressData[ProjectProgressData.Count - 1];
                }
            }
            catch
            {
                CurrentProgress = -1;                
                txtProgress.Value = TrackBarProgress.Value = 0;
                ResetUpdatedDate();
                txtOther.Clear();
            }
            TrackBarProgress.Value = Convert.ToInt32(txtProgress.Value);
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            CloseParent = true;
            this.Close();
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

        private void UpDownDateMonth_ValueChanged(object sender, EventArgs e)
        {
            UpDownDateDay.Maximum = multiFunctions.NoOfDays(Convert.ToInt32(UpDownDateMonth.Value));
        }
    }
}
