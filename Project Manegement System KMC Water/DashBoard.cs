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

namespace Project_Manegement_System_KMC_Water
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        MultiFunctions multiFunctions = new MultiFunctions();

        DateTime txtDate = new DateTime();

        bool BudgetChartIsCumulative = false;
        
        private void DashBoard_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.User == "User")
            {
                //guna2ImageButton2.Enabled = guna2ImageButton3.Enabled = btnEditData.Enabled = btnProgressHistory.Enabled = guna2ImageButton5.Enabled = false;
            }

            try
            {
                CreateProjectSummeryDeatils();
                CreateChartSummeryOFProjectProgress();
                CreateChartSummeryOFProjectBySection();
                CreateChartProductionTimeLine();
                CreateChartProductionCostAndConsumptionIncome();
            }
            catch
            {
                lblTotalMoney.Text = lblNoP.Text = "0";
            }

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(1280, 720);
        }

        void CreateProjectSummeryDeatils() // details on top
        {
            try
            {
                lblNoP.Text = sqlFunctions.SQLRead("SELECT COUNT(ProjectID) AS NoOfProjects FROM dbo.Project " +
                    "WHERE StartDate LIKE '" + DateTime.Now.Year.ToString() + "-%'", "NoOfProjects")[0];
            }
            catch
            {
                lblNoP.Text = "0";
            }            

            try
            {
                lblTotalMoney.Text = "Rs. " + SplitNumber((Math.Round(Convert.ToDouble(sqlFunctions.SQLRead("SELECT SUM(EstimatedMoney) AS TotalMoney " +
                    "FROM dbo.Project WHERE StartDate LIKE '" + DateTime.Now.Year.ToString() + "-%'", "TotalMoney")[0]), 0)).ToString()) + " /=";
            }
            catch
            {
                lblTotalMoney.Text = "Rs. 0 /=";
            }

            try
            {
                lblTotalIncome.Text = "Rs. " + SplitNumber((Math.Round(Convert.ToDouble(sqlFunctions.SQLRead("SELECT SUM(MonthAmount) AS TotalIncome " +
                    "FROM Consumption WHERE Month LIKE '" + DateTime.Now.Year.ToString() + "-%'", "TotalIncome")[0]), 0)).ToString()) + " /=";
            }
            catch
            {
                lblTotalIncome.Text = "Rs. 0 /=";
            }

            try
            {
                lblTotalProductionCapacity.Text = "&nbsp; &nbsp;" + SplitNumber((Math.Round(Convert.ToDouble(sqlFunctions.SQLRead("SELECT SUM(Capacity) AS TotalCapacity " +
                    "FROM Production WHERE Date LIKE '" + DateTime.Now.Year.ToString() + "-%'", "TotalCapacity")[0]), 0)).ToString()) + "&nbsp; (m<sup>3</sup>)";
            }
            catch
            {
                lblTotalProductionCapacity.Text = "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 0 &nbsp; (m<sup>3</sup>)";
            }

            try
            {
                lblTotalConsumption.Text = "&nbsp; &nbsp;" + SplitNumber((Math.Round(Convert.ToDouble(sqlFunctions.SQLRead("SELECT SUM(Capacity) AS TotalCapacity " +
                    "FROM Consumption WHERE Month LIKE '" + DateTime.Now.Year.ToString() + "-%'", "TotalCapacity")[0]), 0)).ToString()) + "&nbsp; (m<sup>3</sup>)";
            }
            catch
            {
                lblTotalConsumption.Text = "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 0 &nbsp; (m<sup>3</sup>)";
            }

            string SplitNumber(string Number)
            {
                if (Number == "")
                    return "0";

                string NumberAfterSplit = "";
                int Count = Number.Length / 3;
                for (var i = 0; i < Count; i++)
                {
                    NumberAfterSplit = "," + Number.Substring(Number.Length - 3) + NumberAfterSplit;
                    Number = Number.Remove(Number.Length - 3);
                }
                NumberAfterSplit = Number + NumberAfterSplit;
                if (NumberAfterSplit[0] == ',')
                {
                    NumberAfterSplit = NumberAfterSplit.Remove(0, 1);
                }

                return NumberAfterSplit;
            }
        }

        void CreateChartSummeryOFProjectBySection()
        {
            int NoOFZones = Convert.ToInt32(sqlFunctions.SQLRead("SELECT COUNT(DISTINCT ZoneNo) AS NoOFZones " +
                "FROM dbo.Estimator", "NoOFZones")[0]);

            List<string> Zones = sqlFunctions.SQLRead("SELECT DISTINCT ZoneNo FROM dbo.Estimator", "ZoneNo");
            List<List<string>> ProjectsOfZones = new List<List<string>> { };
            Dictionary<string, List<int>> ProgressOfZone = new Dictionary<string, List<int>> { };

            foreach (var item in Zones)
            {
                try
                {
                    ProjectsOfZones.Add(sqlFunctions.SQLRead("SELECT Project.ProjectID " +
                    "FROM Project INNER JOIN Estimator ON Project.EstimatorNO = Estimator.EstimatorNO " +
                    "WHERE(Estimator.ZoneNo = '" + item + "') " +
                    "AND (Project.StartDate LIKE '" + DateTime.Now.Year.ToString() + "-%')", "ProjectID"));
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

            ChartSummeryOFProjectByZone.Series[0].IsValueShownAsLabel = ChartSummeryOFProjectByZone.Series[1].IsValueShownAsLabel = 
                ChartSummeryOFProjectByZone.Series[2].IsValueShownAsLabel = true;
        }

        void CreateChartSummeryOFProjectProgress()
        {
            try
            {
                int UnderWorkProjectCount = 0, CompletedProjectCount = 0, Total = 0;

                List<string> ReturnValue = sqlFunctions.SQLRead("SELECT MAX(Progress) AS NoOfProjects " +
                    "FROM ProgressOfProject WHERE Progress>0 AND Progress<100 GROUP BY ProjectID", "NoOfProjects");

                if (ReturnValue != null) 
                    UnderWorkProjectCount = ReturnValue.Count();

                ReturnValue = sqlFunctions.SQLRead("SELECT MAX(Progress) AS NoOfProjects " +
                    "FROM ProgressOfProject WHERE Progress=100 GROUP BY ProjectID", "NoOfProjects");

                if (ReturnValue != null)
                    CompletedProjectCount = ReturnValue.Count;

                ReturnValue = sqlFunctions.SQLRead("SELECT COUNT(DISTINCT ProjectID) AS NoOFProjects " +
                    "FROM ProgressOfProject", "NoOFProjects");

                if (ReturnValue != null) 
                    Total = Convert.ToInt32(ReturnValue[0]);

                ChartSummeryOFProjectProgress.Series[0].Points.Clear();

                if (Total > 0)
                {
                    ChartSummeryOFProjectProgress.Series[0].Points.Add(CompletedProjectCount);
                    ChartSummeryOFProjectProgress.Series[0].Points.Add(UnderWorkProjectCount);
                    ChartSummeryOFProjectProgress.Series[0].Points.Add(Total - (CompletedProjectCount + UnderWorkProjectCount));

                    ChartSummeryOFProjectProgress.Series[0].Points[0].ToolTip = "Completed Projects";
                    ChartSummeryOFProjectProgress.Series[0].Points[1].ToolTip = "Under Work Projects";
                    ChartSummeryOFProjectProgress.Series[0].Points[2].ToolTip = "Not Started Projects";
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong. Can't load 'Overroll Progress' Chart", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }            
        }

        void CreateChartProductionTimeLine() 
        {
            for (int j = 0; j < 4; j++)
                ChartProductionTimeLine.Series[j].Points.Clear();

            Dictionary<string, List<string>> ChartPoints = new Dictionary<string, List<string>> { };
            List<string> ReturnData;

            for (int i = -3; i < 0; i++)
            {
                txtDate = DateTime.Today.AddDays(i);
                ChartPoints.Add(SelectedDate(), new List<string> { "0", "0", "0", "0" });

                for (int j = 0; j < 4; j++)
                {
                    ReturnData = sqlFunctions.SQLRead("SELECT Capacity FROM Production " +
                        "WHERE PlantID='" + j + "' AND Date='" + SelectedDate() + "'", "Capacity");

                    if (ReturnData != null)
                    {
                        if (ReturnData[0] != "")
                        {
                            ChartPoints[SelectedDate()][j] = ReturnData[0];
                        }
                    }
                }
            }

            for (int j = 0; j < 4; j++)
            {
                foreach (KeyValuePair<string, List<string>> ChartPointIndex in ChartPoints)
                {
                    ChartProductionTimeLine.Series[j].Points.AddXY(ChartPointIndex.Key, ChartPointIndex.Value[j]);
                }

                ChartProductionTimeLine.Series[j].MarkerStyle = MarkerStyle.Circle;
            }
        }

        void CreateChartProductionCostAndConsumptionIncome()
        {
            ChartBudget.Series[0].Points.Clear();
            ChartBudget.Series[1].Points.Clear();

            List<string> VariationProductionCapacity;

            int StartMonth = DateTime.Now.Month * (-1);

            for (int j = StartMonth; j < 0; j++)
            {
                // Draw Production Series

                VariationProductionCapacity = sqlFunctions.SQLRead(
                    "SELECT SUM(Capacity) AS TotalProduction FROM Production " +
                    "WHERE Date LIKE '" + DateTime.Today.AddMonths(j).Year + "-" +
                    DateTime.Today.AddMonths(j).Month + "-%'", "TotalProduction");

                if (VariationProductionCapacity != null)
                {
                    if (VariationProductionCapacity[0].Trim() != "")
                    {
                        if (BudgetChartIsCumulative && ChartBudget.Series[0].Points.Count != 0)
                        {
                            ChartBudget.Series[0].Points.AddXY(
                                DateTime.Today.AddMonths(j).Year.ToString() + "-" + DateTime.Today.AddMonths(j).Month.ToString(),
                                Convert.ToDouble(ChartBudget.Series[0].Points[ChartBudget.Series[0].Points.Count - 1].YValues[0]) +
                                Convert.ToDouble(VariationProductionCapacity[0].Trim()));
                        }
                        else
                        {
                            ChartBudget.Series[0].Points.AddXY(
                                   DateTime.Today.AddMonths(j).Year.ToString() + "-" +
                                   DateTime.Today.AddMonths(j).Month.ToString(),
                                   Convert.ToDouble(VariationProductionCapacity[0].Trim()));
                        }
                    }
                    else
                        AddZeroIntoSeries(0, j);
                }
                else
                    AddZeroIntoSeries(0, j);

                // Draw Comsumption Series

                VariationProductionCapacity = sqlFunctions.SQLRead(
                    "SELECT SUM(Capacity) AS TotalConsumption FROM Consumption " +
                    "WHERE Month LIKE '" + DateTime.Today.AddMonths(j).Year + "-" +
                    DateTime.Today.AddMonths(j).Month + "%'", "TotalConsumption");

                if (VariationProductionCapacity != null)
                {
                    if (VariationProductionCapacity[0].Trim() != "")
                    {
                        if (BudgetChartIsCumulative && ChartBudget.Series[1].Points.Count != 0)
                        {
                            ChartBudget.Series[1].Points.AddXY(
                                DateTime.Today.AddMonths(j).Year.ToString() + "-" + DateTime.Today.AddMonths(j).Month.ToString(),
                                Convert.ToDouble(ChartBudget.Series[1].Points[ChartBudget.Series[1].Points.Count - 1].YValues[0]) +
                                Convert.ToDouble(VariationProductionCapacity[0].Trim()));
                        }
                        else
                        {
                            ChartBudget.Series[1].Points.AddXY(
                                DateTime.Today.AddMonths(j).Year.ToString() + "-" +
                                DateTime.Today.AddMonths(j).Month.ToString(),
                                Convert.ToDouble(VariationProductionCapacity[0].Trim()));
                        }
                    }
                    else
                        AddZeroIntoSeries(1, j);
                }
                else
                    AddZeroIntoSeries(1, j);

                
            }

            void AddZeroIntoSeries(int SeriesID, int NoOfMonths)
            {
                if (BudgetChartIsCumulative && ChartBudget.Series[SeriesID].Points.Count != 0)
                {
                    ChartBudget.Series[SeriesID].Points.AddXY(
                        DateTime.Today.AddMonths(NoOfMonths).Year.ToString() + "-" +
                        DateTime.Today.AddMonths(NoOfMonths).Month.ToString(),
                        Convert.ToDouble(ChartBudget.Series[SeriesID].Points
                        [ChartBudget.Series[SeriesID].Points.Count - 1].YValues[0]));
                }
                else
                {
                    ChartBudget.Series[SeriesID].Points.AddXY(
                        DateTime.Today.AddMonths(NoOfMonths).Year.ToString() +
                        "-" + DateTime.Today.AddMonths(NoOfMonths).Month.ToString(), 0);
                }
            }
        }

        string SelectedDate()
        {
            return txtDate.Year.ToString() + "-" + txtDate.Month.ToString() + "-" + txtDate.Day.ToString();
        }

        private void DashBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnCostAndIncomeChartType_Click(object sender, EventArgs e)
        {
            ContextMenuStrip_CostAndIncome.Show();
            ContextMenuStrip_CostAndIncome.Top = this.Top + guna2Panel4.Top + guna2CircleButton6.Bottom;
            ContextMenuStrip_CostAndIncome.Left = this.Left + guna2Panel4.Left + guna2CircleButton6.Left;
        }

        private void ContextMenuStrip_CostAndIncome_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == NonCumulate && !NonCumulate.Checked) 
            {
                NonCumulate.Checked = true;
                Cumulate.Checked = BudgetChartIsCumulative = false;
                CreateChartProductionCostAndConsumptionIncome();
            }
            else if (e.ClickedItem == Cumulate && !Cumulate.Checked)
            {
                NonCumulate.Checked = false;
                Cumulate.Checked = BudgetChartIsCumulative = true;
                CreateChartProductionCostAndConsumptionIncome();
            }
        }

        private void btnNavigation_Click(object sender, EventArgs e)
        {
            Guna2CircleButton btnNavigation = (Guna2CircleButton)sender;
            multiFunctions.NavigateTo(btnNavigation.Tag.ToString(), this);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            multiFunctions.NavigateTo(btnClose.Tag.ToString(), this);
        }
    }
}
