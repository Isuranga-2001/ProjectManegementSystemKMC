using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Guna.UI2.WinForms;

namespace Project_Manegement_System___KMC
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        int ComplitedProjects = 0, NotStartedProjects = 0, NoComplitedProjects = 0;
        EasyMode FilterForm;

        private void btnAddData_Click(object sender, EventArgs e)
        {
            AddNew form = new AddNew();
            form.Show();
            this.Close();
        }
                
        private void btnProgress_Click(object sender, EventArgs e)
        {
            Progress form = new Progress();
            form.Show();
            this.Close();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.User == "User")
            {
                guna2ImageButton2.Enabled = guna2ImageButton3.Enabled = btnEditData.Enabled = btnProgressHistory.Enabled = guna2ImageButton5.Enabled = false;
            }
            try
            {
                CreateChartProgressByPresentageChart();
                CreateChartSummeryOFProjectProgress();
                CreateChartSummeryOFProjectByZone();
                CreateProjectSummeryDeatils();
                CreateChartSummeryOFProjectByMOE();
                ChartCompleted.Value = ChartNotCompleted.Value = ChartNotStarted.Value = 0;
                TimerSetCurrentProgress.Enabled = true;
            }
            catch
            {
                lblTotalMoney.Text = lblNoP.Text = "0";
                ChartCompleted.Value = ChartNotCompleted.Value = ChartNotStarted.Value = 0;
            }
        }

        void CreateProjectSummeryDeatils()
        {
            string Month = DateTime.Now.Month.ToString(), Day = DateTime.Now.Day.ToString();
            if (Month.Length < 2)
                Month = "0" + Month;
            if (Day.Length < 2)
                Day = "0" + Day;

            lblNoP.Text = sqlFunctions.SQLRead("SELECT COUNT(ProjectID) AS NoOfProjects FROM dbo.Project WHERE ProjectID LIKE '"
                + DateTime.Now.Year.ToString()[2].ToString() + DateTime.Now.Year.ToString()[3].ToString() + "%'", "NoOfProjects")[0];

            string TotalMoney = (Math.Round(Convert.ToDouble(sqlFunctions.SQLRead("SELECT Sum(EstimatedMoney) AS TotalMoney FROM dbo.Project WHERE ProjectID LIKE '"
                + DateTime.Now.Year.ToString()[2].ToString() + DateTime.Now.Year.ToString()[3].ToString() + "%'", "TotalMoney")[0]), 0)).ToString();

            string TotalMoneyAfterSplit = "";
            int Count = TotalMoney.Length / 3;
            for (var i = 0; i < Count; i++) 
            {
                TotalMoneyAfterSplit = "," + TotalMoney.Substring(TotalMoney.Length - 3)+ TotalMoneyAfterSplit;
                TotalMoney = TotalMoney.Remove(TotalMoney.Length - 3);
            }
            TotalMoneyAfterSplit = TotalMoney + TotalMoneyAfterSplit;
            if (TotalMoneyAfterSplit[0] == ',')
            {
                TotalMoneyAfterSplit = TotalMoneyAfterSplit.Remove(0, 1);
            }
            lblTotalMoney.Text = "Rs. " + TotalMoneyAfterSplit + " /=";

            lblDateTotalMoney.Text = lblDateNoP.Text = "From " + DateTime.Now.Year.ToString() + "-01-01 To " + DateTime.Now.Year.ToString() + "-" + Month + "-" + Day;
            lblTitleYearNoP.Text = "Total Number of Projects in " + DateTime.Now.Year.ToString();
            lblTitleYearTotalMoney.Text = "Total Extimated Money " + DateTime.Now.Year.ToString();
        }

        void CreateChartSummeryOFProjectByZone()
        {
            int NoOFZones = Convert.ToInt32(sqlFunctions.SQLRead("SELECT COUNT(DISTINCT ZoneNo) AS NoOFZones " +
                "FROM dbo.Estimator", "NoOFZones")[0]);

            List<string> Zones = sqlFunctions.SQLRead("SELECT DISTINCT ZoneNo FROM dbo.Estimator WHERE Active='Yes'", "ZoneNo");
            List<List<string>> ProjectsOfZones = new List<List<string>> { };
            Dictionary<string, List<int>> ProgressOfZone = new Dictionary<string, List<int>> { };

            foreach (var item in Zones)
            {
                try
                {
                    ProjectsOfZones.Add(sqlFunctions.SQLRead("SELECT Project.ProjectID " +
                    "FROM Project INNER JOIN Estimator ON Project.EstimatorNO = Estimator.EstimatorNO " +
                    "WHERE(Estimator.ZoneNo = '" + item + "') " +
                    "AND (Project.ProjectID LIKE '" + DateTime.Now.Year.ToString()[2].ToString() +
                    DateTime.Now.Year.ToString()[3].ToString() + "%')", "ProjectID"));
                }
                catch
                {
                    continue;
                }
            }

            int i = 0;
            foreach (var ProjectList in ProjectsOfZones)
            {
                if (ProjectList == null)
                    continue;

                List<string> PresentageList = new List<string> { };

                foreach (var ProjectID in ProjectList)
                {
                    if ("20" + ProjectID[0].ToString() + ProjectID[1].ToString() == DateTime.Now.Year.ToString())
                    {
                        PresentageList.Add(sqlFunctions.SQLRead("SELECT MAX(Progress) AS LastProgress " +
                            "FROM dbo.ProgressOfProject WHERE ProjectID='" + ProjectID + "'", "LastProgress")[0]);
                    }
                }                

                ProgressOfZone.Add("Zone " + Zones[i], Count(PresentageList));
                i += 1;
            }

            List<int> Count(List<string> ItemList)
            {
                List<int> count = new List<int> { 0, 0, 0 };

                foreach (var Item in ItemList)
                {
                    if (Item == "")
                    {
                        count[1] += 1;
                        continue;
                    }
                        
                    int Value = Convert.ToInt32(Item);

                    if (Value == 0)
                    {
                        count[1] += 1;
                    }
                    else if (Value <= 99)
                    {
                        count[0] += 1;
                    }
                    else
                    {
                        count[2] += 1;
                    }
                }

                return count;
            }

            if (ProgressOfZone.Count != NoOFZones)
            {
                for (var j = 0; j < Zones.Count; j++)
                {
                    List<int> value = new List<int> { 0, 0, 0 };
                    try
                    {
                        if (ProgressOfZone["Zone " + Zones[j]] == value) 
                        {
                            continue;
                        }
                    }
                    catch
                    {
                        if (!(Char.IsNumber(Zones[j][0])))
                        {
                            continue;
                        }
                        ProgressOfZone.Add("Zone " + (j + 1).ToString(), new List<int> { 0, 0, 0 });
                    }
                }
            }

            for (var j = 0; j < ProgressOfZone.Count; j++)
            {
                for (var k = 0; k < 3; k++)
                {
                    try
                    {
                        if (!(Char.IsNumber(Zones[j][0])))
                        {
                            ChartSummeryOFProjectByZone.Series[k].Points.AddXY(Zones[j], ProgressOfZone["Zone " + Zones[j]][k]);
                        }
                        else
                        {
                            ChartSummeryOFProjectByZone.Series[k].Points.AddXY("Zone " + Zones[j], ProgressOfZone["Zone " + Zones[j]][k]);
                        }
                    }
                    catch
                    {
                        if (!(Char.IsNumber(Zones[j][0])))
                        {
                            ChartSummeryOFProjectByZone.Series[k].Points.AddXY("Zone " + Zones[j], 0);
                            MessageBox.Show("Not even a project assign to Zone" + Zones[j] + ". Then Can't Update dashboard.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            ChartSummeryOFProjectByZone.Series[k].Points.AddXY(Zones[j], 0);
                            MessageBox.Show("Not even a project assign to " + Zones[j] + ". Then Can't Update dashboard.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;
                    }
                }
            }

            lblTitleZoneChart.Text = "Projects by Section " + DateTime.Now.Year.ToString();

            ChartSummeryOFProjectByZone.Series[0].IsValueShownAsLabel = ChartSummeryOFProjectByZone.Series[1].IsValueShownAsLabel = 
                ChartSummeryOFProjectByZone.Series[2].IsValueShownAsLabel = true;
            ChartSummeryOFProjectByZone.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            ChartSummeryOFProjectByZone.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
        }

        void CreateChartSummeryOFProjectProgress()
        {
            NoComplitedProjects = Convert.ToInt32(sqlFunctions.SQLRead("SELECT COUNT(DISTINCT ProjectID) AS NotStartedProjects FROM ProgressOfProject " +
                "WHERE (ProjectID LIKE '" + DateTime.Now.Year.ToString()[2].ToString() + DateTime.Now.Year.ToString()[3].ToString() + "%')",
                "NotStartedProjects")[0]) - ComplitedProjects - NotStartedProjects;

            int Total = Convert.ToInt32(ComplitedProjects) + Convert.ToInt32(NoComplitedProjects) + Convert.ToInt32(NotStartedProjects);
            if (Total > 0)
            {
                ComplitedProjects = Convert.ToInt32(Math.Round(Convert.ToDouble(ComplitedProjects) / Total * 100));
                NotStartedProjects = Convert.ToInt32(Math.Round(Convert.ToDouble(NotStartedProjects) / Total * 100));
                NoComplitedProjects = Convert.ToInt32(Math.Round(Convert.ToDouble(NoComplitedProjects) / Total * 100));
            }

            lblTitleChart.Text = "<Center> Current Situation of Projects <br> by Progress " + DateTime.Now.Year.ToString();
        }
                
        void CreateChartProgressByPresentageChart()
        {
            List<string> ProjectList = sqlFunctions.SQLRead("SELECT DISTINCT ProjectID AS StartedProjects FROM dbo.ProgressOfProject", "StartedProjects");
            List<string> PresentageList = new List<string> { };

            foreach (var ProjectID in ProjectList)
            {
                if ("20" + ProjectID[0].ToString() + ProjectID[1].ToString() == DateTime.Now.Year.ToString()) 
                {
                    PresentageList.Add(sqlFunctions.SQLRead("SELECT MAX(Progress) AS LastProgress FROM dbo.ProgressOfProject WHERE ProjectID='" + ProjectID + "'", "LastProgress")[0]);
                }
            }

            List<int> PresentageCountList = Count(PresentageList);
            List<string> SeriesList = new List<string> { "1% - 20%", "21% - 40%", "41% - 60%", "61% - 80%", "81% - 99%", "100%" };
            ChartProgressByPresentage.Series.Clear();

            Series ProgressByPresentage = ChartProgressByPresentage.Series.Add("ProgressByPresentage");
            ProgressByPresentage.ChartType = SeriesChartType.Doughnut;
            for (int i = 0; i < PresentageCountList.Count; i++) 
            {                
                ProgressByPresentage.Points.AddXY(SeriesList[i], PresentageCountList[i]);
            }

            ProgressByPresentage.IsValueShownAsLabel = true;
            ChartProgressByPresentage.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            ChartProgressByPresentage.ChartAreas[0].AxisX.MinorGrid.Enabled = false;

            ChartProgressByPresentage.Series[0].Points[0].Color = Color.OrangeRed;
            ChartProgressByPresentage.Series[0].Points[1].Color = Color.Gold;
            ChartProgressByPresentage.Series[0].Points[2].Color = Color.Yellow;
            ChartProgressByPresentage.Series[0].Points[3].Color = Color.SpringGreen;
            ChartProgressByPresentage.Series[0].Points[4].Color = Color.LimeGreen;
            ChartProgressByPresentage.Series[0].Points[5].Color = Color.DodgerBlue;
        }

        void CreateChartSummeryOFProjectByMOE()
        {
            List<string> MOEList = sqlFunctions.SQLRead("SELECT MethodOfExecution FROM Project WHERE ProjectID LIKE '"
                + DateTime.Now.Year.ToString()[2].ToString() + DateTime.Now.Year.ToString()[3].ToString() + "%'", "MethodOfExecution");

            List<string> ProjectIDList = sqlFunctions.SQLRead("SELECT ProjectID FROM Project WHERE ProjectID LIKE '"
                + DateTime.Now.Year.ToString()[2].ToString() + DateTime.Now.Year.ToString()[3].ToString() + "%'", "ProjectID");

            Dictionary<string, List<int>> DicProgressStatusByMOEType = new Dictionary<string, List<int>>
            {
                { "Society", new List<int>{ 0, 0, 0 } }, { "Department", new List<int>{ 0, 0, 0 } },
                { "Tenderer", new List<int>{ 0, 0, 0 } }, { "Special Projects", new List<int>{ 0, 0, 0 } },
            };

            for (int i = 0; i < MOEList.Count; i++) 
            {
                UpdateMOEParts(ProjectIDList[i], DicProgressStatusByMOEType[MOEList[i]]);
            }

            void UpdateMOEParts(string ProjectID, List<int> ProgressList)
            {
                int LastProgress = Convert.ToInt32(sqlFunctions.SQLRead("SELECT MAX(Progress) AS LastProgess From ProgressOfProject " +
                    "WHERE (ProjectID = '" + ProjectID + "')", "LastProgess")[0]);

                if (LastProgress == 100)
                {
                    ProgressList[2] += 1;
                }
                else if (LastProgress == 0)
                {
                    ProgressList[1] += 1;
                }
                else
                {
                    ProgressList[0] += 1;
                }
            }           

            foreach (string MOEType in new List<string> { "Society", "Department", "Tenderer", "Special Projects" })
            {
                for (var k = 0; k < 3; k++)
                {
                    ChartProgressStatusByMOE.Series[k].Points.AddXY(MOEType, DicProgressStatusByMOEType[MOEType][k]);
                }
            }
            ChartProgressStatusByMOE.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            ChartProgressStatusByMOE.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            /*
            ChartProgressStatusByMOE.Series[0].IsValueShownAsLabel = ChartProgressStatusByMOE.Series[1].IsValueShownAsLabel = 
                ChartProgressStatusByMOE.Series[2].IsValueShownAsLabel = true;*/
        }

        List<int> Count(List<string> ItemList)
        {
            List<int> count = new List<int> { 0, 0, 0, 0, 0, 0 };

            foreach (var Item in ItemList)
            {
                int Value = Convert.ToInt32(Item);

                if (Value == 100)
                {
                    ComplitedProjects += 1;
                    count[5] += 1;
                    continue;
                }
                if (Value == 0)
                {
                    NotStartedProjects += 1;
                    continue;
                }

                if (Value <= 20)
                {
                    count[0] += 1;
                }
                else if (Value <= 40)
                {
                    count[1] += 1;
                }
                else if (Value <= 60)
                {
                    count[2] += 1;
                }
                else if (Value <= 80)
                {
                    count[3] += 1;
                }
                else
                {
                    count[4] += 1;
                }
            }

            return count;
        }

        private void DashBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            TimerSetCurrentProgress.Stop();
            TimerSetCurrentProgress.Enabled = false;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter form = new Filter();
            form.Show();
            this.Close();
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

        private void TimerSetCurrentProgress_Tick(object sender, EventArgs e)
        {
            if (ChartCompleted.Value < ComplitedProjects)
            {
                ChartCompleted.Value += 1;
                lblCompletedValue.Text = ChartCompleted.Value + "%";
            }
            if (ChartNotCompleted.Value < NoComplitedProjects)
            {
                ChartNotCompleted.Value += 1;
                lblNotCompletedValue.Text = ChartNotCompleted.Value + "%";
            }
            if (ChartNotStarted.Value < NotStartedProjects)
            {
                ChartNotStarted.Value += 1;
                lblNotStartedValue.Text = ChartNotStarted.Value + "%";
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings form = new Settings();
            form.BeforeLoad();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }

        private void OverallSummeryNotStartedElement_DoubleClick(object sender, EventArgs e)
        {
            FilterForm = new EasyMode();
            FilterForm.btnNotStarted.Checked = true;
            FilterForm.ShowDialog();
        }

        private void OverallSummeryCompletedElement_DoubleClick(object sender, EventArgs e)
        {
            FilterForm = new EasyMode();
            FilterForm.btnCompleted.Checked = true;
            FilterForm.ShowDialog();
        }

        private void OverallSummeryUserWorkElement_DoubleClick(object sender, EventArgs e)
        {
            FilterForm = new EasyMode();
            FilterForm.btnUnderWork.Checked = true;
            FilterForm.ShowDialog();
        }

        private void ChartProgressByPresentage_DoubleClick(object sender, EventArgs e)
        {
            FilterForm = new EasyMode();
            FilterForm.btnAll.Checked = true;
            FilterForm.ShowDialog();
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

        private void ChartProgressStatusByMOE_DoubleClick(object sender, EventArgs e)
        {
            FilterForm = new EasyMode();
            FilterForm.btnAll.Checked = true;
            FilterForm.btnMOEAll.Checked = true;
            FilterForm.ShowDialog();
        }

        private void ChartSummeryOFProjectByZone_DoubleClick(object sender, EventArgs e)
        {
            FilterForm = new EasyMode();
            FilterForm.btnAll.Checked = true;
            FilterForm.ShowDialog();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            sqlFunctions.SignOut(this);
        }
    }
}
